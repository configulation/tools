// See https://aka.ms/new-console-template for more information

using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;

const byte MessageTypeConnectionInfo = 8;

int port = 9100;
if (args != null && args.Length > 0)
{
    foreach (var a in args)
    {
        if (a.StartsWith("--port=", StringComparison.OrdinalIgnoreCase))
        {
            if (int.TryParse(a.Substring("--port=".Length), out var p))
            {
                port = p;
            }
        }
        else if (int.TryParse(a, out var p2))
        {
            port = p2;
        }
    }
}

var waitingHosts = new ConcurrentDictionary<string, ClientSession>(StringComparer.OrdinalIgnoreCase);
var waitingControllers = new ConcurrentDictionary<string, ClientSession>(StringComparer.OrdinalIgnoreCase);
var activePairs = new ConcurrentDictionary<string, PairSession>(StringComparer.OrdinalIgnoreCase);

var cts = new CancellationTokenSource();
Console.CancelKeyPress += (s, e) =>
{
    e.Cancel = true;
    cts.Cancel();
};

var listener = new TcpListener(IPAddress.Any, port);
listener.Start();
Console.WriteLine($"RelayServer listening on 0.0.0.0:{port}");

await AcceptLoopAsync(listener, cts.Token);

async Task AcceptLoopAsync(TcpListener tcpListener, CancellationToken token)
{
    while (!token.IsCancellationRequested)
    {
        TcpClient client;
        try
        {
            client = await tcpListener.AcceptTcpClientAsync(token);
        }
        catch
        {
            break;
        }

        _ = Task.Run(() => HandleClientAsync(client, token), token);
    }

    try
    {
        tcpListener.Stop();
    }
    catch
    {
    }
}

async Task HandleClientAsync(TcpClient tcpClient, CancellationToken token)
{
    var stream = tcpClient.GetStream();
    var remoteEndPoint = tcpClient.Client.RemoteEndPoint?.ToString() ?? "unknown";
    ClientSession? session = null;

    try
    {
        var handshake = await ReadFrameAsync(stream, token);
        if (handshake == null)
        {
            CloseQuietly(tcpClient);
            return;
        }

        if (handshake.Value.Type != MessageTypeConnectionInfo)
        {
            CloseQuietly(tcpClient);
            return;
        }

        var info = Encoding.UTF8.GetString(handshake.Value.Payload);
        if (!TryParseHandshake(info, out var role, out var code))
        {
            CloseQuietly(tcpClient);
            return;
        }

        session = new ClientSession(tcpClient, stream, role, code, remoteEndPoint);
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {remoteEndPoint} handshake role={role} code={code}");

        if (role == ClientRole.Host)
        {
            if (waitingHosts.TryGetValue(code, out var old))
            {
                old.Close();
            }

            waitingHosts[code] = session;

            if (waitingControllers.TryRemove(code, out var ctrl))
            {
                waitingHosts.TryRemove(code, out _);
                await StartPairAsync(code, session, ctrl, token);
                return;
            }
        }
        else
        {
            if (waitingControllers.TryGetValue(code, out var old))
            {
                old.Close();
            }

            waitingControllers[code] = session;

            if (waitingHosts.TryRemove(code, out var host))
            {
                waitingControllers.TryRemove(code, out _);
                await StartPairAsync(code, host, session, token);
                return;
            }
        }

        while (!token.IsCancellationRequested && tcpClient.Connected)
        {
            await Task.Delay(1000, token);
        }
    }
    catch
    {
    }
    finally
    {
        try
        {
            if (session != null)
            {
                if (session.Role == ClientRole.Host)
                {
                    if (waitingHosts.TryGetValue(session.Code, out var cur) && ReferenceEquals(cur, session))
                    {
                        waitingHosts.TryRemove(session.Code, out _);
                    }
                }
                else
                {
                    if (waitingControllers.TryGetValue(session.Code, out var cur) && ReferenceEquals(cur, session))
                    {
                        waitingControllers.TryRemove(session.Code, out _);
                    }
                }
            }
        }
        catch
        {
        }
        CloseQuietly(tcpClient);
    }
}

async Task StartPairAsync(string code, ClientSession host, ClientSession controller, CancellationToken token)
{
    var pairCts = CancellationTokenSource.CreateLinkedTokenSource(token);
    var pair = new PairSession(code, host, controller, pairCts);

    if (activePairs.TryGetValue(code, out var oldPair))
    {
        oldPair.Close();
    }

    activePairs[code] = pair;

    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] paired code={code} host={host.RemoteEndPoint} ctrl={controller.RemoteEndPoint}");

    try
    {
        await WriteFrameAsync(host.Stream, MessageTypeConnectionInfo, Encoding.UTF8.GetBytes("PAIRED"), pairCts.Token);
        await WriteFrameAsync(controller.Stream, MessageTypeConnectionInfo, Encoding.UTF8.GetBytes("PAIRED"), pairCts.Token);
    }
    catch
    {
        pair.Close();
        activePairs.TryRemove(code, out _);
        return;
    }

    var t1 = PumpAsync(host.Stream, controller.Stream, pairCts.Token);
    var t2 = PumpAsync(controller.Stream, host.Stream, pairCts.Token);

    await Task.WhenAny(t1, t2);

    pair.Close();
    activePairs.TryRemove(code, out _);

    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] unpaired code={code}");
}

static async Task WriteFrameAsync(NetworkStream stream, byte type, byte[] payload, CancellationToken token)
{
    payload ??= Array.Empty<byte>();
    var header = new byte[5];
    header[0] = type;
    Array.Copy(BitConverter.GetBytes(payload.Length), 0, header, 1, 4);
    await stream.WriteAsync(header, 0, header.Length, token);
    if (payload.Length > 0)
    {
        await stream.WriteAsync(payload, 0, payload.Length, token);
    }
    await stream.FlushAsync(token);
}

static async Task PumpAsync(NetworkStream from, NetworkStream to, CancellationToken token)
{
    var buffer = new byte[64 * 1024];
    while (!token.IsCancellationRequested)
    {
        int read;
        try
        {
            read = await from.ReadAsync(buffer, 0, buffer.Length, token);
        }
        catch
        {
            break;
        }

        if (read <= 0)
        {
            break;
        }

        try
        {
            await to.WriteAsync(buffer, 0, read, token);
            await to.FlushAsync(token);
        }
        catch
        {
            break;
        }
    }
}

static bool TryParseHandshake(string text, out ClientRole role, out string code)
{
    role = ClientRole.Controller;
    code = string.Empty;
    if (string.IsNullOrWhiteSpace(text))
    {
        return false;
    }

    var idx = text.IndexOf('|');
    if (idx <= 0 || idx >= text.Length - 1)
    {
        return false;
    }

    var roleText = text.Substring(0, idx).Trim();
    var codeText = text.Substring(idx + 1).Trim();
    if (string.IsNullOrWhiteSpace(codeText))
    {
        return false;
    }

    if (roleText.Equals("H", StringComparison.OrdinalIgnoreCase))
    {
        role = ClientRole.Host;
    }
    else if (roleText.Equals("C", StringComparison.OrdinalIgnoreCase))
    {
        role = ClientRole.Controller;
    }
    else
    {
        return false;
    }

    code = codeText;
    return true;
}

static async Task<(byte Type, byte[] Payload)?> ReadFrameAsync(NetworkStream stream, CancellationToken token)
{
    var header = new byte[5];
    if (!await ReadExactAsync(stream, header, token))
    {
        return null;
    }

    byte type = header[0];
    int len = BitConverter.ToInt32(header, 1);
    if (len < 0 || len > 1024 * 1024)
    {
        return null;
    }

    var payload = new byte[len];
    if (len > 0)
    {
        if (!await ReadExactAsync(stream, payload, token))
        {
            return null;
        }
    }

    return (type, payload);
}

static async Task<bool> ReadExactAsync(NetworkStream stream, byte[] buffer, CancellationToken token)
{
    int offset = 0;
    while (offset < buffer.Length)
    {
        int read;
        try
        {
            read = await stream.ReadAsync(buffer, offset, buffer.Length - offset, token);
        }
        catch
        {
            return false;
        }

        if (read <= 0)
        {
            return false;
        }

        offset += read;
    }
    return true;
}

static void CloseQuietly(TcpClient client)
{
    try
    {
        client.Close();
    }
    catch
    {
    }
}

sealed class PairSession
{
    public string Code { get; }
    public ClientSession Host { get; }
    public ClientSession Controller { get; }
    private readonly CancellationTokenSource cts;

    public PairSession(string code, ClientSession host, ClientSession controller, CancellationTokenSource cts)
    {
        Code = code;
        Host = host;
        Controller = controller;
        this.cts = cts;
    }

    public void Close()
    {
        try
        {
            cts.Cancel();
        }
        catch
        {
        }

        Host.Close();
        Controller.Close();

        try
        {
            cts.Dispose();
        }
        catch
        {
        }
    }
}

sealed class ClientSession
{
    private readonly TcpClient tcpClient;
    public NetworkStream Stream { get; }
    public ClientRole Role { get; }
    public string Code { get; }
    public string RemoteEndPoint { get; }

    public ClientSession(TcpClient tcpClient, NetworkStream stream, ClientRole role, string code, string remoteEndPoint)
    {
        this.tcpClient = tcpClient;
        Stream = stream;
        Role = role;
        Code = code;
        RemoteEndPoint = remoteEndPoint;
    }

    public void Close()
    {
        try
        {
            tcpClient.Close();
        }
        catch
        {
        }
    }
}

enum ClientRole
{
    Host = 1,
    Controller = 2
}

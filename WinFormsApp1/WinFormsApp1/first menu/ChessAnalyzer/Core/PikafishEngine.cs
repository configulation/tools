using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WinFormsApp1.first_menu.ChessAnalyzer.Core
{
    /// <summary>
    /// Pikafish 引擎管理类
    /// </summary>
    public class PikafishEngine : IDisposable
    {
        private Process _process;
        private StreamWriter _input;
        private StreamReader _output;
        private bool _isReady = false;
        private bool _disposed = false;
        private Thread _outputThread;

        // 事件：引擎准备完毕
        public event EventHandler EngineReady;
        // 事件：收到最佳走法
        public event EventHandler<BestMoveEventArgs> BestMoveReceived;
        // 事件：收到分析信息
        public event EventHandler<EngineInfoEventArgs> InfoReceived;
        // 事件：输出日志
        public event EventHandler<string> LogReceived;

        public bool IsReady => _isReady;

        /// <summary>
        /// 启动引擎
        /// </summary>
        public async Task<bool> StartAsync(string enginePath, string nnuePath = null)
        {
            try
            {
                Log($"正在启动引擎: {enginePath}");

                if (!File.Exists(enginePath))
                {
                    throw new FileNotFoundException($"引擎文件不存在: {enginePath}");
                }

                // 创建进程配置
                _process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = enginePath,
                        UseShellExecute = false,
                        RedirectStandardInput = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true,
                        StandardOutputEncoding = Encoding.UTF8,
                        StandardErrorEncoding = Encoding.UTF8,
                        WorkingDirectory = Path.GetDirectoryName(enginePath)
                    }
                };

                // 启动进程
                _process.Start();
                _input = _process.StandardInput;
                _output = _process.StandardOutput;

                // 启动输出读取线程
                _outputThread = new Thread(ReadOutputLoop)
                {
                    IsBackground = true,
                    Name = "EngineOutputReader"
                };
                _outputThread.Start();

                // 初始化引擎
                await InitializeEngineAsync(nnuePath);

                Log("✓ 引擎启动成功");
                return true;
            }
            catch (Exception ex)
            {
                Log($"✗ 启动引擎失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 初始化引擎（UCI 协议握手）
        /// </summary>
        private async Task InitializeEngineAsync(string nnuePath)
        {
            // 发送 uci 命令
            SendCommand("uci");
            await Task.Delay(200);

            // 设置选项
            if (!string.IsNullOrEmpty(nnuePath) && File.Exists(nnuePath))
            {
                SendCommand($"setoption name EvalFile value {nnuePath}");
            }

            SendCommand("setoption name Threads value 4");
            SendCommand("setoption name Hash value 256");

            // 检查是否准备好
            SendCommand("isready");
            
            // 等待 readyok
            for (int i = 0; i < 50; i++)
            {
                await Task.Delay(100);
                if (_isReady) break;
            }

            if (!_isReady)
            {
                throw new TimeoutException("引擎初始化超时");
            }

            EngineReady?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 发送命令到引擎
        /// </summary>
        private void SendCommand(string command)
        {
            if (_input == null || _disposed)
            {
                throw new InvalidOperationException("引擎未启动或已释放");
            }

            Log($">> {command}");
            _input.WriteLine(command);
            _input.Flush();
        }

        /// <summary>
        /// 读取引擎输出（循环）
        /// </summary>
        private void ReadOutputLoop()
        {
            try
            {
                while (!_disposed && _process != null && !_process.HasExited)
                {
                    string line = _output.ReadLine();
                    if (string.IsNullOrEmpty(line)) continue;

                    Log($"<< {line}");
                    ParseEngineLine(line);
                }
            }
            catch (Exception ex)
            {
                if (!_disposed)
                {
                    Log($"读取输出错误: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// 解析引擎输出行
        /// </summary>
        private void ParseEngineLine(string line)
        {
            try
            {
                if (line.Contains("readyok"))
                {
                    _isReady = true;
                }
                else if (line.StartsWith("bestmove"))
                {
                    // 格式: bestmove h2e2 ponder h9g7
                    var parts = line.Split(' ');
                    string bestMove = parts.Length > 1 ? parts[1] : null;
                    string ponderMove = parts.Length > 3 ? parts[3] : null;

                    BestMoveReceived?.Invoke(this, new BestMoveEventArgs
                    {
                        BestMove = bestMove,
                        PonderMove = ponderMove
                    });
                }
                else if (line.StartsWith("info"))
                {
                    var info = ParseInfoLine(line);
                    if (info != null)
                    {
                        InfoReceived?.Invoke(this, new EngineInfoEventArgs { Info = info });
                    }
                }
            }
            catch (Exception ex)
            {
                Log($"解析输出错误: {ex.Message}");
            }
        }

        /// <summary>
        /// 解析 info 行
        /// </summary>
        private EngineInfo ParseInfoLine(string line)
        {
            try
            {
                var info = new EngineInfo();
                var parts = line.Split(' ');

                for (int i = 1; i < parts.Length; i++)
                {
                    switch (parts[i])
                    {
                        case "depth":
                            if (i + 1 < parts.Length)
                                info.Depth = int.Parse(parts[++i]);
                            break;
                        case "score":
                            if (i + 2 < parts.Length && parts[i + 1] == "cp")
                            {
                                info.Score = int.Parse(parts[i + 2]);
                                i += 2;
                            }
                            break;
                        case "nodes":
                            if (i + 1 < parts.Length)
                                info.Nodes = long.Parse(parts[++i]);
                            break;
                        case "nps":
                            if (i + 1 < parts.Length)
                                info.Nps = long.Parse(parts[++i]);
                            break;
                        case "time":
                            if (i + 1 < parts.Length)
                                info.TimeMs = int.Parse(parts[++i]);
                            break;
                        case "pv":
                            // 主要变化（后续所有走法）
                            var pvMoves = new System.Collections.Generic.List<string>();
                            for (int j = i + 1; j < parts.Length; j++)
                            {
                                pvMoves.Add(parts[j]);
                            }
                            info.PrincipalVariation = string.Join(" ", pvMoves);
                            i = parts.Length;
                            break;
                    }
                }

                return info;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 设置局面并分析
        /// </summary>
        public void AnalyzePosition(string fen, int depth = 18, int timeMs = 2000)
        {
            if (!_isReady)
            {
                throw new InvalidOperationException("引擎未准备好");
            }

            // 设置局面
            SendCommand($"position fen {fen}");

            // 开始计算（使用深度限制）
            SendCommand($"go depth {depth}");
        }

        /// <summary>
        /// 停止计算
        /// </summary>
        public void Stop()
        {
            if (_isReady)
            {
                SendCommand("stop");
            }
        }

        /// <summary>
        /// 日志输出
        /// </summary>
        private void Log(string message)
        {
            LogReceived?.Invoke(this, $"[{DateTime.Now:HH:mm:ss}] {message}");
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;

            try
            {
                if (_input != null)
                {
                    SendCommand("quit");
                }

                if (_process != null && !_process.HasExited)
                {
                    if (!_process.WaitForExit(2000))
                    {
                        _process.Kill();
                    }
                }
            }
            catch { }
            finally
            {
                _input?.Dispose();
                _output?.Dispose();
                _process?.Dispose();
            }

            Log("引擎已关闭");
        }
    }

    /// <summary>
    /// 最佳走法事件参数
    /// </summary>
    public class BestMoveEventArgs : EventArgs
    {
        public string BestMove { get; set; }
        public string PonderMove { get; set; }
    }

    /// <summary>
    /// 引擎信息事件参数
    /// </summary>
    public class EngineInfoEventArgs : EventArgs
    {
        public EngineInfo Info { get; set; }
    }

    /// <summary>
    /// 引擎分析信息
    /// </summary>
    public class EngineInfo
    {
        public int Depth { get; set; }                  // 搜索深度
        public int Score { get; set; }                  // 评分（厘兵）
        public long Nodes { get; set; }                 // 搜索节点数
        public long Nps { get; set; }                   // 每秒节点数
        public int TimeMs { get; set; }                 // 思考时间（毫秒）
        public string PrincipalVariation { get; set; }  // 主要变化

        /// <summary>
        /// 获取评分显示文本
        /// </summary>
        public string GetScoreText()
        {
            double score = Score / 100.0;
            return score >= 0 ? $"+{score:F2}" : $"{score:F2}";
        }
    }
}

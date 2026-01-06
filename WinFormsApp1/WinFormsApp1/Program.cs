namespace WinFormsApp1;

internal static class Program
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // .NET 8 内置 DPI 支持，通过 csproj 的 ApplicationHighDpiMode 配置
        ApplicationConfiguration.Initialize();
        //Application.Run(new FrmSoftWareAnalysis());
        Application.Run(new FrmMain());
        //Application.Run(new FrmRemoteControl());
    }
}
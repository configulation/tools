using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WinFormsApp1.first_menu;
using WinFormsApp1.first_menu.RemoteControl;

namespace WinFormsApp1
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // 修复HighDpiMode问题 - updated by cqy at 2025-9-8
            DpiAwareness.Enable();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new FrmSoftWareAnalysis());
            Application.Run(new FrmMain());
            //Application.Run(new FrmRemoteControl());
        }
    }

    internal static class DpiAwareness
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetProcessDPIAware();

        [DllImport("shcore.dll")]
        private static extern int SetProcessDpiAwareness(int value);

        public static void Enable()
        {
            try
            {
                SetProcessDpiAwareness(2);
            }
            catch
            {
            }

            try
            {
                SetProcessDPIAware();
            }
            catch
            {
            }
        }
    }
}
using Chess.SubWindow;
using Chess.Test;
using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Chess
{
    /// <summary>
    /// 主窗口类
    /// </summary>
    public partial class MainWindow : Window
    {

        public static int menuItem;
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 软件关闭退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMainWindowClose(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Settings.Default.Save(); // 保存用户更改的设置
            Environment.Exit(0); // 关闭所有窗口，并释放所有资源，包括相关辅助窗口。
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            menuItem = 0;
            ReturnButton.Visibility = Visibility.Hidden;
            // 应用用户上次选择的主题
            string[] themfiles = { "Orange", "Green", "Blue", "Violet", "Null", "ChinaRed", "DarkGreen", "DarkViolet", "Wood", "Wood_Light" };
            int index = (Settings.Default.ThemsIndex < themfiles.Length) ? Settings.Default.ThemsIndex : 0;
            Settings.Default.ThemsIndex = index;
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri(@"/Thems/Dictionary_" + themfiles[index] + ".xaml", UriKind.Relative) });
        }
        private void ReturnMainMenu(object sender, RoutedEventArgs e)
        {
            MainFrame.Source = null;
            MainMenu.Visibility = Visibility.Visible;
            ReturnButton.Visibility = Visibility.Hidden;
            menuItem = 0;
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void AllwayOnTop(object sender, RoutedEventArgs e)
        {
            this.Topmost = !this.Topmost;
        }

        private void MainMenuClick(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            switch (btn.Tag.ToString())
            {
                case "1":
                    menuItem = GlobalValue.PERSON_PC;
                    MainFrame.Source = new Uri("QiPanPage.xaml", UriKind.RelativeOrAbsolute);
                    break;
                case "2":
                    menuItem = GlobalValue.PC_PC;
                    MainFrame.Source = new Uri("QiPanPage.xaml", UriKind.RelativeOrAbsolute);
                    break;
                case "3":
                    menuItem = GlobalValue.FREE_DAPU;
                    MainFrame.Source = new Uri("QiPanPage.xaml", UriKind.RelativeOrAbsolute);
                    break;
                case "4":
                    menuItem = GlobalValue.QIPU_RECORD;
                    MainFrame.Source = new Uri("QiPanPage.xaml", UriKind.RelativeOrAbsolute);
                    break;
                case "5":
                    menuItem = GlobalValue.CANJU_DESIGN;
                    MainFrame.Source = new Uri("CanJuSheJi.xaml", UriKind.RelativeOrAbsolute);
                    break;
                case "6":
                    menuItem = GlobalValue.CANJU_POJIE;
                    MainFrame.Source = new Uri("QiPanPage.xaml", UriKind.RelativeOrAbsolute);
                    break;
                case "7":
                    menuItem = 7;
                    MainFrame.Source = new Uri("GuPuInputPage.xaml", UriKind.RelativeOrAbsolute);
                    break;
                case "8":
                    menuItem = 8;
                    MainFrame.Source = new Uri("GuPuLianXiPage.xaml", UriKind.RelativeOrAbsolute);
                    break;
                default:
                    menuItem = 0;
                    MainMenu.Visibility = Visibility.Visible;
                    ReturnButton.Visibility = Visibility.Hidden;
                    break;
            }
            if (menuItem is >= 1 and <= 8)
            {
                MainMenu.Visibility = Visibility.Hidden;
                ReturnButton.Visibility = Visibility.Visible;
                WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }
        }
        
        /// <summary>
        /// 打开用户设置窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SystemSetup(object sender, RoutedEventArgs e)
        {
            SystemSetting setWindow = new();
            setWindow.ShowDialog();
        }

        private void WindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Title = $"China Chess 单机版 V5.0  {Width} x {Height}";
        }
        /// <summary>
        /// 键盘输入处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {

            if (e.Key == System.Windows.Input.Key.Delete)
            {
                if ((menuItem == GlobalValue.CANJU_DESIGN) && (GlobalValue.CurrentQiZi is >= 0 and < 32))
                {
                    GlobalValue.qiZiArray[GlobalValue.CurrentQiZi].SetInitPosition();
                    GlobalValue.CurrentQiZi = 100;
                    for (int i = 0; i <= 8; i++)
                    {
                        for (int j = 0; j <= 9; j++)
                        {
                            GlobalValue.pathPointImage[i, j].HasPoint = false; // 走棋后，隐藏走棋路径
                        }
                    }
                }
            }
        }

        private void OpenTestWindow(object sender, RoutedEventArgs e)
        {
            TestWindow tw=new TestWindow();
            tw.Show();
        }
    }
    /// <summary>
    /// 转换器。将文件名字符串转换为BitMapImage实例。
    /// </summary>
    public class StringToImageSourceConverter : IValueConverter
    {
        #region Converter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string path = (string)value;
            if (!string.IsNullOrEmpty(path))
            {
                FileInfo fileInfo = new(path);
                string fileFullName = AppDomain.CurrentDomain.BaseDirectory + "/picture/BackGround/" + fileInfo.Name;
                if (!File.Exists(fileFullName))  // 如果文件不存在，则使用默认背景图。
                    fileFullName = AppDomain.CurrentDomain.BaseDirectory + "/picture/BackGround/山水之间.jpeg";
                return new BitmapImage(new Uri(fileFullName, UriKind.Absolute));
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
        #endregion
    }
}

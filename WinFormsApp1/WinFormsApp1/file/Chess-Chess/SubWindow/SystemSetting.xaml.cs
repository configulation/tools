using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Chess.SubWindow
{
    /// <summary>
    /// SystemSetting.xaml 的交互逻辑
    /// </summary>
    public partial class SystemSetting : Window
    {
        public SystemSetting()
        {
            InitializeComponent();
        }
        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            //MoveDelayTime.Value = Settings.Default.MoveDelayTime;
            //ArrowsShowOrHidden.IsChecked = Settings.Default.ArrowVisable;
            //ArrowMaxNumSlider.Value = Settings.Default.ArrowsMaxNum;
            CustomClass.HuoHuan huoHuan = new CustomClass.HuoHuan();
            DongHuaGrid.Children.Add(huoHuan);
        }
        private void OnWindowUnloaded(object sender, RoutedEventArgs e)
        {
            Settings.Default.Save();
        }
        /// <summary>
        /// 选择窗口背景图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectBKImage(object sender, RoutedEventArgs e)
        {
            string imageDefaultPath = $"{AppDomain.CurrentDomain.BaseDirectory}picture\\BackGround\\";
            // 注意Debug和Release模式的区别，所选模式模式不同，AppDomain.CurrentDomain.BaseDirectory对应的文件夹也不同。它更不是原代码所在的文件夹

            OpenFileDialog openFileDialog = new()
            {
                Filter = "图像文件|*.jpg;*.jpeg;*.png;*.bmp;|所有文件|*.*",
                InitialDirectory = imageDefaultPath,
                DefaultExt = string.Empty,
                RestoreDirectory = true,
                Title = "选择窗口背景图片"
            };
            if ((bool)openFileDialog.ShowDialog())
            {
                FileInfo sourceFile = new(openFileDialog.FileName);
                string targetFile = imageDefaultPath + sourceFile.Name;
                if (!File.Exists(targetFile))
                {
                    // 如果在{AppDomain.CurrentDomain.BaseDirectory}\picture\BackGround\文件夹下没有该文件，则将图片文件复制到该文件夹。
                    File.Copy(sourceFile.FullName, targetFile, true);
                }
                Settings.Default.mainBKImage = sourceFile.Name;
                Settings.Default.Save();
            }
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {

        }
        /// <summary>
        /// 选择主题
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ThemsChangeed(object sender, SelectionChangedEventArgs e)
        {
            // 要想使用新主题，xaml中应使用DynamicResource，而不是StaticResource，否则，主题不会生效
            string[] themfiles = {"Orange","Green","Blue","Violet","Null","ChinaRed","DarkGreen","DarkViolet","Wood", "Wood_Light" };
            int index = (thems_combox.SelectedIndex < themfiles.Length) ? thems_combox.SelectedIndex : 0;
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri(@"/Thems/Dictionary_" + themfiles[index]+".xaml", UriKind.Relative) });

        }

        private void OpenColorWindow(object sender, RoutedEventArgs e)
        {
            SystemColor systemColor = new SystemColor();
            systemColor.Show();
        }
    }
}

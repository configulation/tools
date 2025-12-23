using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Chess.OpenSource;

namespace Chess
{
    /// <summary>
    /// QiPanPage.xaml 的交互逻辑
    /// </summary>
    public partial class CanJuSheJi : Page
    {

        public CanJuSheJi()
        {
            InitializeComponent();

        }
        /// <summary>
        /// 主窗口载入时，初始化自定义控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainFormLoaded(object sender, RoutedEventArgs e)
        {
            GlobalValue.yuanWeiZhi = new QiZi(); // 棋子原位置图片
            _ = qiziCanvas.Children.Add(GlobalValue.yuanWeiZhi);

            for (int i = 0; i < 32; i++)
            {
                GlobalValue.qiZiArray[i] = new QiZi(i);  // 初始化32个棋子
                _ = qiziCanvas.Children.Add(GlobalValue.qiZiArray[i]);
            }
            for (int i = 0; i <= 8; i++)
            {
                for (int j = 0; j <= 9; j++)
                {
                    GlobalValue.pathPointImage[i, j] = new PathPoint(i, j);  // 走棋路径
                    _ = qiziCanvas.Children.Add(GlobalValue.pathPointImage[i, j]);
                }
            }
            GlobalValue.IsQiPanFanZhuan = false; // 棋盘翻转，初始为未翻转，黑方在上，红方在下

            for (int i = 0; i <= 8; i++)
            {
                for (int j = 0; j <= 9; j++)
                {
                    GlobalValue.QiPan[i, j] = -1; // 棋盘数据清空
                    GlobalValue.pathPointImage[i, j].HasPoint = false; // 走棋路径点清空
                }
            }

        }

        /// <summary>
        /// 保存残局棋谱
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveCanJu(object sender, RoutedEventArgs e)
        {
            string fenstr = Engine.XQEngine.QiPanDataToFenStr_header();

            Dictionary<string, object> dic = new()
            {
                { "Name", CanJuName.Text },
                { "Comment", Comment.Text },
                { "FENstring", fenstr }
            };
            int rows = SqliteHelper.Insert("CanJuKu", dic);
            if (rows > 0)
            {
                SaveOk.Visibility = Visibility.Visible;
            }
            else
            {
                SaveNotOk.Visibility = Visibility.Visible;
            }
            GlobalValue.Delay(1000);
            SaveOk.Visibility = Visibility.Hidden;
            SaveNotOk.Visibility = Visibility.Hidden;
        }
        /// <summary>
        /// 清空棋盘
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearQiPan(object sender, RoutedEventArgs e)
        {
            CanJuName.Text = "";
            Comment.Text = "";
            for (int i = 0; i < 32; i++)
            {
                GlobalValue.qiZiArray[i].SetInitPosition();
            }
            for (int i = 0; i <= 8; i++)
            {
                for (int j = 0; j <= 9; j++)
                {
                    GlobalValue.QiPan[i, j] = -1; // 棋盘数据清空
                    GlobalValue.pathPointImage[i, j].HasPoint = false; // 走棋路径点清空
                }
            }
        }

        private void CanJuName_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tbx = sender as TextBox;
            tbx.SelectAll();
            e.Handled = true;
        }

        private void OnKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Delete)
            {
                if (GlobalValue.CurrentQiZi is >=0 and < 32)
                {
                    GlobalValue.qiZiArray[GlobalValue.CurrentQiZi].SetInitPosition();
                }
            }
        }
    }
}

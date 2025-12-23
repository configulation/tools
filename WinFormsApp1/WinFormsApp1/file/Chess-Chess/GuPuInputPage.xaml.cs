using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Chess.OpenSource;
using Newtonsoft.Json;
using Chess.SubWindow;
using Chess.CustomClass;
using System.Data;
using System.Diagnostics;
using System.Windows.Threading;
using System;

namespace Chess
{
    /// <summary>
    /// QiPanPage.xaml 的交互逻辑
    /// </summary>
    public partial class GuPuInputPage : Page
    {
        private static DataTable GuPuNameList;

        public GuPuInputPage()
        {
            InitializeComponent();
            #region 添加界面控件元素
            GlobalValue.yuanWeiZhi = new();
            _ = qiziCanvas.Children.Add(GlobalValue.yuanWeiZhi);// 棋子原位置图片
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
            GlobalValue.BestMoveInfo = new()
            {
                Text = "",
                Foreground = Brushes.Black,
                FontSize = 14,
                Width = 300,
                Margin = new Thickness(10, 10, 10, 10),
                Padding = new Thickness(5, 5, 5, 5),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                TextWrapping = TextWrapping.Wrap,

            };
            //Infomation_board.Children.Add(GlobalValue.BestMoveInfo); // 下一步最佳着法提示信息
            GlobalValue.jiangJunTiShi = new()
            {
                Text = "战况信息：",
                Foreground = Brushes.Black,
                FontSize = 14,
                Width = 300,
                Margin = new Thickness(10, 10, 10, 10),
                Padding = new Thickness(5, 5, 5, 5),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                TextWrapping = TextWrapping.Wrap,
            };
            //Infomation_board.Children.Add(GlobalValue.jiangJunTiShi);// 将军状态文字提示
            GlobalValue.JueShaGrid = new();
            _ = JueshaGrid.Children.Add(GlobalValue.JueShaGrid); // 绝杀图片

            GlobalValue.arrows = new();
            _ = DrawGrid.Children.Add(GlobalValue.arrows.grid); // 走棋提示箭头



            #endregion
        }
        /// <summary>
        /// 页面载入时，初始化参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainFormLoaded(object sender, RoutedEventArgs e)
        {

            GlobalValue.IsQiPanFanZhuan = false; // 棋盘翻转，初始为未翻转，黑方在上，红方在下
            QiPanChange(false);
            GlobalValue.Reset();
            GuPuNameList = SqliteHelper.Select("GuPuList", "rowid,*");
            gupunameCombox.ItemsSource = GuPuNameList.DefaultView;
            QiPuDataGrid.ItemsSource=Qipu.QiPuList;
        }

        /// <summary>
        /// 重新开局
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetBtnClick(object sender, RoutedEventArgs e)
        {
            GlobalValue.Reset();
        }

        /// <summary>
        /// 点击“棋盘翻转”button时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFanZhuanQiPan(object sender, RoutedEventArgs e)
        {
            GlobalValue.IsQiPanFanZhuan = !GlobalValue.IsQiPanFanZhuan;
            QiPanChange(GlobalValue.IsQiPanFanZhuan);  // 更换棋盘
            GlobalValue.yuanWeiZhi.FanZhuanPosition(); // 走棋原位置图片刷新
            foreach (QiZi item in GlobalValue.qiZiArray)
            {
                item.FanZhuanPosition(); // 棋盘翻转后，刷新显示所有棋子
            }
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    GlobalValue.pathPointImage[i, j].FanZhuPosition(); // 棋盘翻转后，刷新显示所有走棋路径
                }
            }
            GlobalValue.arrows.HideAllPath(); //  隐藏提示箭头
        }

        /// <summary>
        /// 棋盘翻转时更换棋盘
        /// </summary>
        /// <param name="isChange">false=上黑下红，true=上红下黑</param>
        private void QiPanChange(bool isChange)
        {
            if (isChange)
            {
                qipan_topBlack.Visibility = Visibility.Hidden;
                qipan_topRed.Visibility = Visibility.Visible;
                redSideRect.Margin = new Thickness(30, 260, 0, 0);
                blackSideRect.Margin = new Thickness(30, 500, 0, 0);
            }
            else
            {
                qipan_topBlack.Visibility = Visibility.Visible;
                qipan_topRed.Visibility = Visibility.Hidden;
                redSideRect.Margin = new Thickness(30, 500, 0, 0);
                blackSideRect.Margin = new Thickness(30, 260, 0, 0);
            }
        }

        /// <summary>
        /// 保存棋谱
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveQiPu(object sender, RoutedEventArgs e)
        {
            Save_Window window = new();
            _ = window.ShowDialog();
        }

        /// <summary>
        /// 上一步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HuiQiButton(object sender, RoutedEventArgs e)
        {
            GlobalValue.HuiQi();
            if (MainWindow.menuItem == GlobalValue.PERSON_PC || MainWindow.menuItem == GlobalValue.CANJU_POJIE)
            {
                GlobalValue.HuiQi();
            }
        }

        /// <summary>
        /// 下一步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NextStep(object sender, RoutedEventArgs e)
        {
            int childCount = GlobalValue.NextStep();
            if (childCount > 1)
            {
                ChildSelecteWindow selectPage = new(childCount);
                selectPage.ShowDialog();
                string childid = Clipboard.GetText();
                if (childid != null && childid.Length == 1)
                {
                    GlobalValue.NextStep(childid);
                }
            }
        }

        /// <summary>
        /// 打开复盘窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenFuPanWindow(object sender, RoutedEventArgs e)
        {
            if (GlobalValue.qiPuKuForm.IsVisible)
            {
                GlobalValue.qiPuKuForm.Close();
            }
            else
            {
                GlobalValue.qiPuKuForm = new();
                GlobalValue.qiPuKuForm.Show();
            }
        }
        /// <summary>
        /// 添加注释
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddRemark(object sender, RoutedEventArgs e)
        {
            string str = GlobalValue.qiPuRecordRoot.Cursor.Remarks;
            if (str == null || str.Length < 1)
            {
                str = GlobalValue.qiPuRecordRoot.Cursor.Cn;
            }
        }
        

        private void UpdateQiPu(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Window_QiPu.GetRowid()))
            {
                GlobalValue.qiPuSimpleRecordRoot = GlobalValue.ConvertQiPuToSimple(GlobalValue.qiPuRecordRoot);  // 更新简易棋谱记录
                Dictionary<string, object> dic = new()
                {
                    { "jsonrecord", JsonConvert.SerializeObject(GlobalValue.qiPuSimpleRecordRoot) }
                };
                if (SqliteHelper.Update("mybook", $"rowid={Window_QiPu.GetRowid()}", dic) > 0)
                {
                    MessageBox.Show("数据保存成功！", "提示");
                }
                else
                {
                    MessageBox.Show("数据没有能够保存，请查找原因！", "提示");
                }
            }
            else
            {
                //  如果棋谱库编号为空，则另存为新棋谱。
                Save_Window window = new();
                _ = window.ShowDialog();
            }
            //  更新数据后，刷新棋谱列表
            GlobalValue.qiPuKuForm.QipuDBListRefresh();
        }

        private void OnSaveBtnClick(object sender, RoutedEventArgs e)
        {
            string ss = ((DataRowView)gupunameCombox.SelectedItem)["rowid"].ToString();
            Dictionary<string, object> dic = new();
            dic.Add("GuPuLeiBie", int.Parse(ss));
            dic.Add("GuPuName", ((DataRowView)gupunameCombox.SelectedItem)["Name"].ToString());
            dic.Add("Title", QiJuName.Text);
            dic.Add("Result", Result.Text);
            dic.Add("Memo", Remarks.Text);
            
            GlobalValue.qiPuSimpleRecordRoot = GlobalValue.ConvertQiPuToSimple(GlobalValue.qiPuRecordRoot);  // 更新简易棋谱记录
            dic.Add("Jsonrecord", JsonConvert.SerializeObject(GlobalValue.qiPuSimpleRecordRoot));
            _ = SqliteHelper.Insert("GuPuBook", dic);
        }
    }
}

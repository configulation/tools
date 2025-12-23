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
    public partial class QiPanPage : Page
    {
        private static Window_JiPu jipuWindow;  // 记谱窗口
        private static SpyWindow spyWindow;    // 棋盘数据监视窗口
        private static DataTable CanJuData;
        private static int CanJuIndex = 0;

        public QiPanPage()
        {
            InitializeComponent();
            
        }
        /// <summary>
        /// 页面载入时，初始化参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainFormLoaded(object sender, RoutedEventArgs e)
        {
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
            Infomation_board.Children.Add(GlobalValue.BestMoveInfo); // 下一步最佳着法提示信息
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
            Infomation_board.Children.Add(GlobalValue.jiangJunTiShi);// 将军状态文字提示
            GlobalValue.JueShaGrid = new();
            _ = JueshaGrid.Children.Add(GlobalValue.JueShaGrid); // 绝杀图片

            GlobalValue.arrows = new();
            _ = DrawGrid.Children.Add(GlobalValue.arrows.grid); // 走棋提示箭头

            jipuWindow = new Window_JiPu(); // 棋谱记录窗口
            jipuWindow.Hide();
            spyWindow = new SpyWindow(); // 棋盘数据监视窗口
            spyWindow.Hide();

            GlobalValue.qiPuKuForm = new Window_QiPu(); // 棋谱库浏览窗口
            GlobalValue.qiPuKuForm.Hide();

            #endregion
            GlobalValue.IsQiPanFanZhuan = false; // 棋盘翻转，初始为未翻转，黑方在上，红方在下
            QiPanChange(false);

            this.PCVsPc.Visibility = Visibility.Hidden;
            this.PersonVsPC.Visibility = Visibility.Hidden;
            this.FreeDaPu.Visibility = Visibility.Hidden;
            this.FuPan.Visibility = Visibility.Hidden;
            this.CanJuLianXi.Visibility = Visibility.Hidden;

            GlobalValue.Reset();
            GlobalValue.EnableGameStop = true;
            AutoMoveCanJuQiZi.IsEnabled = true;
            StopAutoMove.IsEnabled = false;
            PCVsPcAutoMoveCanJuQiZi.IsEnabled = true;
            PCVsPcStopAutoMove.IsEnabled = false;
            CanJuIndex = Settings.Default.CanJuIndex;
            GlobalValue.EnableGameStop = false;

            switch (MainWindow.menuItem) // 根据主菜单，打开对应的按钮面板
            {
                case 1: // 人机对战
                    this.PersonVsPC.Visibility = Visibility.Visible;
                    break;
                case 2: // 电脑对战
                    this.PCVsPc.Visibility = Visibility.Visible;
                    break;
                case 3: // 自由打谱
                    this.FreeDaPu.Visibility = Visibility.Visible;
                    break;
                case 4: // 复盘
                    this.FuPan.Visibility = Visibility.Visible;
                    GlobalValue.qiPuKuForm.Visibility = Visibility.Visible;
                    break;
                case 6:// 残局练习
                    this.CanJuLianXi.Visibility = Visibility.Visible;
                    CanJuData = SqliteHelper.Select("CanJuKu", "rowid,*");
                    for (int i = 0; i < 32; i++)
                    {
                        GlobalValue.qiZiArray[i].SetDied();
                    }
                    string fen = CanJuData.Rows[CanJuIndex]["FENstring"].ToString();
                    GlobalValue.QiPan = Engine.XQEngine.ConvertFenStrToQiPan(fen);
                    for (int i = 0; i <= 8; i++)
                    {
                        for (int j = 0; j <= 9; j++)
                        {
                            int qizi = GlobalValue.QiPan[i, j];
                            if (qizi > -1)
                            {
                                GlobalValue.qiZiArray[qizi].SetPosition(i, j);
                                GlobalValue.qiZiArray[qizi].Setlived();
                            }
                        }
                    }
                    CanJuComment.Text = $"第{CanJuIndex + 1}局/共{CanJuData.Rows.Count}局{System.Environment.NewLine}【{CanJuData.Rows[CanJuIndex]["Name"].ToString()}】：{System.Environment.NewLine}{CanJuData.Rows[CanJuIndex]["Comment"].ToString()}";
                    break;
                default:
                    break;
            }
            GlobalValue.BestMoveInfoText = Engine.XQEngine.UcciInfo.GetBestMove(false); // 调用象棋引擎，得到下一步推荐着法
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
        /// 打开或关闭记谱窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenJiPuWindow(object sender, RoutedEventArgs e)
        {
            if (jipuWindow.IsVisible)
            {
                jipuWindow.Close();
            }
            else
            {
                jipuWindow = new();
                jipuWindow.Show();
            }
        }

        /// <summary>
        /// 打开或关闭棋盘数据监控窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenSpyWindow(object sender, RoutedEventArgs e)
        {
            if (spyWindow.IsVisible)
            {
                spyWindow.Close();
            }
            else
            {
                spyWindow = new SpyWindow();
                spyWindow.Show();
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
            remarkGrid.Visibility = Visibility.Visible;
            string str = GlobalValue.qiPuRecordRoot.Cursor.Remarks;
            if (str == null || str.Length < 1)
            {
                str = GlobalValue.qiPuRecordRoot.Cursor.Cn;
            }
            remarkTextBox.Text = str;
        }
        /// <summary>
        /// 点击保存按钮后，保存到内存中，同时更新数据显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveRemark(object sender, RoutedEventArgs e)
        {
            GlobalValue.qiPuRecordRoot.Cursor.Remarks = remarkTextBox.Text;
            remarkGrid.Visibility = Visibility.Hidden;

            GlobalValue.qiPuSimpleRecordRoot = GlobalValue.ConvertQiPuToSimple(GlobalValue.qiPuRecordRoot);  // 更新简易棋谱记录
            Qipu.ContractQiPu.ConvertFromQiPuRecord(GlobalValue.qiPuRecordRoot); // 压缩树更新
            GlobalValue.qiPuKuForm.remarksTextBlock.Text = JsonConvert.SerializeObject(GlobalValue.qiPuSimpleRecordRoot);
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

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            GlobalValue.EnableGameStop = true;
            GlobalValue.IsGameOver = true;
            //Environment.Exit(0);
        }

        private void GameOverClick(object sender, RoutedEventArgs e)
        {
            GlobalValue.IsGameOver = true;
        }

        //  以下为残局练习的相关代码

        /// <summary>
        /// 重来，当前局重来
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReStartCanJu(object sender, RoutedEventArgs e)
        {
            string fen = CanJuData.Rows[CanJuIndex]["FENstring"].ToString();
            GlobalValue.QiPan = Engine.XQEngine.ConvertFenStrToQiPan(fen);
            for (int i = 0; i < 32; i++)
            {
                GlobalValue.qiZiArray[i].SetDied();
            }
            for (int i = 0; i <= 8; i++)
            {
                for (int j = 0; j <= 9; j++)
                {
                    int qizi = GlobalValue.QiPan[i, j];
                    if (qizi > -1)
                    {
                        GlobalValue.qiZiArray[qizi].SetPosition(i, j);
                        GlobalValue.qiZiArray[qizi].Setlived();
                    }
                    GlobalValue.pathPointImage[i, j].HasPoint = false; // 走棋路径点清空
                }
            }
            GlobalValue.EnableGameStop = true;
            GlobalValue.yuanWeiZhi.HiddenYuanWeiZhiImage();
            AutoMoveCanJuQiZi.IsEnabled = true;
            StopAutoMove.IsEnabled = false;
            PCVsPcAutoMoveCanJuQiZi.IsEnabled = true;
            PCVsPcStopAutoMove.IsEnabled = false;
            GlobalValue.IsGameOver = false;
            GlobalValue.SideTag = GlobalValue.REDSIDE;
            GlobalValue.BestMoveInfoText = Engine.XQEngine.UcciInfo.GetBestMove(false); // 调用象棋引擎，得到下一步推荐着法
            CanJuComment.Text = $"{CanJuIndex + 1}/{CanJuData.Rows.Count}  " + CanJuData.Rows[CanJuIndex]["Name"].ToString() + "：" + System.Environment.NewLine + CanJuData.Rows[CanJuIndex]["Comment"].ToString();
        }
        /// <summary>
        /// 前一个残局
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PerCanJu(object sender, RoutedEventArgs e)
        {
            CanJuIndex--;
            if (CanJuIndex < 0) CanJuIndex = 0;
            Settings.Default.CanJuIndex = CanJuIndex;
            ReStartCanJu(sender, e);
        }
        /// <summary>
        /// 下一个残局
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NextCanJu(object sender, RoutedEventArgs e)
        {
            CanJuIndex++;
            if (CanJuIndex >= CanJuData.Rows.Count) CanJuIndex = CanJuData.Rows.Count - 1;
            Settings.Default.CanJuIndex = CanJuIndex;
            ReStartCanJu(sender, e);
        }
        /// <summary>
        /// 电脑自动走残局
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AutoMoveCanJu(object sender, RoutedEventArgs e)
        {
            //ReStartCanJu(sender, e);
            GlobalValue.EnableGameStop = false;
            AutoMoveCanJuQiZi.IsEnabled = false;
            StopAutoMove.IsEnabled = true;
            PCVsPcAutoMoveCanJuQiZi.IsEnabled = false;
            PCVsPcStopAutoMove.IsEnabled = true;
            while (GlobalValue.EnableGameStop == false && GlobalValue.IsGameOver == false)
            {
                Qipu.StepCode step = Engine.XQEngine.UcciInfo.GetBestSetp();
                if (step != null) step.LunchStep(); else break;
                GlobalValue.Delay(Settings.Default.MoveDelayTime);
            }
            GlobalValue.EnableGameStop = true;
            AutoMoveCanJuQiZi.IsEnabled = true;
            StopAutoMove.IsEnabled = false;
            PCVsPcAutoMoveCanJuQiZi.IsEnabled = true;
            PCVsPcStopAutoMove.IsEnabled = false;

        }

        private void VideoUrl(object sender, RoutedEventArgs e)
        {
            Process proc = new();
            proc.StartInfo.UseShellExecute = true;
            // 在 .Net中，为了保证跨平台性，
            // 需要委托 Windows Shell 做一些事情时，
            // 需要显式声明 Process.StartUseShellExecute=true
            proc.StartInfo.FileName = "https://blog.csdn.net/weixin_33347911/article/details/114608150?spm=1035.2023.3001.6557&utm_medium=distribute.pc_relevant_bbs_down_v2.none-task-blog-2~default~OPENSEARCH~Rate-11.pc_relevant_bbs_down_v2_default&depth_1-utm_source=distribute.pc_relevant_bbs_down_v2.none-task-blog-2~default~OPENSEARCH~Rate-11.pc_relevant_bbs_down_v2_default";
            _ = proc.Start();
        }

        private void StopAutoMoveCanJu(object sender, RoutedEventArgs e)
        {
            GlobalValue.EnableGameStop = true;
            AutoMoveCanJuQiZi.IsEnabled = true;
            StopAutoMove.IsEnabled = false;
            PCVsPcAutoMoveCanJuQiZi.IsEnabled = true;
            PCVsPcStopAutoMove.IsEnabled = false;
        }

    }
}

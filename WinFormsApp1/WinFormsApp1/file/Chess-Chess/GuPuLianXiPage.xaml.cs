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
    public partial class GuPuLianXiPage : Page
    {

        public GuPuLianXiPage()
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
            openQiPuGrid.Visibility = Visibility.Visible;
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

        private void OnOkClick(object sender, RoutedEventArgs e)
        {
            
            if (qipuBook.SelectedIndex > -1)
            {
                openQiPuGrid.Visibility = Visibility.Hidden;
                string rowId = ((DataRowView)qipuBook.SelectedItem).Row["rowid"].ToString();
                DataTable sr = OpenSource.SqliteHelper.Select("GuPuBook","rowid,*", $"rowid={rowId}");
                DataRow row = sr.Rows[0];
                GupuName.Text =row["GuPuName"].ToString();
                QiJuName.Text = row["Title"].ToString();
                Result.Text = row["Result"].ToString();
                Remarks.Text = row["Memo"].ToString();
                string jsonStr = row["Jsonrecord"].ToString();
                int maxDepth = 1000;
                var simpleRecord = JsonConvert.DeserializeObject<Qipu.QiPuSimpleRecord>(jsonStr, new JsonSerializerSettings
                {
                    //  MaxDepth默认值为64，此处加大该值
                    TypeNameHandling = TypeNameHandling.None,
                    MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                    MaxDepth = maxDepth
                }); // 反序列化 

                GlobalValue.qiPuRecordRoot = GlobalValue.ConvertQiPuToFull(simpleRecord); // 转换为完全树数据结构
                Qipu.ContractQiPu.ConvertFromQiPuRecord(GlobalValue.qiPuRecordRoot); // 转换为收缩树数据结构
                GlobalValue.qiPuRecordRoot.Cursor = GlobalValue.qiPuRecordRoot; // 指向棋谱第一步，提示箭头自动显示
                QiPuDataGrid.ItemsSource = Qipu.ContractQiPu.ChildSteps;
            }
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            openQiPuGrid.Visibility = Visibility.Hidden;

        }

        private void OnQiPuKuOpen(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (openQiPuGrid.Visibility == Visibility.Visible)
            {
                DataTable sr = OpenSource.SqliteHelper.Select("GuPuBook", "rowid,*");
                if (sr == null) return;
                qipuBook.ItemsSource = sr.DefaultView;

                DataTable GuPuNameList = SqliteHelper.Select("GuPuList", "rowid,*");
                ShaiXuan.ItemsSource = GuPuNameList.DefaultView;
                
            }
        }

        private void QipuDBListRefresh(object sender, RoutedEventArgs e)
        {
            DataTable sr = OpenSource.SqliteHelper.Select("GuPuBook", "rowid,*");
            if (sr == null) return;
            qipuBook.ItemsSource = sr.DefaultView;
        }

        private void DeleteRowData(object sender, RoutedEventArgs e)
        {
            if (qipuBook.SelectedIndex > -1)
            {
                string rowId = ((DataRowView)qipuBook.SelectedItem).Row["rowid"].ToString();
                _ = OpenSource.SqliteHelper.Delete("GuPuBook", $"rowid={rowId}");
                QipuDBListRefresh(sender, e);
            }
        }

        private void OnDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            OnOkClick(sender, null);
        }

        private void OpenGuPuBook(object sender, RoutedEventArgs e)
        {
            openQiPuGrid.Visibility = Visibility.Visible;
        }
    }
}

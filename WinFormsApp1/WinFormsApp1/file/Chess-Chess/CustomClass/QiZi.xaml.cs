using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media.Effects;
using System.Windows.Media;
using System.Threading;
using System.Windows.Media.Animation;
using System.IO;

namespace Chess
{

    /// <summary>
    /// 棋子类
    /// 主程序中有32个棋子实例
    /// </summary>
    public partial class QiZi : UserControl
    {
        private readonly int init_col;  // 开局时棋子的列坐标
        private readonly int init_row;  // 开局时棋子的行坐标

        public int Col { get; set; }  // 棋子的列坐标
        public int Row { get; set; }  // 棋子的行坐标
        public int QiziId { get; set; }  // 棋子编号
        private bool _selected;
        /// <summary>
        /// 
        /// </summary>
        public bool Selected    // 棋子的选中状态
        { 
            get { return _selected; } 
            set { 
                if (value)
                {
                    Storyboard sb = (Storyboard)this.Resources["QiZiSeleted"];  // 阴影取消，动画
                    sb.Begin();
                    
                    yuxuankuang_image.Visibility = Visibility.Visible;
                    GlobalValue.CurrentQiZi = QiziId;
                    //Scall(1.01);
                    if (_selected==false && SideColor==GlobalValue.SideTag)SuanFa.MoveCheck.GetAndShowPathPoints(GlobalValue.CurrentQiZi); // 获取可移动路径，并显示在棋盘上
                    GlobalValue.yuanWeiZhi.SetPosition(Col, Row); // 棋子原位置标记，显示在当前位置
                    if (MainWindow.menuItem != GlobalValue.CANJU_DESIGN)
                    {
                        GlobalValue.yuanWeiZhi.ShowYuanWeiZhiImage();
                    }
                    else
                    {
                        GlobalValue.yuanWeiZhi.HiddenYuanWeiZhiImage();
                    }
                    if (Settings.Default.EnableSound)
                    {
                        GlobalValue.player.Open(new Uri("Sounds/select.mp3", UriKind.Relative));
                        GlobalValue.player.Play();
                    }
                }
                else
                {
                    yuxuankuang_image.Visibility = Visibility.Hidden; // 本棋子的预选框隐藏
                    if (_selected == true) // 棋子由选中状态变为非选中状态时
                    {
                        Storyboard sb = (Storyboard)this.Resources["YinYingCancel"];  // 阴影取消，动画
                        sb.Begin();
                        GlobalValue.CurrentQiZi = 100;
                    }
                }
                _selected = value;

            }
        }  
        public bool SideColor { get; set; }  // 棋子属于哪一方，false：黑棋，true：红棋

        /// <summary>
        /// 棋子类构造函数
        /// 空实例
        /// </summary>
        public QiZi()
        {
            InitializeComponent();
            yuxuankuang_image.Visibility=Visibility.Hidden;
            QiziId = -1;

        }
        /// <summary>
        /// 棋子类构造函数
        /// 根据棋子编号，载入对应的棋子图像，设定在棋盘的初始位置
        /// </summary>
        /// <param name="id">棋子编号</param>
        public QiZi(int id)
        {
            InitializeComponent();
            if (id is < 0 or > 31)
            {
                return;
            }
            QiziId = id;
            string path = Environment.CurrentDirectory + @"\picture\" + GlobalValue.qiZiImageFileName[QiziId] + ".png";
            //string path = @"pack://application:,,,/picture/" + GlobalValue.QiZiImageFileName[QiziId] + ".png";
            if (!File.Exists(path))
            {
                MessageBox.Show($"未找到文件：{path}");
                return;
            }
            BitmapImage bi = new(new Uri(path, UriKind.Absolute)); // 载入棋子图片
            bi.Freeze();
            QiZiImage.Source = bi;
            init_col = GlobalValue.qiZiInitPosition[id, 0]; // 开局时，棋子的位置
            init_row = GlobalValue.qiZiInitPosition[id, 1];
            if (MainWindow.menuItem == GlobalValue.CANJU_DESIGN)
            {
                init_col = GlobalValue.qiZiCanJuInitPosition[id, 0]; // 残局设计开局时，棋子的位置，在棋盘外。
                init_row = GlobalValue.qiZiCanJuInitPosition[id, 1];
            }
            SetPosition(init_col, init_row);
            SideColor = id >= 16;
            Selected = false;
        }

        /// <summary>
        /// 点击棋子时，其他棋子取消选中状态，本棋子设定选中状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            foreach (QiZi item in GlobalValue.qiZiArray)
            {
                item.Selected=false;
            }
            if (SideColor == GlobalValue.SideTag) // 只有走棋方的棋子，才可以选中
            {
                Selected = true;
            }
            else
            {
                Selected = true; // 点击非走棋方棋子时，阴影变化一下，表示选不中。
                Selected = false;
            }
            if (MainWindow.menuItem == GlobalValue.CANJU_DESIGN)
            {
                Selected = true;
            }
        }

        /// <summary>
        /// 取消选中状态
        /// </summary>
        public void Deselect()
        {
            Selected = false;
        }
        /// <summary>
        /// 选中时的处理
        /// </summary>
        public void Select()
        {
            foreach(QiZi item in GlobalValue.qiZiArray)
            {
                item.Deselect();
            }
            Selected = true;
        }

        /// <summary>
        /// 改变棋子的坐标位置
        /// 棋盘上设置了9列10行的坐标系，左上角第一个位置坐标为（0，0），右下角最后一个位置坐标为（8，9）
        /// </summary>
        /// <param name="x">列坐标</param>
        /// <param name="y">行坐标</param>
        public void SetPosition(int x, int y)
        {
            Col = x;
            Row = y;
            if (GlobalValue.IsQiPanFanZhuan) // 如果棋盘翻转为上红下黑，则进行坐标转换
            {
                x = 8 - x;
                y = 9 - y;
            }
            SetValue(Canvas.LeftProperty, GlobalValue.QiPanGrid_X[x] - 33);
            if (y >= 0 && y < 10)
            {
                SetValue(Canvas.TopProperty, GlobalValue.QiPanGrid_Y[y] - 33);
            }
            if (y == -1)
            {
                SetValue(Canvas.TopProperty, GlobalValue.QiPanGrid_Y_0);
            }
            if (y == 10)
            {
                SetValue(Canvas.TopProperty, GlobalValue.QiPanGrid_Y_10);
            }
            Selected = false;

            //QiZiImage.SetValue(EffectProperty, new DropShadowEffect() { ShadowDepth = 8, BlurRadius = 10, Opacity = 0.6 });
        }


        /// <summary>
        /// 设置棋子到开局时的初始位置
        /// </summary>
        public void SetInitPosition()
        {
            Visibility = Visibility.Visible;  // 棋子复活
            SetPosition(init_col, init_row);
            Selected=false;
        }
        /// <summary>
        /// 缩放
        /// </summary>
        /// <param name="scaller">缩放参数，1.0=原始尺寸</param>
        public void Scall(double scaller)
        {
            if (scaller is > 0 and < 10)
            {
                TransformGroup group = QiZiImage.FindResource("UserControlRenderTransform1") as TransformGroup;
                ScaleTransform scaler = group.Children[0] as ScaleTransform;
                scaler.ScaleX = scaller;
                scaler.ScaleY = scaller;
            }
        }
        public void FanZhuanPosition()
        {
            SetPosition(Col, Row);
        }
        /// <summary>
        /// 棋子被杀死
        /// </summary>
        public void SetDied()
        {
            Selected = false;
            Visibility = Visibility.Collapsed;
            //yuxuankuang.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// 棋子复活，用于复盘、悔棋等操作
        /// </summary>
        public void Setlived()
        {
            Visibility = Visibility.Visible;
        }

        public void ShowYuanWeiZhiImage()
        {
            yuanweizhi_image.Visibility = Visibility.Visible;
        }
        public void HiddenYuanWeiZhiImage()
        {
            yuanweizhi_image.Visibility = Visibility.Hidden;
        }
        /// <summary>
        /// 在残局设计界面，可移除棋子
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteQiZi(object sender, RoutedEventArgs e)
        {
            if (MainWindow.menuItem == GlobalValue.CANJU_DESIGN)
            {
                GlobalValue.QiPan[Col, Row] = -1;
                SetInitPosition();
            }
        }
    }
}


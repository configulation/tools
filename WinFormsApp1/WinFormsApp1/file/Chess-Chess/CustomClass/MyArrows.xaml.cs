using System;
using System.Collections.Generic;
using System.Drawing;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Chess.CustomClass
{
    /// <summary>
    /// MyArrows.xaml 的交互逻辑
    /// </summary>
    public partial class MyArrows : UserControl
    {
        private static readonly int arrowAngle = 160; // 箭头斜边相对箭杆的偏角
        private static readonly int arrowAngle1 = 170; // 箭头斜边相对箭杆的偏角
        private static readonly int arrowLong = 30; // 箭头斜边的长度
        private int _index;
        public int Index
        {
            get { return _index; }
            set
            {
                _index = value;
                arrowNumber.Text = (value + 1).ToString();
                if (value == 0)
                {
                    arrowPath.Opacity -= Index * 0.1;
                    arrowEllipse.Opacity -= Index * 0.1;

                    arrowNumber.FontWeight = FontWeights.Bold;
                    arrowNumber.Foreground = Brushes.OrangeRed;
                    arrowPrompt.FontWeight = FontWeights.Bold;
                }
            }
        }
        public MyArrows(int arrowNumber)
        {
            InitializeComponent();
            Index = arrowNumber;

        }

        private void IsArrowVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue.Equals(true))
            {
                if (arrowPrompt.Text.Length > 0 && Settings.Default.ArrowsMemo)
                {
                    arrowPromptBorder.Visibility = Visibility.Visible;
                }
                else
                {
                    arrowPromptBorder.Visibility = Visibility.Hidden;
                }
            }
            else
            {
                arrowPrompt.Text = "";
            }
        }
        /// <summary>
        /// 根据箭头起始点，计算绘制箭头的各项数据，并显示到界面
        /// </summary>
        /// <param name="arrowId">箭头的编号，0-4 </param>
        /// <param name="point0">起始点</param>
        /// <param name="point1">终点</param>
        /// <param name="sameTargetPoint">箭头指向同一位置时，第二个参数为true，避免编号位置重叠</param>
        public void SetPathData(System.Drawing.Point point0, System.Drawing.Point point1, bool sameTargetPoint, string memo)
        {

            int haveQizi = GlobalValue.QiPan[point1.X, point1.Y]; // 目标位置的棋子编号，-1表示没有棋子。
            if (GlobalValue.IsQiPanFanZhuan)
            {
                point0.X = 8 - point0.X;  // 棋盘处于翻转状态时，转换坐标
                point0.Y = 9 - point0.Y;
                point1.X = 8 - point1.X;
                point1.Y = 9 - point1.Y;
            }

            double x0, y0, x1, y1;
            x0 = GlobalValue.QiPanGrid_X[point0.X];
            y0 = GlobalValue.QiPanGrid_Y[point0.Y];
            x1 = GlobalValue.QiPanGrid_X[point1.X];
            y1 = GlobalValue.QiPanGrid_Y[point1.Y];

            #region  计算提示箭头及数字编号标识
            List<PointF> pointFs = new();

            double angle = Math.Atan2(y1 - y0, x1 - x0); //箭杆的角度
            double angle1;
            double xm, ym, xn, yn;

            angle1 = angle;
            int leng0 = 10; // 箭头初端离开起始点10
            x0 = (float)Math.Floor(x0 + (leng0 * Math.Cos(angle1)));
            y0 = (float)Math.Floor(y0 + (leng0 * Math.Sin(angle1)));
            pointFs.Add(new PointF((float)x0, (float)y0)); // 存入第一个点

            angle1 = angle + Radians(180); // 箭头末端(xm,ym)离开终点10
            if (Math.Abs(point0.X - point1.X) + Math.Abs(point0.Y - point1.Y) > 1) leng0 = 1;
            xm = (float)Math.Floor(x1 + (leng0 * Math.Cos(angle1)));
            ym = (float)Math.Floor(y1 + (leng0 * Math.Sin(angle1)));

            // 以下均以箭头末端为基点，计算其他各点位置
            angle1 = angle + Radians(arrowAngle1); // 斜边相对坐标轴的角度 = 箭杆的角度 - 箭头斜边相对箭杆的偏角
            xn = (float)Math.Floor(xm + (arrowLong * 2 / 3 * Math.Cos(angle1)));
            yn = (float)Math.Floor(ym + (arrowLong * 2 / 3 * Math.Sin(angle1)));
            pointFs.Add(new PointF((float)xn, (float)yn));  // 存入第二个点

            angle1 = angle + Radians(arrowAngle); // 斜边相对坐标轴的角度 = 箭杆的角度 - 箭头斜边相对箭杆的偏角
            xn = (float)Math.Floor(xm + (arrowLong * Math.Cos(angle1)));
            yn = (float)Math.Floor(ym + (arrowLong * Math.Sin(angle1)));
            pointFs.Add(new PointF((float)xn, (float)yn));  // 存入第三个点

            pointFs.Add(new PointF((float)xm, (float)ym)); // 箭头末端，是第四个点

            angle1 = angle - Radians(arrowAngle); // 斜边相对坐标轴的角度 = 箭杆的角度 - 箭头斜边相对箭杆的偏角
            xn = (float)Math.Floor(xm + (arrowLong * Math.Cos(angle1)));
            yn = (float)Math.Floor(ym + (arrowLong * Math.Sin(angle1)));
            pointFs.Add(new PointF((float)xn, (float)yn));  // 存入第五个点

            angle1 = angle - Radians(arrowAngle1); // 斜边相对坐标轴的角度 = 箭杆的角度 - 箭头斜边相对箭杆的偏角
            xn = (float)Math.Floor(xm + (arrowLong * 2 / 3 * Math.Cos(angle1)));
            yn = (float)Math.Floor(ym + (arrowLong * 2 / 3 * Math.Sin(angle1)));
            pointFs.Add(new PointF((float)xn, (float)yn));  // 存入第六个点

            arrowPath.Data = Geometry.Parse(MakePathData(pointFs));
            //ArrowPath[arrowId].Visibility = Visibility.Visible;

            double circleX, circleY;
            double cirlcePos = 1.0;
            if (Math.Abs(point0.X - point1.X) + Math.Abs(point0.Y - point1.Y) > 1) // 长箭头的数字标识放在箭杆上
            {
                cirlcePos = arrowLong * -1.25;
            }
            //计算圆圈的位置，其中心设置在箭杆的中心线上
            circleX = Math.Floor(x1 + (cirlcePos * Math.Cos(angle))) - 10; // 10是圆圈的半径。计算结果为圆心位置，而margin是从其边界计算，因此需用半径修正数据。
            circleY = Math.Floor(y1 + (cirlcePos * Math.Sin(angle))) - 10;

            arrowEllipsGrid.Margin = new Thickness(circleX, circleY, 0, 0);

            if (memo == null || string.IsNullOrEmpty(memo)) return;

            arrowPrompt.Text = $"{Index + 1}：{memo}";
            arrowPromptBorder.Margin = new Thickness(circleX, circleY + 22, 80, 0);
            #endregion
        }
        /// <summary>
        /// 角度转换为弧度
        /// </summary>
        /// <param name="degress">角度</param>
        /// <returns>弧度值</returns>
        private static double Radians(double degress)
        {
            return degress * Math.PI / 180;
        }
        /// <summary>
        /// 根据路径点，转换为路径绘图指令
        /// </summary>
        /// <param name="pointFList">路径点列表</param>
        /// <returns>路径绘图指令字符串</returns>
        private static string MakePathData(List<PointF> pointFList)
        {
            string path = $"M {pointFList[0].X}, {pointFList[0].Y} ";
            foreach (PointF point in pointFList)
            {
                path += $"L {point.X} , {point.Y} ";
            }
            path += " Z";  // Z=封闭路径
            return path;
        }
    }
}

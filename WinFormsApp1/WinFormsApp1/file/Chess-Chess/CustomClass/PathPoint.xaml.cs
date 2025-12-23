using Chess.CustomClass;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;

namespace Chess
{
    /// <summary>
    /// 路径标记点
    /// </summary>
    public partial class PathPoint : UserControl
    {
        private bool _haspoint = false;
        public bool HasPoint
        {
            get { return _haspoint; }
            set
            {
                _haspoint = value;
                if (value) Visibility = Visibility.Visible;
                else Visibility = Visibility.Hidden;
                if (value && Settings.Default.EnablePathPoint) { image.Visibility= Visibility.Visible; } // 依据用户设置，是否显示路径点
                else { image.Visibility= Visibility.Hidden; }
                if (MainWindow.menuItem==GlobalValue.CANJU_DESIGN) { image.Visibility= Visibility.Hidden; }
            }
        }  // 是否是有效的走棋路径点
        public int Col { get; set; }    // 路径点的列坐标
        public int Row { get; set; }    // 路径点的行坐标

        /// <summary>
        /// 棋子移动目的地标记类，
        /// 在棋子可移动到的有效位置，设置标记。
        /// 点击此标记时，当前棋子移动到标记位置。
        /// </summary>
        /// <param name="x">列位置</param>
        /// <param name="y">行位置</param>
        public PathPoint(int x, int y)
        {
            InitializeComponent();
            if (x is < 0 or > 8)
            {
                return;
            }
            if (y is < 0 or > 9)
            {
                return;
            }
            HasPoint = false;
            Setposition(x, y);
            if (MainWindow.menuItem == GlobalValue.CANJU_DESIGN) image.Visibility = Visibility.Hidden;

        }

        /// <summary>
        /// 设置本棋子的坐标位置
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void Setposition(int x, int y)
        {
            Col = x;
            Row = y;
            if (GlobalValue.IsQiPanFanZhuan)
            {
                x = 8 - x;
                y = 9 - y;
            }
            SetValue(Canvas.LeftProperty, GlobalValue.QiPanGrid_X[x] - 30);
            SetValue(Canvas.TopProperty, GlobalValue.QiPanGrid_Y[y] - 30);
        }
        public void FanZhuPosition()
        {
            Setposition(Col, Row);
        }

        /// <summary>
        /// 鼠标点击时，棋子移动处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseup(object sender, MouseButtonEventArgs e)
        {
            if (MainWindow.menuItem == GlobalValue.CANJU_DESIGN)
            {
                // 自由摆放棋子
                GlobalValue.QiZiFreeMoveTo(GlobalValue.CurrentQiZi, Col, Row, true);
            }
            else
            {
                // 按走棋规则走棋
                if (GlobalValue.IsGameOver == false)
                    if (GlobalValue.QiZiMoveTo(GlobalValue.CurrentQiZi, Col, Row, true))
                    {
                        if (MainWindow.menuItem == GlobalValue.PERSON_PC && GlobalValue.SideTag == GlobalValue.BLACKSIDE)
                        {
                            GlobalValue.Delay(Settings.Default.MoveDelayTime);
                            Qipu.StepCode step = Engine.XQEngine.UcciInfo.GetBestSetp();
                            if (step != null) step.LunchStep();
                        }
                        // 电脑对战，第一步需人为走出。此处暂保留，后期可能删除
                        if (MainWindow.menuItem == 100)
                        {
                            while (GlobalValue.EnableGameStop == false && GlobalValue.IsGameOver == false)
                            {
                                GlobalValue.Delay(Settings.Default.MoveDelayTime);
                                Qipu.StepCode step = Engine.XQEngine.UcciInfo.GetBestSetp();
                                if (step != null) step.LunchStep(); else break;
                            }
                        }
                        // 残局破解
                        if (MainWindow.menuItem == GlobalValue.CANJU_POJIE)
                        {
                            GlobalValue.Delay(Settings.Default.MoveDelayTime);
                            Qipu.StepCode step = Engine.XQEngine.UcciInfo.GetBestSetp();
                            if (step != null) step.LunchStep();
                        }
                    }
            }
        }
    }
}

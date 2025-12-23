using System;
using System.Windows;
using System.Windows.Controls;

namespace Chess.Test
{
    /// <summary>
    /// Huo.xaml 的交互逻辑
    /// </summary>
    public partial class Huo : Page
    {
        private int index = 0;
        private int rate = 0;
        private Rect rect = new Rect
        {
            Width = 210,
            Height = 210,
            X = 0,
            Y = 0
        };
        private int[,] pos = new int[7, 2] {
            { -2, 206 },
            { 191, 201 },
            { 408, -6 },
            { 385, 192 },
            { 209, -5 },
            { 0, 0 },
            { 608, -5 } };
        public Huo()
        {
            System.Windows.Media.CompositionTarget.Rendering += DongHua; // 按每秒60帧速率调用
        }
        protected void DongHua(object Sender, EventArgs e)
        {
            rate++;
            if (rate == 20) // 降低帧率
            {
                rate = 0;
                index++;
                index %= 7;
                rect.X = pos[index, 0];
                rect.Y = pos[index, 1];
                HuoBrush.Viewbox = rect;
            }
        }
    }
}


using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Media;
using Chess;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Data;
using System.Runtime.CompilerServices;

namespace Chess.CustomClass
{
    /// <summary>
    /// 走棋提示箭头
    /// </summary>
    public class MyGraphics
    {
        private readonly static int _maxNum = 9; //提示箭头数量上限为9个，对应变招数量，所以多了也没用。
        public Grid grid = new(); // 绘图板，承载所有绘图元素
        private readonly MyArrows[] ArrowPath = new MyArrows[_maxNum];  // 箭头本体。
        
        private static int arrowCount = 0;// 当前有效的箭头数量
        /// <summary>
        /// 初始化走棋提示箭头
        /// </summary>
        public MyGraphics()
        {
            grid.Opacity = 1;
            grid.HorizontalAlignment = HorizontalAlignment.Stretch;
            grid.VerticalAlignment = VerticalAlignment.Stretch;
            for (int i=ArrowPath.Length-1;i>=0; i--)
            {
                ArrowPath[i] = new MyArrows(i); // 箭头本体
                _ = grid.Children.Add(ArrowPath[i]);
            }
        }
        /// <summary>
        /// 隐藏所有箭头
        /// </summary>
        public void HideAllPath()
        {
            foreach (MyArrows item in ArrowPath)
            {
                item.Visibility = Visibility.Hidden;
            }
        }

        public void ShowAllPath()
        {
            if (Settings.Default.ArrowVisable)
            {
                for (int i = 0; i < arrowCount; i++)
                {
                    ArrowPath[i].Visibility = Visibility.Visible;
                }
            }
        }
        /// <summary>
        /// 根据箭头起始点，计算绘制箭头的各项数据，并显示到界面
        /// </summary>
        /// <param name="arrowId">箭头的编号，0-4 </param>
        /// <param name="point0">起始点</param>
        /// <param name="point1">终点</param>
        /// <param name="sameTargetPoint">箭头指向同一位置时，第二个参数为true，避免编号位置重叠</param>
        public void SetPathData(int arrowId, System.Drawing.Point point0, System.Drawing.Point point1, bool sameTargetPoint, string memo)
        {
            arrowCount = arrowId + 1;// 有效箭头数量取最后一次提交的编号
            if (arrowId > _maxNum - 1) return; // 箭头从0开始编号，数量不能超过上限（_maxNum）
            ArrowPath[arrowId].SetPathData(point0, point1, sameTargetPoint, memo);
            
        }
    }
}

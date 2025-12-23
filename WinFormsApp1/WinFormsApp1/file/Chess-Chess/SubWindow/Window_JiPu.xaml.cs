using Chess;
using System;
using System.Collections.Generic;
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
using System.Windows.Threading;
using System.Collections.ObjectModel;
using Chess.CustomClass;

namespace Chess
{
    /// <summary>
    /// Window_JiPu.xaml 的交互逻辑
    /// </summary>
    public partial class Window_JiPu : Window
    {
        public Window_JiPu()
        {
            InitializeComponent();
            Left = SystemParameters.WorkArea.Right - this.Width;
            Top = SystemParameters.WorkArea.Top;
            Height = SystemParameters.WorkArea.Height;

        }

        private void FormLoad(object sender, RoutedEventArgs e)
        {
            JiPuDataGrid.ItemsSource = Qipu.QiPuList;

        }
        /// <summary>
        /// 递归查找变招位置，并将变招存入相应分支
        /// </summary>
        /// <param name="oldQiPu">老谱</param>
        /// <param name="newQiPu">新谱</param>
        private void ReBuildQipuList(ObservableCollection<ObservableCollection<Qipu.ContractQPClass>> oldQiPu, ObservableCollection<Qipu.ContractQPClass> newQiPu)
        {
            bool findExist = false;
            foreach (ObservableCollection<Qipu.ContractQPClass> oldqp in oldQiPu)
            {
                if (string.Equals(newQiPu[0].Cn, oldqp[0].Cn, StringComparison.Ordinal))
                {
                    findExist = true; // 查找是否有第一步相同的棋谱
                }
            }
            if (findExist == false)
            {
                oldQiPu.Add(newQiPu);
                return;
            }
            else // 存在第一步相同的棋谱
            {
                for (int listIndex = 0; listIndex < oldQiPu.Count; listIndex++)
                {
                    if (string.Equals(newQiPu[0].Cn, oldQiPu[listIndex][0].Cn, StringComparison.Ordinal)) // 定位到第一步相同的棋谱
                    {
                        for (int i = 1; i < oldQiPu[listIndex].Count; i++) // 逐项对比
                        {
                            if (i > newQiPu.Count - 1)
                            {
                                return; // 如果的步数少于老谱，且新棋谱与老谱完全重合，则不作处理，直接退出
                            }
                            if (i == oldQiPu[listIndex].Count - 1 && oldQiPu[listIndex].Count < newQiPu.Count) // 老谱已到末尾，且新谱还没结束时
                            {
                                for (int j = oldQiPu[listIndex].Count; j < newQiPu.Count; j++)
                                {
                                    oldQiPu[listIndex].Add(newQiPu[j]); // 将新谱剩余的步数追加到老谱上
                                }
                                return;
                            }
                            if (!string.Equals(newQiPu[i].Cn, oldQiPu[listIndex][i].Cn, StringComparison.Ordinal))
                            {
                                // 找到变招位置后
                                ObservableCollection<Qipu.ContractQPClass> subNew = new();
                                for (int j = i; j < newQiPu.Count; j++)
                                {
                                    subNew.Add(newQiPu[j]); // 删除相同的招数
                                }
                                ReBuildQipuList(oldQiPu[listIndex][i - 1].ChildSteps, subNew); // 将变化招数存入子分支
                                break;
                            }
                        }
                        break;
                    }
                }
            }
        }
    }
}

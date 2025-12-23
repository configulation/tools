using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections;
using Newtonsoft.Json;
using System.Windows.Controls;
using System.Drawing;
using System.ComponentModel;

namespace Chess.CustomClass
{
    public static class Qipu  // 棋谱类
    {

        public static ObservableCollection<ContractQPClass> QiPuList = new(); // 棋谱记录列表
        public static ContractQPClass ContractQiPu = new(); // 收缩树
        /// <summary>
        /// 着法类
        /// </summary>
        public class StepCode
        {
            #region 棋谱数据
            public int QiZi { get; set; } // 棋子编号
            public int X0 { get; set; } // 移动前位置
            public int Y0 { get; set; }
            public int X1 { get; set; } // 移动后位置
            public int Y1 { get; set; }
            public int DieQz { get; set; } // 移动后杀死的棋子
            public string UcciStep;
            public StepCode(int qiZi, int x0, int y0, int x1, int y1, int dieQz)
            {
                QiZi = qiZi;
                X0 = x0;
                Y0 = y0;
                X1 = x1;
                Y1 = y1;
                DieQz = dieQz;
                string cols = "abcdefghi";
                string rows = "9876543210";
                UcciStep = string.Concat(cols.AsSpan(x0, 1), rows.AsSpan(y0, 1), cols.AsSpan(x1, 1), rows.AsSpan(y1, 1));
            }
            /// <summary>
            /// 执行着法
            /// </summary>
            public void LunchStep()
            {
                if (QiZi is > -1 and < 32)
                {
                    GlobalValue.QiZiMoveTo(QiZi, X1, Y1, true);
                }
            }
            #endregion
        }
        /// <summary>
        /// 棋谱基类
        /// 包含棋谱步数、走棋数字代码（用于数据库存储）、走棋中文代码（用于列表或树显示）等
        /// </summary>
        public class QPBase
        {
            #region 棋谱数字代码、中文代码、备注
            public int Id { get; set; } // 步数
            public string Nm { get; set; } // 数字代码
            public string Cn { get; set; } // 中文代码
            public string Remarks { get; set; } // 备注
            public StepCode StepData { get; set; } // 棋谱记录
            #endregion
        }
        /// <summary>
        /// 收缩树存储类，用于更直观的查看棋谱内容。
        /// 每个节点有多个兄弟节点，也可以有多个子节点，但只有一个父节点。
        /// 收缩树系从完整树转换而来。
        /// 转换时，通过递归，将完整树的每个节点的第一子节点，变为兄弟节点。
        /// </summary>
        public class ContractQPClass : QPBase
        {
            public ObservableCollection<ObservableCollection<ContractQPClass>> ChildSteps { get; set; }   // 棋谱变化
            public ContractQPClass()
            {
                ChildSteps = new ObservableCollection<ObservableCollection<ContractQPClass>>();
            }
            /// <summary>
            /// 棋谱完整树，转换为收缩树
            /// </summary>
            /// <param name="qiPuRecord">棋谱完整树</param>
            public void ConvertFromQiPuRecord(QiPuRecord qiPuRecord)
            {
                ChildSteps.Clear();
                var obobQiPu = ConvertData(qiPuRecord);
                foreach (var step in obobQiPu)
                {
                    ChildSteps.Add(step);
                }
                //ChildSteps.Add(obobstep);
            }
            /// <summary>
            /// 递归转换完整树
            /// </summary>
            /// <param name="qiPuRecord"></param>
            /// <returns></returns>
            private static ObservableCollection<ObservableCollection<ContractQPClass>> ConvertData(QiPuRecord qiPuRecord)
            {
                ObservableCollection<ObservableCollection<ContractQPClass>> qpRoot = new();
                QiPuRecord currStep = qiPuRecord;
                while (!currStep.IsLeaf())
                {
                    ContractQPClass qp0 = new();
                    qp0.CopyDataFromQiPuRecord(currStep);
                    ObservableCollection<ContractQPClass> qpList = new();
                    qpList.Add(qp0);
                    qpRoot.Add(qpList);

                    for (int i = 1; i < currStep.ChildNode.Count; i++)
                    {
                        var steps = ConvertData(currStep.ChildNode[i]);
                        foreach (var step in steps)
                        {
                            qp0.ChildSteps.Add(step);
                        }
                    }
                    currStep = currStep.ChildNode[0];
                }
                if (currStep.IsLeaf())
                {
                    ContractQPClass qp0 = new();
                    qp0.CopyDataFromQiPuRecord(currStep);
                    ObservableCollection<ContractQPClass> qplist = new();
                    qplist.Add(qp0);
                    qpRoot.Add(qplist);
                }

                return qpRoot;

            }
            /// <summary>
            /// 复制走棋指令数据
            /// </summary>
            /// <param name="qrecord"></param>
            private void CopyDataFromQiPuRecord(QiPuRecord qrecord)
            {
                Id = qrecord.Id;
                Nm = qrecord.Nm;
                Cn = qrecord.Cn;
                Remarks = qrecord.Remarks;
            }
        }
        /// <summary>
        /// 棋谱完整树数据存储结构
        /// 每个结点可以有多个子结点，但没有兄弟节点，且只有一个父结点。
        /// 用于保存每一步走棋信息。
        /// </summary>
        public class QiPuRecord : QPBase, INotifyPropertyChanged
        {
            private QiPuRecord ParentNode { get; set; } // 父结点
            public ObservableCollection<QiPuRecord> ChildNode { get; set; }  // 子结点
            private QiPuRecord _cursor;
            public QiPuRecord Cursor
            {
                get { return _cursor; }
                set
                {
                    _cursor = value;
                    _cursor.IsSelected = true;
                    if (GlobalValue.arrows != null) GlobalValue.arrows.HideAllPath();
                    if (MainWindow.menuItem == GlobalValue.QIPU_RECORD && !_cursor.IsLeaf())
                    {
                        var points = GetListPoint(_cursor);
                        for (int i = 0; i < points.Count; i++)
                        {
                            bool sameTargetPoint = false;
                            if (i > 0)
                            {
                                if (points[i][1].X == points[i - 1][1].X && points[i][1].Y == points[i - 1][1].Y)
                                {
                                    sameTargetPoint = true;
                                }
                            }
                            GlobalValue.arrows.SetPathData(i, points[i][0], points[i][1], sameTargetPoint, _cursor.ChildNode[i].Remarks);
                        }
                        GlobalValue.arrows.ShowAllPath();
                    }
                    if (_cursor.Remarks != null)
                        GlobalValue.jiangJunTiShiText = _cursor.Remarks;
                }
            }  // 当前结点游标指针，仅根结点游标有用
            public string SideColor { get; set; }  // RED=红方，BLACK=黑方
            private bool _isSelected;
            public bool IsSelected
            {
                get { return _isSelected; }
                set
                {
                    _isSelected = value;
                    INotifyPropertyChanged("IsSelected");
                }
            }    // 是否选中

            private void INotifyPropertyChanged(string v)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
            }

            public QiPuRecord()
            {
                ParentNode = null;
                ChildNode = new ObservableCollection<QiPuRecord>();
                Cursor = this;
                SideColor = "Red";
                IsSelected = false;
            }
            public QiPuRecord(QiPuRecord recordNode)
            {
                ParentNode = recordNode;
                recordNode.ChildNode.Add(this);
                ChildNode = new ObservableCollection<QiPuRecord>();
                Cursor = null;
            }

            public event PropertyChangedEventHandler PropertyChanged;
            /// <summary>
            /// 棋谱增加子节点
            /// </summary>
            /// <param name="child"></param>
            /// <returns>返回增加的节点</returns>
            public QiPuRecord AddChild(QiPuRecord child)
            {
                foreach (var item in ChildNode)
                {
                    if (string.Equals(item.Cn, child.Cn, StringComparison.OrdinalIgnoreCase))
                    {
                        return item;  // 如果已存在，则不增加新步骤
                    }
                }
                //child.CurrentRecord = null;
                child.ParentNode = this;
                child.Id = Id + 1;
                ChildNode.Add(child);
                return child;
            }
            public void DeleteChildNode()
            {
                ChildNode.Clear();
                //ChildNode = new ObservableCollection<QiPuRecord>();
            }
            /// <summary>
            /// 是否为根节点
            /// </summary>
            /// <returns>true=是根节点</returns>
            public bool IsRoot()
            {
                return ParentNode == null;
            }
            /// <summary>
            /// 是否为叶子节点
            /// </summary>
            /// <returns>true=是叶子节点</returns>
            public bool IsLeaf()
            {
                return ChildNode.Count == 0;
            }
            public void SetParent(QiPuRecord parent)
            {
                ParentNode = parent;
            }
            /// <summary>
            /// 获取父节点
            /// </summary>
            /// <returns>父节点</returns>
            public QiPuRecord GetParent()
            {
                return ParentNode;
            }
            /// <summary>
            /// 设置棋谱记录数据
            /// </summary>
            /// <param name="code">已有的记录</param>
            public void CopyRecordData(StepCode code)
            {
                if (code == null) return;
                SetRecordData(code.QiZi, code.X0, code.Y0, code.X1, code.Y1, code.DieQz);
            }
            /// <summary>
            /// 设置棋谱记录数据
            /// </summary>
            /// <param name="QiZi"></param>
            /// <param name="x0"></param>
            /// <param name="y0"></param>
            /// <param name="x1"></param>
            /// <param name="y1"></param>
            /// <param name="DieQz"></param>
            public void SetRecordData(int QiZi, int x0, int y0, int x1, int y1, int DieQz)
            {
                Nm = $"{QiZi:d2} {x0:d} {y0:d} {x1:d} {y1:d} {DieQz:d}";
                Cn = GlobalValue.TranslateToCN(QiZi, x0, y0, x1, y1);
                //Remarks = "";
                StepData = new StepCode(QiZi, x0, y0, x1, y1, DieQz);
                SideColor = QiZi is >= 0 and <= 15 ? "Black" : "Red";  //  在treeView中，显示不同的颜色。
                //QiPuList.Add(this);
            }
            
            /// <summary>
            /// 获取6个历史着法，主要用于象棋引擎的重复走棋判断
            /// </summary>
            /// <returns></returns>
            public string Get6Moves()
            {
                string str = "";
                QiPuRecord record = this.Cursor;
                if (record.IsRoot()) return str;
                for (int i = 0; i < 20; i++)
                {
                    if (!record.IsRoot())
                    {
                        str += " " + record.StepData.UcciStep;
                        record = record.GetParent();
                    }
                }
                return " moves " + str;
            }
            /// <summary>
            /// 连续杀4次，则判负。
            /// </summary>
            /// <returns></returns>
            public bool IsLianSha()
            {
                QiPuRecord record = this.Cursor;
                if (record.IsRoot()) return false;
                string[] stepString = new string[20];
                for (int i = 0; i < 20; i++)
                {
                    stepString[i] = "";
                    if (!record.IsRoot())
                    {
                        stepString[i] = record.StepData.UcciStep;
                        record = record.GetParent();
                    }
                }
                string step1 = stepString[0] + stepString[1] + stepString[2] + stepString[3];
                string step2 = stepString[4] + stepString[5] + stepString[6] + stepString[7];
                string step3 = stepString[8] + stepString[9] + stepString[10] + stepString[11];

                if (step1.Equals(step2) && step1.Equals(step3))
                {
                    return true;
                }
                return false;
            }
        }
        /// <summary>
        /// 棋谱简易记录类
        /// 用于数据库存储
        /// </summary>
        public class QiPuSimpleRecord
        {
            public int Id { get; set; } // 步数
            public List<QiPuSimpleRecord> Child { get; set; }  // 子结点
            public StepCode Data { get; set; } // 棋谱记录
            public string Remarks { get; set; } // 备注

            public QiPuSimpleRecord()
            {
                Child = new List<QiPuSimpleRecord>();
            }
            public void CopyDataFromStep(StepCode code)
            {
                if (code == null) return;
                Data = new StepCode(code.QiZi, code.X0, code.Y0, code.X1, code.Y1, code.DieQz);
            }
        }


        /// <summary>
        /// 将棋谱中文代码转化为长字符串
        /// </summary>
        /// <returns></returns>
        public static string CnToString()
        {
            int maxLen = 20;
            string recode = "";
            foreach (ContractQPClass p in QiPuList)
            {
                recode += p.Cn + " ";
            }
            string substr = recode;
            if (recode.Length > maxLen)
            {
                substr = string.Concat(recode.AsSpan(0, maxLen), " ...");
            }
            return $"{substr} (共{QiPuList.Count}步)";
        }
        /// <summary>
        /// 根据子节点数量，编制每个走棋提示箭头的坐标数据
        /// </summary>
        /// <param name="qPStep">棋谱节点</param>
        /// <returns>坐标数据列表</returns>
        public static List<List<Point>> GetListPoint(QiPuRecord qPStep)
        {
            List<List<Point>> pp = new();
            foreach (var lp in qPStep.ChildNode)
            {
                List<Point> pt = new()
                {
                    new Point(lp.StepData.X0, lp.StepData.Y0),
                    new Point(lp.StepData.X1, lp.StepData.Y1)
                };
                pp.Add(pt);
            }
            return pp;
        }


    }
}

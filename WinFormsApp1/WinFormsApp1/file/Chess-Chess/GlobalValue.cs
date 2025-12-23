using Chess.SuanFa;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using static Chess.CustomClass.Qipu;
using Chess.CustomClass;
using System.Windows.Media;
using System.Windows.Threading;
using System.ComponentModel;

namespace Chess
{
    internal class GlobalValue
    {
        public const float GRID_WIDTH = 67.5f;   //棋盘格大小为 67.5*67.5
        public const bool BLACKSIDE = false;  // 黑方
        public const bool REDSIDE = true;   //红方

        public static bool _sideTag;
        public static bool SideTag   // 当前走棋方
        {
            get { return _sideTag; }
            set
            {
                _sideTag = value;
                Settings.Default.CurrentSide = value;
            }
        }
        private static bool _isGameOver;
        public static bool IsGameOver  // 游戏结束标志
        {
            get { return _isGameOver; }
            set
            {
                _isGameOver = value;
                if (value)
                {
                    Juesha = new();
                    JueShaGrid.Children.Add(Juesha);
                } // 已绝杀时，显示绝杀图像
            }
        }  // 游戏结束，系统自动检测
        public static bool EnableGameStop; // 人为停止游戏，用于电脑自动走棋过程中，中止走棋

        public static bool IsQiPanFanZhuan; // 棋盘上下翻转，默认值为false，下红上黑，设为true后，翻转后为下黑上红
        public static int CurrentQiZi;  // 当前选定的棋子
        public static int[,] QiPan = new int[9, 10]; // 棋盘数据，9列10行，记录棋子位置，如果为-1，则表示该位置没有棋子。

        public static MediaPlayer player = new();
        public const int PERSON_PC = 1;
        public const int PC_PC = 2;
        public const int FREE_DAPU = 3;
        public const int QIPU_RECORD = 4;
        public const int CANJU_DESIGN = 5;
        public const int CANJU_POJIE = 6;


        #region // 用户界面元素
        public static PathPoint[,] pathPointImage = new PathPoint[9, 10];  // 棋子可走路径的圆点标记
        public static QiZi[] qiZiArray = new QiZi[32]; // 棋子数组，所有棋子均在此数组中
        public static QiZi yuanWeiZhi;  // 棋子走动后在原位置显示圆圈

        public static string jiangJunTiShiText
        {
            get { return ""; }
            set
            {
                if (jiangJunTiShi != null)
                {
                    jiangJunTiShi.Text = value;
                }
            }
        }
        public static TextBlock jiangJunTiShi; // 将军时的文字提示
        public static string BestMoveInfoText
        {
            get { return ""; }
            set
            {
                if (BestMoveInfo != null)
                {
                    BestMoveInfo.Text = value;
                }
            }
        }
        public static TextBlock BestMoveInfo; // 将军时的文字提示
        public static Grid JueShaGrid;  // 绝杀图片的容器
        public static JueSha Juesha; // 绝杀时显示图片
        public static Window_QiPu qiPuKuForm; // 棋谱库窗口
        public static MyGraphics arrows; // 走棋指示箭头
        #endregion

        #region 数据存储
        //public static ObservableCollection<ContractQPClass> fuPanDataList = new(); // 复盘棋谱步骤列表，后期将弃用本变量
        public static QiPuRecord qiPuRecordRoot = new(); // 棋谱树型数据结构
        public static QiPuSimpleRecord qiPuSimpleRecordRoot = new(); // 棋谱树型数据结构的精简版
        //public static List<Qipu.ContractQPClass> cnNumberList = new();  // 棋谱中文步骤列表
        #endregion

        #region 棋子及棋盘基础数据
        /// <summary>
        /// 棋子编号0-31，分别对应的图像文件名
        /// </summary>
        public static readonly string[] qiZiImageFileName = {
            "黑将", "黑士", "黑士", "黑象", "黑象", "黑马", "黑马", "黑车", "黑车", "黑炮", "黑炮", "黑卒", "黑卒", "黑卒", "黑卒", "黑卒",
            "红帅", "红仕", "红仕", "红相", "红相", "红马", "红马", "红车", "红车", "红炮", "红炮", "红兵", "红兵", "红兵", "红兵", "红兵"
        };
        /// <summary>
        /// 棋子的中文简称，用于棋谱翻译
        /// </summary>
        public static readonly string[] qiZiCnName = {
            "将", "士", "士", "象", "象", "马", "马", "车", "车", "炮", "炮", "卒", "卒", "卒", "卒", "卒",
            "帅", "仕", "仕", "相", "相", "马", "马", "车", "车", "炮", "炮", "兵", "兵", "兵", "兵", "兵"
        };
        /// <summary>
        /// 棋子初始位置
        /// </summary>
        public static readonly int[,] qiZiInitPosition = new int[32, 2]
        {
            {4, 0},{3, 0},{5, 0},{2, 0},{6, 0},{1, 0},{7, 0},{0, 0},{8, 0},
            {1, 2},{7, 2},
            {0, 3},{2, 3},{4, 3},{6, 3},{8, 3},
            {4, 9},{3, 9},{5, 9},{2, 9},{6, 9},{1, 9},{7, 9},{0, 9},{8, 9},
            {1, 7},{7, 7},
            {0, 6},{2, 6},{4, 6},{6, 6},{8, 6}
        };
        /// <summary>
        /// 残局设计时棋子的初始位置
        /// </summary>
        public static readonly int[,] qiZiCanJuInitPosition = new int[32, 2]
        {
            {0, -1},{1, -1},{1, -1},{2, -1},{2, -1},{3, -1},{3, -1},{4, -1},{4, -1},
            {5, -1},{5, -1},
            {6, -1},{6, -1},{6, -1},{6, -1},{6, -1},
            {0, 10},{1, 10},{1, 10},{2, 10},{2, 10},{3, 10},{3, 10},{4, 10},{4, 10},
            {5, 10},{5, 10},
            {6, 10},{6, 10},{6, 10},{6, 10},{6, 10}
        };
        const int gw = 35;
        /// <summary>
        /// 棋盘每一格的列坐标
        /// </summary>
        /// 
        public static readonly double[] QiPanGrid_X = new double[9]
        {
            75.0 + gw, 143.0 + gw, 211.0 + gw, 278.0 + gw, 346.0 + gw, 413.0 + gw, 480.0 + gw, 548.0 + gw, 616.0 + gw
        };
        /// <summary>
        /// 棋盘每一格的行坐标
        /// </summary>
        public static readonly double[] QiPanGrid_Y = new double[10]
        {
            61.0 + gw, 130.0 + gw, 197.0 + gw, 264.0 + gw, 332.0 + gw, 400.0 + gw, 467.0 + gw, 535.0 + gw, 603.0 + gw, 669.0 + gw
        };
        public static readonly double QiPanGrid_Y_0 = -20;
        public static readonly double QiPanGrid_Y_10 = 750;
        /// <summary>
        /// 阿拉伯数字0-9，对应的中文数字
        /// </summary>
        public static readonly string[] CnNumber = { "", "一", "二", "三", "四", "五", "六", "七", "八", "九" };
        #endregion


        /// <summary>
        /// 棋子移动的处理，如果棋子移动后配方被将军，则不能移动。
        /// </summary>
        /// <param name="qiZi">棋子编号</param>
        /// <param name="m">目的地的列</param>
        /// <param name="n">目的地的行</param>
        /// <param name="dieQiZi">所杀死的棋子的编号，-1表示没有杀死棋子</param>
        /// <param name="sound">是否打开声音效果</param>
        public static bool QiZiMoveTo(int qiZi, int m, int n, bool sound)  // 运子
        {
            if (qiZi is < 0 or > 31) return false;
            if (GlobalValue.IsGameOver == true) return false;
            qiZiArray[qiZi].Select(); // 人工走棋时，引代码多余。仅用于电脑走棋时，模拟棋子被选中。
            // 运子到(m,n)位置
            int x0 = qiZiArray[qiZi].Col;
            int y0 = qiZiArray[qiZi].Row;
            int dieQiZi = QiPan[m, n];

            AnimationMove(qiZi, x0, y0, m, n); // 动画为异步运行，要注意系统数据的更新是否同步，放在此处，是为了提高应用体验，点击时能够有所反馈。后期注意验证。
            if (sound && Settings.Default.EnableSound)
            {
                player.Open(new Uri("sounds/go.mp3", UriKind.Relative));
                player.Play();
            }

            if (MoveCheck.AfterMoveStillJiangJun(qiZi, m, n, QiPan)) return false; // 如果棋子移动后，本方处于将军状态，则不可以移动。

            qiZiArray[qiZi].SetPosition(m, n);
            GlobalValue.QiPan[x0, y0] = -1;
            GlobalValue.QiPan[m, n] = qiZi;
            arrows.HideAllPath();  // 隐藏提示箭头
            AddQiPuItem(qiZi, x0, y0, m, n, dieQiZi); // 增加一行棋谱记录

            for (int i = 0; i <= 8; i++)
            {
                for (int j = 0; j <= 9; j++)
                {
                    pathPointImage[i, j].HasPoint = false; // 走棋后，隐藏走棋路径
                }
            }
            SideTag = !SideTag;  // 变换走棋方
            if (dieQiZi != -1) // 如果杀死了棋子
            {
                qiZiArray[dieQiZi].SetDied();
                if (dieQiZi is 0 or 16) // 将帅被吃，则游戏结束。本系统通过将军和绝杀判断，不允许出现此种情况。仅仅暂且保留此代码。
                {
                    IsGameOver = true;
                    return false;
                }
            }
            if (JiangJun.IsJueSha(qiZi)) // 检查是否绝杀
            {
                IsGameOver = true;
                return false;
            }
            if (JiangJun.IsKunBi(SideTag)) // 检查是否困毙
            {
                IsGameOver = true;
                if (SideTag == BLACKSIDE)
                {
                    jiangJunTiShiText = "战况信息：" + Environment.NewLine + "【黑方】被困毙！";
                }
                if (SideTag == REDSIDE)
                {
                    jiangJunTiShiText = "战况信息：" + Environment.NewLine + "【红方】被困毙！";
                }
                return false;
            }
            if (GlobalValue.qiPuRecordRoot.IsLianSha()) // 检查是否连杀超过3次
            {
                IsGameOver = true;
                if (SideTag == BLACKSIDE)
                {
                    jiangJunTiShiText = "战况信息：" + Environment.NewLine + "【红方】连续吃将未变招，判负！";
                }
                if (SideTag == REDSIDE)
                {
                    jiangJunTiShiText = "战况信息：" + Environment.NewLine + "【黑方】连续吃将未变招，判负！";
                }
                return false;
            }

            CurrentQiZi = 100;  //  当前预选棋子设为无效棋子
            //AnimationMove(qiZi, x0, y0, m, n); // 动画为异步运行，要注意系统数据的更新是否同步，因此将动画放在最后执行，避免所取数据出现错误。
            //Delay(200);
            BestMoveInfoText = Engine.XQEngine.UcciInfo.GetBestMove(false); // 调用象棋引擎，得到下一步推荐着法
            return true;
        }
        /// <summary>
        /// 自由摆放棋子，主要用于残局设计
        /// </summary>
        /// <param name="qiZi"></param>
        /// <param name="m"></param>
        /// <param name="n"></param>
        /// <param name="sound"></param>
        public static void QiZiFreeMoveTo(int qiZi, int m, int n, bool sound)  // 运子
        {
            if (qiZi is < 0 or > 31) return;
            // 运子到(m,n)位置
            int x0 = qiZiArray[qiZi].Col;
            int y0 = qiZiArray[qiZi].Row;
            if (x0 == m && y0 == n) return;
            int dieQiZi = QiPan[m, n];

            AnimationMove(qiZi, x0, y0, m, n); // 动画为异步运行，要注意系统数据的更新是否同步，放在此处，是为了提高应用体验，点击时能够有所反馈。后期注意验证。
            if (sound && Settings.Default.EnableSound)
            {
                player.Open(new Uri("sounds/go.mp3", UriKind.Relative));
                player.Play();
            }
            if (!MoveCheck.FreeMoveCheck(qiZi, m, n)) return; // 棋子摆放位置是否合规

            qiZiArray[qiZi].SetPosition(m, n);
            if (y0 >= 0 && y0 < 10) GlobalValue.QiPan[x0, y0] = -1;
            GlobalValue.QiPan[m, n] = qiZi;

            for (int i = 0; i <= 8; i++)
            {
                for (int j = 0; j <= 9; j++)
                {
                    pathPointImage[i, j].HasPoint = false; // 走棋后，隐藏走棋路径
                }
            }
            if (dieQiZi != -1) // 如果杀死了棋子
            {
                qiZiArray[dieQiZi].SetInitPosition();
            }
            //CurrentQiZi = 100;  //  当前预选棋子设为无效棋子
            AnimationMove(qiZi, x0, y0, m, n); // 动画为异步运行，要注意系统数据的更新是否同步，因此将动画放在最后执行，避免所取数据出现错误。

            //Delay(500);
        }
        /// <summary>
        /// 添加一条棋谱记录
        /// </summary>
        /// <param name="QiZi"></param>
        /// <param name="x0"></param>
        /// <param name="y0"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="DieQz"></param>
        public static void AddQiPuItem(int QiZi, int x0, int y0, int x1, int y1, int DieQz)
        {
            string char1 = GlobalValue.qiZiCnName[QiZi];
            string char2 = QiZi is > 0 and < 15 ? (x0 + 1).ToString() : GlobalValue.CnNumber[9 - x0];
            string char3 = "";
            string char4;
            #region 棋谱翻译为中文
            int m = Math.Abs(y1 - y0);
            // 进退平
            if (y0 == y1)
            {
                char3 = "平";
                char4 = QiZi is >= 0 and <= 15 ? (x1 + 1).ToString() : GlobalValue.CnNumber[9 - x1];
            }
            else
            {
                if (QiZi is >= 0 and <= 15)
                {
                    char3 = y1 > y0 ? "进" : "退";
                }
                if (QiZi is >= 16 and <= 31)
                {
                    char3 = y1 > y0 ? "退" : "进";
                }

                char4 = QiZi switch
                {
                    1 or 2 or 3 or 4 or 5 or 6 => (x1 + 1).ToString(),
                    17 or 18 or 19 or 20 or 21 or 22 => GlobalValue.CnNumber[9 - x1],
                    // 其他所有可以直走的棋子
                    _ => QiZi is > 0 and < 15 ? m.ToString() : GlobalValue.CnNumber[m],
                };

            }
            #endregion
            QiPuList.Add(new ContractQPClass()
            {
                Id = QiPuList.Count + 1,
                Nm = $"{QiZi:d2} {x0:d} {y0:d} {x1:d} {y1:d} {DieQz:d}",
                Cn = char1 + char2 + char3 + char4,
                Remarks = "",
                StepData = new StepCode(QiZi, x0, y0, x1, y1, DieQz)
            });


            QiPuRecord QRecord = new();
            QRecord.SetRecordData(QiZi, x0, y0, x1, y1, DieQz);
            GlobalValue.qiPuRecordRoot.Cursor = GlobalValue.qiPuRecordRoot.Cursor.AddChild(QRecord);  // 棋谱增加新的节点，指针更新为该节点
            GlobalValue.qiPuRecordRoot.Cursor.IsSelected = true;

            GlobalValue.qiPuSimpleRecordRoot = GlobalValue.ConvertQiPuToSimple(GlobalValue.qiPuRecordRoot);  // 更新简易棋谱记录

            ContractQiPu.ConvertFromQiPuRecord(GlobalValue.qiPuRecordRoot);
            //x0 = 100;
        }

        /// <summary>
        /// 走棋动画
        /// </summary>
        /// <param name="qiZi"></param>
        /// <param name="x0"></param>
        /// <param name="y0"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        private static void AnimationMove(int qiZi, int x0, int y0, int x1, int y1)
        {
            const double delayTime = 200.0; // 动画延续时间，毫秒
            double Grid_y0 = 0.0;
            #region 棋子有效位置检测

            if (y0 >= 0 && y0 < 10) // 棋子在棋盘内
            {
                Grid_y0 = QiPanGrid_Y[y0] - GRID_WIDTH / 2;
            }
            if (y0 == -1) // 残局设计时，棋子初始位置在棋盘外
            {
                Grid_y0 = QiPanGrid_Y_0;
            }
            if (y0 == 10) // 残局设计时，棋子初始位置在棋盘外
            {
                Grid_y0 = QiPanGrid_Y_10;
            }
            double Grid_y1 = 0.0;
            if (y1 >= 0 && y1 < 10) // 棋子在棋盘内
            {
                Grid_y1 = QiPanGrid_Y[y1] - GRID_WIDTH / 2;
            }
            if (y1 == -1) // 残局设计时，棋子初始位置在棋盘外
            {
                Grid_y1 = QiPanGrid_Y_0;
            }
            if (y1 == 10) // 残局设计时，棋子初始位置在棋盘外
            {
                Grid_y1 = QiPanGrid_Y_10;
            }
            #endregion
            #region 棋子移动动画参数设置
            Storyboard sb = new();
            DoubleAnimation PAx = new()
            {
                From = QiPanGrid_X[x0] - GRID_WIDTH / 2,
                To = QiPanGrid_X[x1] - GRID_WIDTH / 2,
                FillBehavior = FillBehavior.Stop,
                Duration = new Duration(TimeSpan.FromSeconds(delayTime / 1000))
            };
            DoubleAnimation PAy = new()
            {
                From = Grid_y0,
                To = Grid_y1,
                FillBehavior = FillBehavior.Stop,
                Duration = new Duration(TimeSpan.FromSeconds(delayTime / 1000))
            };

            if (IsQiPanFanZhuan)
            {
                PAx.From = QiPanGrid_X[8 - x0] - GRID_WIDTH / 2;
                PAx.To = QiPanGrid_X[8 - x1] - GRID_WIDTH / 2;
                PAy.From = QiPanGrid_Y[9 - y0] - GRID_WIDTH / 2;
                PAy.To = QiPanGrid_Y[9 - y1] - GRID_WIDTH / 2;
            }

            Storyboard.SetTarget(PAx, qiZiArray[qiZi]);
            Storyboard.SetTarget(PAy, qiZiArray[qiZi]);
            Storyboard.SetTargetProperty(PAx, new PropertyPath(Canvas.LeftProperty));
            Storyboard.SetTargetProperty(PAy, new PropertyPath(Canvas.TopProperty));
            sb.Children.Add(PAx);
            sb.Children.Add(PAy);
            sb.Begin();
            //qiZiArray[qiZi].Effect.BeginAnimation(DropShadowEffect.ShadowDepthProperty, DA);
            #endregion
            //qiZiArray[qiZi].BeginAnimation(Canvas.LeftProperty, PAx); // 棋子移动动画
            //qiZiArray[qiZi].BeginAnimation(Canvas.TopProperty, PAy);
        }

        /// <summary>
        /// 初始化界面，棋盘设置为开局状态，但棋盘翻转状态不会重置
        /// </summary>
        public static void Reset()
        {
            foreach (QiZi item in qiZiArray)
            {
                item.SetInitPosition(); // 所有棋子的位置信息复位
            }

            for (int i = 0; i <= 8; i++)
            {
                for (int j = 0; j <= 9; j++)
                {
                    QiPan[i, j] = -1; // 棋盘数据清空
                    pathPointImage[i, j].HasPoint = false; // 走棋路径点清空
                }
            }
            for (int i = 0; i < 32; i++)
            {
                QiPan[qiZiArray[i].Col, qiZiArray[i].Row] = i; // 棋子位置信息转换到棋盘上
            }
            yuanWeiZhi.HiddenYuanWeiZhiImage(); // 原位置标志隐藏
            Qipu.QiPuList.Clear(); // 棋谱记录清空

            arrows.HideAllPath();  // 隐藏所有提示箭头

            qiPuRecordRoot.Cursor = qiPuRecordRoot;  // 棋谱记录指针回到根部
            qiPuRecordRoot.DeleteChildNode(); // 棋谱记录清除所有子节点

            GlobalValue.EnableGameStop = false;
            IsGameOver = false;
            SideTag = REDSIDE; // 红方先走
        }

        /// <summary>
        /// 悔棋按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void HuiQi()
        {
            if (Qipu.QiPuList.Count < 1)
            {
                return;
            }

            Qipu.StepCode step = Qipu.QiPuList[^1].StepData; // ^1：索引运算符，表示倒数第一个
            qiZiArray[step.QiZi].Select();  // 重新计算可移动路径
            qiZiArray[step.QiZi].SetPosition(step.X0, step.Y0);
            AnimationMove(step.QiZi, step.X1, step.Y1, step.X0, step.Y0);
            qiZiArray[step.QiZi].Select();
            qiZiArray[step.QiZi].Deselect();

            if (step.DieQz > -1)
            {
                qiZiArray[step.DieQz].Setlived();
                qiZiArray[step.DieQz].SetPosition(step.X1, step.Y1);
            }
            QiPan[step.X0, step.Y0] = step.QiZi;
            QiPan[step.X1, step.Y1] = step.DieQz;
            Qipu.QiPuList.RemoveAt(Qipu.QiPuList.Count - 1);
            SideTag = !SideTag;
            foreach (var item in pathPointImage)
            {
                item.HasPoint = false;
            }
            if (qiPuRecordRoot.Cursor.GetParent() != null)
            {
                qiPuRecordRoot.Cursor = qiPuRecordRoot.Cursor.GetParent();
                qiPuRecordRoot.Cursor.IsSelected = true;
            }
        }
        /// <summary>
        /// 下一步
        /// </summary>
        public static int NextStep()
        {
            QiPuRecord cursor = qiPuRecordRoot.Cursor;
            if (cursor.ChildNode.Count == 1)
            {
                cursor = cursor.ChildNode[0];
                StepCode step = cursor.StepData;
                step.LunchStep();
                //QiZiMoveTo(step.QiZi, step.X1, step.Y1, true);
                qiPuRecordRoot.Cursor = cursor;
                return 1;
            }
            if (cursor.ChildNode.Count > 1)
            {
                return cursor.ChildNode.Count;
            }
            return 0;
        }
        public static void NextStep(string childId)
        {
            NextStep(int.Parse(childId));
        }
        public static void NextStep(int childId)
        {
            QiPuRecord cursor = qiPuRecordRoot.Cursor;
            if (childId > 0 && childId <= cursor.ChildNode.Count)
            {
                cursor = cursor.ChildNode[childId - 1];
                StepCode step = cursor.StepData;
                step.LunchStep();
                //QiZiMoveTo(step.QiZi, step.X1, step.Y1, true);
                qiPuRecordRoot.Cursor = cursor;
            }
        }

        /// <summary>
        /// 将全记录棋谱转化为简易记录棋谱，经JsonConvert.SerializeObject,存入数据库。目的是压缩数据量。
        /// </summary>
        /// <param name="fullQiPu">全局变量QiPuRecordRoot</param>
        /// <returns>简易记录棋谱</returns>
        public static QiPuSimpleRecord ConvertQiPuToSimple(QiPuRecord fullQiPu)
        {
            QiPuSimpleRecord simpleQiPu = new()
            {
                Id = fullQiPu.Id,
                Remarks = fullQiPu.Remarks
            };
            simpleQiPu.CopyDataFromStep(fullQiPu.StepData);
            foreach (QiPuRecord Recode in fullQiPu.ChildNode)
            {
                QiPuSimpleRecord childRecode = ConvertQiPuToSimple(Recode);
                simpleQiPu.Child.Add(childRecode);
            }
            return simpleQiPu;
        }

        /// <summary>
        /// 将简易记录棋谱转化为全记录棋谱。用于从数据库读取数据后，经JsonConvert.DeserializeObject，存入全局变量QiPuRecordRoot
        /// </summary>
        /// <param name="simpleQiPu">全局变量QiPuSimpleRecordRoot</param>
        /// <returns>全记录棋谱</returns>
        public static QiPuRecord ConvertQiPuToFull(QiPuSimpleRecord simpleQiPu)
        {
            QiPuRecord qiPu = new()
            {
                Id = simpleQiPu.Id,
                Remarks = simpleQiPu.Remarks
            };
            qiPu.CopyRecordData(simpleQiPu.Data);
            foreach (QiPuSimpleRecord Recode in simpleQiPu.Child)
            {
                QiPuRecord childRecode = ConvertQiPuToFull(Recode);
                qiPu.AddChild(childRecode);

            }
            return qiPu;
        }
        /// <summary>
        /// 转换为中文走棋指令
        /// </summary>
        /// <param name="QiZi"></param>
        /// <param name="x0"></param>
        /// <param name="y0"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <returns></returns>
        public static string TranslateToCN(int QiZi, int x0, int y0, int x1, int y1)
        {
            string char1 = GlobalValue.qiZiCnName[QiZi];
            string char2 = QiZi is >= 0 and <= 15 ? (x0 + 1).ToString() : GlobalValue.CnNumber[9 - x0];
            string char3 = "";
            string char4;
            #region 棋谱翻译为中文
            int m = Math.Abs(y1 - y0);
            // 进退平
            if (y0 == y1)
            {
                char3 = "平";
                char4 = QiZi is >= 0 and <= 15 ? (x1 + 1).ToString() : GlobalValue.CnNumber[9 - x1];
            }
            else
            {
                if (QiZi is >= 0 and <= 15)
                {
                    char3 = y1 > y0 ? "进" : "退";
                }
                if (QiZi is >= 16 and <= 31)
                {
                    char3 = y1 > y0 ? "退" : "进";
                }

                char4 = QiZi switch
                {
                    1 or 2 or 3 or 4 or 5 or 6 => (x1 + 1).ToString(),
                    17 or 18 or 19 or 20 or 21 or 22 => GlobalValue.CnNumber[9 - x1],
                    // 其他所有可以直走的棋子
                    _ => QiZi is >= 0 and <= 15 ? m.ToString() : GlobalValue.CnNumber[m],
                };

            }
            #endregion
            return char1 + char2 + char3 + char4;
        }

        /// <summary>
        /// 延时函数，延时期间可接收和执行事件，解决了系统假死问题
        /// </summary>
        /// <param name="milliSecond"></param>
        public static void Delay(int milliSecond)
        {
            int start = Environment.TickCount;
            while (Math.Abs(Environment.TickCount - start) < milliSecond)
            {
                DispatcherHelper.DoEvents();
            }
        }
        public static class DispatcherHelper
        {
            //[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
            public static void DoEvents()
            {
                DispatcherFrame frame = new();
                // Dispatcher的作用是用于管理线程工作项队列，类似于Win32中的消息队列
                Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(ExitFrames), frame);
                try { Dispatcher.PushFrame(frame); }
                catch (InvalidOperationException) { }
            }
            private static object ExitFrames(object frame)
            {
                ((DispatcherFrame)frame).Continue = false;
                return null;
            }
        }
    }
}


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Chess.Engine
{
    public class XQEngine
    {
        public static UcciInfoClass UcciInfo = new();   // 推荐着法存储类

        /// <summary>
        /// 调用象棋引擎eleeye.exe，获取推荐走棋着法
        /// </summary>
        /// <param name="thinkTime">思考限制时间</param>
        /// <param name="depth">计算深度</param>
        /// <returns></returns>
        public static string CallEngine(int thinkTime, int depth)
        {
            string fenstr = QiPanDataToFenStr();
            Process proEngine = new()
            {
                StartInfo = new ProcessStartInfo()
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    CreateNoWindow = true,
                    FileName = "Engine/eleeye.exe"
                }
            };
            proEngine.Start();
            proEngine.StandardInput.WriteLine("ucci");
            proEngine.StandardInput.WriteLine("setoption batch on");
            proEngine.StandardInput.WriteLine("position fen " + fenstr);
            proEngine.StandardInput.WriteLine("go time " + thinkTime.ToString());
            proEngine.StandardInput.WriteLine("go depth " + depth.ToString());
            proEngine.StandardInput.WriteLine("quit");
            string output = proEngine.StandardOutput.ReadToEnd();
            proEngine.Close();
            if (!output.Contains("nobestmove"))
            {
                // 裁剪掉无用信息
                int start = output.IndexOf("info depth");
                output = output[start..];
                output = output.Replace("bye" + Environment.NewLine, "");
            }
            else
            {
                output = "No BestMove!";
            }
            return output;
        }

        // UCCI标准棋子名称，小写为黑方，对应0-15，大写为红方，对应16-31，见GlobalValue.qiZiCnName棋子名称数组
        private static readonly string[] Fen = {
            "k","a","a","b","b","n","n","r","r","c","c","p","p","p","p","p",
            "K","A","A","B","B","N","N","R","R","C","C","P","P","P","P","P"
        };
        public static string QiPanDataToFenStr_header()
        {
            #region 棋盘数据生成FEN字符串
            string output = "";
            for (int i = 0; i < 10; i++) // 行
            {
                string lineString = "";
                int spaceNum = 0;
                for (int j = 0; j < 9; j++) // 列
                {
                    int qz = GlobalValue.QiPan[j, i];
                    if (qz == -1)
                    {
                        spaceNum++;
                    }
                    if (qz > -1)
                    {
                        if (spaceNum > 0)
                        {
                            lineString += spaceNum.ToString();
                            spaceNum = 0;
                        }
                        lineString += Fen[qz];
                    }
                }
                if (spaceNum > 0)
                {
                    lineString += spaceNum.ToString();
                }
                output += lineString;
                if (i < 9)
                {
                    output += "/";
                }
            }
            return output;
            #endregion
        }
        /// <summary>
        /// 根据棋盘数据，生成FEN字符串
        /// </summary>
        /// <returns></returns>
        public static string QiPanDataToFenStr()
        {
            #region 棋盘数据生成FEN字符串
            string output = QiPanDataToFenStr_header();

            if (GlobalValue.SideTag == GlobalValue.BLACKSIDE)
            {
                output += " b";
            }
            else
            {
                output += " w";
            }
            //output += " - - 0 1";
            output += GlobalValue.qiPuRecordRoot.Get6Moves();
            return output;
            #endregion
        }
        /// <summary>
        /// 将FEN串信息转换到棋谱数组中
        /// </summary>
        /// <param name="fen">FEN串</param>
        /// <returns>棋谱数组int[9,10]</returns>
        public static int[,] ConvertFenStrToQiPan(string fen)
        {
            int[,] output = new int[9, 10];
            int col, row;
            col = 0;
            row = 0;
            string[] locFen = new string[Fen.Length];
            Array.Copy(Fen, locFen, Fen.Length);

            foreach (char c in fen)
            {
                if (c == '/')
                {
                    row++;
                    col = 0;
                    continue;
                }
                if (char.IsDigit(c)) // 如果是数字
                {
                    int num = (int)Char.GetNumericValue(c);
                    while (num > 0)
                    {
                        output[col, row] = -1;
                        col++;
                        num--;
                    }
                }
                else
                {
                    // 如果是字符
                    int offset = Array.IndexOf(locFen, c + "");
                    output[col, row] = offset;
                    col++;
                    locFen[offset] = "";

                }
            }
            return output;
        }
        /// <summary>
        /// 头条着法存储类，用于存储单行UCCI的解析信息
        /// </summary>
        public class PvClass
        {
            internal int Depth { get; set; }
            internal int Score { get; set; }
            internal List<string> UcciStrList { get; set; }
            internal List<string> CnStrList { get; set; }
            public CustomClass.Qipu.StepCode FirstStep { get; set; }
            private string _InfoStrLine;
            internal string InfoStrLine
            {
                get { return _InfoStrLine; }
                set
                {
                    _InfoStrLine = value.Trim();
                    UcciStrList.Clear();
                    CnStrList.Clear();
                    Depth = 0;
                    Score = 0;
                    if (!_InfoStrLine.Contains("info depth"))
                    {
                        return;
                    }
                    string[] strarr = _InfoStrLine.Split(" ");
                    Depth = int.Parse(strarr[Array.IndexOf(strarr, "depth") + 1]); // 搜索深度
                    Score = int.Parse(strarr[Array.IndexOf(strarr, "score") + 1]); // 分值
                    int start = Array.IndexOf(strarr, "pv") + 1; // “pv”后边是着法，着法可能仅有一个，也可能是一串。
                    for (int i = start; i < strarr.Length; i++)
                    {
                        UcciStrList.Add(strarr[i].Trim());
                    }
                    FirstStep = GetStep(UcciStrList[0], GlobalValue.QiPan);
                    CnStrList = UcciStrToCnStr(UcciStrList, GlobalValue.QiPan);
                }
            }
            /// <summary>
            /// 类初始化
            /// </summary>
            public PvClass()
            {
                UcciStrList = new List<string>();
                CnStrList = new List<string>();
            }
            /// <summary>
            /// 获取着法信息串
            /// </summary>
            /// <returns></returns>
            internal string GetMove()
            {
                string movestr = "";
                if (CnStrList.Count > 0)
                {
                    foreach (string str in CnStrList)
                    {
                        movestr += str + " ";
                    }
                }
                else
                {
                    movestr = "无";
                }
                return movestr;
            }
            /// <summary>
            /// 获取着法的原位置和目标位置坐标数据，用于电脑走棋和箭头提示
            /// </summary>
            /// <returns></returns>
            public List<System.Drawing.Point> GetPoint()
            {
                List<System.Drawing.Point> points = new();
                System.Drawing.Point pt0 = new(FirstStep.X0, FirstStep.Y0);
                points.Add(pt0);
                System.Drawing.Point pt1 = new(FirstStep.X1, FirstStep.Y1);
                points.Add(pt1);
                return points;
            }
        }

        /// <summary>
        /// 着法类，存储UCCI的全部解析信息
        /// </summary>
        public class UcciInfoClass
        {
            private string _InfoSource;
            private string InfoSource // 象棋引擎返回的多行字符串，需里进行解析，并保存到相应变量中。
            {
                get { return _InfoSource; }
                set
                {
                    _InfoSource = value;
                    InfoList.Clear();
                    Bestmove = "";
                    Ponder = "";
                    if (!_InfoSource.Contains("info depth"))
                    {
                        return;
                    }
                    string keystr = "bestmove"; // 最佳着法
                    int start = _InfoSource.IndexOf(keystr);
                    if (start > -1)
                    {
                        string uccistr = InfoSource.Substring(start + keystr.Length, 5).Trim();
                        Bestmove = UcciStrToCnStr(uccistr, GlobalValue.QiPan, false);
                    }

                    keystr = "ponder"; // 后续的最佳着法
                    start = InfoSource.IndexOf(keystr);
                    if (start > -1)
                    {
                        string uccistr = InfoSource.Substring(start + keystr.Length, 5).Trim();
                        Ponder = UcciStrToCnStr(uccistr, GlobalValue.QiPan, false);
                    }

                    int end = InfoSource.IndexOf("bestmove"); // 删除bestmove所在行
                    string[] bufArr = InfoSource[..end].Trim().Split(Environment.NewLine); // 按行分割字符串为数组

                    foreach (string str in bufArr)
                    {
                        PvClass pvc = new()
                        {
                            InfoStrLine = str
                        };
                        InfoList.Add(pvc);
                    }
                }
            }
            private string Bestmove;    // 最佳着法
            private string Ponder;  // 后续的最佳着法
            private List<PvClass> InfoList { get; set; }
            public UcciInfoClass()
            {
                InfoList = new List<PvClass>();
            }

            /// <summary>
            /// 获取最佳着法，同时显示所有推荐着法的提示箭头
            /// </summary>
            /// <param name="showarrow">得到最佳着法后，是否在棋盘上显示指示箭头</param>
            /// <returns>最佳着法</returns>
            public string GetBestMove(bool showarrow)
            {
                InfoSource = CallEngine(10, 5); // 调用象棋引擎，获得下一步着法信息
                string str = "";
                if (InfoList.Count > 0)
                {
                    int maxscore = InfoList.Max(x => x.Score);
                    str = InfoList.FirstOrDefault(x => x.Score == maxscore).GetMove();
                    str = $"最佳着法（{maxscore}分）：{System.Environment.NewLine} {str}";
                }
                else
                {
                    str = $"最佳着法： {System.Environment.NewLine}{InfoSource}";
                }
                if (showarrow || MainWindow.menuItem == GlobalValue.PERSON_PC) ShowArrows();
                return str;
            }

            /// <summary>
            /// 显示所有推荐着法的提示箭头。
            /// 提示箭头的数量限定为5个。
            /// </summary>
            private void ShowArrows()
            {
                GlobalValue.arrows.HideAllPath();
                if (InfoList.Count > 0)
                {
                    List<List<System.Drawing.Point>> points = new();
                    foreach (PvClass p in InfoList)
                    {
                        points.Add(p.GetPoint());
                    }
                    int arrowCount = (points.Count > Settings.Default.ArrowsMaxNum) ? Settings.Default.ArrowsMaxNum : points.Count; // 提示箭头的数量限定为5个。超过5个时，多余的没有用，且界面太乱。
                    for (int i = 0; i < arrowCount; i++)
                    {
                        bool sameTargetPoint = false;
                        if (i > 0)
                        {
                            for (int j = 0; j < i; j++)
                            {
                                if (points[i][1].X == points[j][1].X && points[i][1].Y == points[j][1].Y)
                                {
                                    sameTargetPoint = true;
                                    break;
                                }
                            }
                        }
                        string tipInfo = $"{InfoList[i].Score}分{Environment.NewLine}{InfoList[i].GetMove()}";
                        GlobalValue.arrows.SetPathData(i, points[i][0], points[i][1], sameTargetPoint, tipInfo);
                    }
                }
                GlobalValue.arrows.ShowAllPath();
            }
            public CustomClass.Qipu.StepCode GetBestSetp()
            {
                if (InfoList.Count > 0) return InfoList[0].FirstStep;
                return null;
            }
        }

        /// <summary>
        /// 将单个UCCI着法指令，转换为中文着法指令
        /// </summary>
        /// <param name="fenStr">UCCI着法指令，例如"h9g8"</param>
        /// <param name="curQiPan">棋盘数据数组</param>
        /// <returns>中文着法指令,例如“马八进七”</returns>
        private static List<string> UcciStrToCnStr(List<string> strlist, int[,] curQiPan)
        {
            int[,] qipan = new int[9, 10];
            for (int i = 0; i <= 8; i++)
            {
                for (int j = 0; j <= 9; j++)
                {
                    qipan[i, j] = curQiPan[i, j];
                }
            }

            List<string> returnlist = new();
            foreach (string str in strlist)
            {
                string workStr = UcciStrToCnStr(str, qipan, true);
                returnlist.Add(workStr);
            }
            return returnlist;
        }

        /// <summary>
        /// 将单个UCCI着法，转换为中文着法
        /// </summary>
        /// <param name="ucciStr">UCCI着法</param>
        /// <param name="qipan">棋盘数据</param>
        /// <param name="writeQipan">是否有后续着法，用于避免改动系统现棋盘数据，导致数据错误</param>
        /// <returns></returns>
        private static string UcciStrToCnStr(string ucciStr, int[,] qipan, bool writeQipan)
        {
            ucciStr = ucciStr.Trim();
            if (ucciStr.Length != 4)
            {
                return "";
            }
            string cols = "abcdefghi";
            string rows = "9876543210";
            int x0 = cols.IndexOf(ucciStr[0]);  // 现位置
            int y0 = rows.IndexOf(ucciStr[1]);
            int x1 = cols.IndexOf(ucciStr[2]);  // 目标位置
            int y1 = rows.IndexOf(ucciStr[3]);
            string resultstr = "";
            int qizi = qipan[x0, y0];
            if (qizi > -1)
            {
                resultstr = GlobalValue.TranslateToCN(qizi, x0, y0, x1, y1);
                if (qizi<16) resultstr+=System.Environment.NewLine;
                if (writeQipan)
                {
                    qipan[x0, y0] = -1;
                    qipan[x1, y1] = qizi;
                }
            }
            return resultstr;
        }

        /// <summary>
        /// 将UCCI着法转换为step着法数据
        /// </summary>
        /// <param name="ucciStr">UCCI着法字符串，一般是从象棋引擎中取得</param>
        /// <param name="qipan">棋盘数据。此变量建议保留。</param>
        /// <returns></returns>
        private static CustomClass.Qipu.StepCode GetStep(string ucciStr, int[,] qipan)
        {
            ucciStr = ucciStr.Trim();
            if (ucciStr.Length != 4)
            {
                return null;
            }
            string cols = "abcdefghi";
            string rows = "9876543210";
            int x0 = cols.IndexOf(ucciStr[0]);  // 现位置
            int y0 = rows.IndexOf(ucciStr[1]);
            int x1 = cols.IndexOf(ucciStr[2]);  // 目标位置
            int y1 = rows.IndexOf(ucciStr[3]);
            int qizi = qipan[x0, y0];
            int dieQiZi = qipan[x1, y1];
            CustomClass.Qipu.StepCode step = new(qizi, x0, y0, x1, y1, dieQiZi);
            return step;
        }
    }
}

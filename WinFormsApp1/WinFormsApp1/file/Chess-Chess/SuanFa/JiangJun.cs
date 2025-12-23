using System;
using System.Collections;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;

namespace Chess.SuanFa // 算法
{
    /// <summary>
    /// 将军
    /// </summary>
    class JiangJun  // 将军
    {
        /// <summary>
        /// 棋子移动后，判断对方是否被绝杀
        /// </summary>
        /// <param name="moveQiZi">最后移动的棋子</param>
        /// <returns>true=已被绝杀</returns>
        public static bool IsJueSha(int moveQiZi)
        {

            int[] jiangJunQiZi = { -1, -1, -1 };
            if (moveQiZi < 16) jiangJunQiZi = IsJiangJun(16); // 检查红帅是否被将军。
            if (moveQiZi >= 16) jiangJunQiZi = IsJiangJun(0); // 检查黑将是否被将军
            GlobalValue.jiangJunTiShiText = "战况信息："; // 在棋盘上部用文字显示棋局状态，主要用于调试，后期可优化为图像模式
            if (jiangJunQiZi[0] == -1) return false;  // 没有被将军时，则不需检测是否绝杀

            string gongJiQiZi1; // 第一个攻击棋子的名字
            if (jiangJunQiZi[1] != -1) gongJiQiZi1 = GlobalValue.qiZiImageFileName[jiangJunQiZi[1]]; else gongJiQiZi1 = "";
            string gongJiQiZi2; // 第二个攻击棋子的名字
            if (jiangJunQiZi[2] != -1) gongJiQiZi2 = "和" + GlobalValue.qiZiImageFileName[jiangJunQiZi[2]]; else gongJiQiZi2 = "";

            if (jiangJunQiZi[0] == 0) // 被将军的是黑将
            {
                GlobalValue.jiangJunTiShiText = System.Environment.NewLine + "【黑将】正被将军！";

                bool[,] points = MoveCheck.GetPathPoints(0, GlobalValue.QiPan); // 获取黑将的可移动路径
                bool selfCanMove = false;
                for (int i = 3; i <= 5; i++)
                    for (int j = 0; j <= 2; j++)
                    {
                        if (points[i, j] == true && !MoveCheck.IsKilledPoint(0, i, j, GlobalValue.QiPan)) // 检查黑将可移动路径是否是对方的攻击点
                        {
                            selfCanMove = true; // 如果不是对方的攻击点，则可移动到请该点。
                            break;
                        }
                    }
                if (selfCanMove)  // 黑将可移动解杀时
                {
                    GlobalValue.jiangJunTiShiText += System.Environment.NewLine + "【黑将】被" + gongJiQiZi1 + "将军，可移动位置解杀。";
                    return false;
                }
                else  // 黑将不可移动时
                {
                    if (jiangJunQiZi[2] != -1) // 如果是双将
                    {
                        if ((jiangJunQiZi[1] is 21 or 22) || (jiangJunQiZi[2] is 21 or 22))
                        {
                            GlobalValue.jiangJunTiShiText += Environment.NewLine + "【黑将】不能移动，被" + gongJiQiZi1 + gongJiQiZi2 + "双将绝杀！";
                            return true;
                        }
                        GlobalValue.jiangJunTiShiText += Environment.NewLine + "【黑将】被" + gongJiQiZi1 + gongJiQiZi2 + "双将，请求外援！";
                    }
                    else
                    {
                        GlobalValue.jiangJunTiShiText += Environment.NewLine + "【黑将】被" + gongJiQiZi1 + "将军，不能移动，请求外援。";
                    }
                    if (!JieSha(jiangJunQiZi[1])) // 本方其他棋子解杀不成
                    {
                        GlobalValue.jiangJunTiShiText += Environment.NewLine + "【黑将】被" + gongJiQiZi1 + "绝杀！";
                        return true;
                    }
                }
            }
            if (jiangJunQiZi[0] == 16) // 被将军的是红帅
            {
                #region 被将军的是红帅
                GlobalValue.jiangJunTiShiText = Environment.NewLine + "【红帅】被" + gongJiQiZi1 + "将军！";

                bool[,] points = MoveCheck.GetPathPoints(16, GlobalValue.QiPan);
                bool selfCanMove = false;
                for (int i = 3; i <= 5; i++)
                    for (int j = 7; j <= 9; j++)
                    {
                        if (points[i, j] == true && !MoveCheck.IsKilledPoint(16, i, j, GlobalValue.QiPan))
                        {
                            selfCanMove = true;
                            break;
                        }
                    }
                if (selfCanMove)
                {
                    GlobalValue.jiangJunTiShiText = Environment.NewLine + "【红帅】被" + gongJiQiZi1 + "将军！！红帅可自己移动解杀。";
                }
                else
                {
                    if (jiangJunQiZi[2] != -1) // 双将
                    {
                        if ((jiangJunQiZi[1] is 5 or 6) || (jiangJunQiZi[2] is 5 or 6))
                        {
                            GlobalValue.jiangJunTiShiText += Environment.NewLine + "【红帅】不能移动，被" + gongJiQiZi1 + gongJiQiZi2 + "双将绝杀！";
                            return true;
                        }

                        GlobalValue.jiangJunTiShiText = Environment.NewLine + "【红帅】被" + gongJiQiZi1 + gongJiQiZi2 + "双将，不能移动，请求外援！";
                    }
                    else // 单将
                    {
                        GlobalValue.jiangJunTiShiText = Environment.NewLine + "【红帅】被" + gongJiQiZi1 + "将军，不能移动，请求外援。";
                    }
                    if (!JieSha(jiangJunQiZi[1]))  // 绝杀判断
                    {
                        GlobalValue.jiangJunTiShiText = Environment.NewLine + "【红帅】被" + gongJiQiZi1 + "绝杀！";
                        return true;
                    }

                }
                #endregion
            }
            return false;
        }
        /// <summary>
        /// 检查本棋子是否对将帅构成将军，在走棋之后判断
        /// </summary>
        /// <param name="jiangOrShuai"> 0：黑将，16：红帅 </param>
        /// <returns>
        /// 返回一维数组，其中有三个数据
        /// int[0]=-1: 没有发生将军
        /// int[0]=0: 黑将被将军
        /// int[0]=16: 红帅被将军 
        /// int[1]: 对方将军的棋子编号
        /// int[2]: 发生双将时的对方将军的第二个棋子的编号
        /// </returns>
        public static int[] IsJiangJun(int jiangOrShuai)
        {

            int[] jiangJunQiZi = { -1, -1, -1 }; // 保存发起将军的所有棋子，可能是一个，也可能是两个。
            if (jiangOrShuai != 0 && jiangOrShuai != 16) return jiangJunQiZi;
            int[,] myQiPan = new int[9, 10]; // 复制一份棋盘副本，防止破坏原棋盘数组的数据
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 10; j++)
                {
                    myQiPan[i, j] = GlobalValue.QiPan[i, j];
                }

            bool[,] thisPoints;

            if (jiangOrShuai == 16) // 被将军的是红帅
            {
                jiangJunQiZi[0] = jiangJunQiZi[1] = jiangJunQiZi[2] = -1;
                for (int qizi = 5; qizi <= 15; qizi++) //车(7,8)，马(5,6)，炮(9,10)，卒(11,12,13,14,15)
                {
                    if (GlobalValue.qiZiArray[qizi].Visibility != System.Windows.Visibility.Visible) continue; // 已死的棋子排除
                    thisPoints = MoveCheck.GetPathPoints(qizi, myQiPan);
                    int x = GlobalValue.qiZiArray[16].Col;
                    int y = GlobalValue.qiZiArray[16].Row;
                    if (thisPoints[x, y] == true)
                    {
                        jiangJunQiZi[0] = 16;
                        if (jiangJunQiZi[1] == -1)
                        {
                            jiangJunQiZi[1] = qizi; // 第一个发起将军的棋子
                        }
                        else
                        {
                            jiangJunQiZi[2] = qizi; // 双将，保存第二个发起将军的棋子
                            return jiangJunQiZi;
                        }
                    }
                }
            }
            if (jiangOrShuai == 0) // 被将军的是黑将
            {
                jiangJunQiZi[0] = jiangJunQiZi[1] = jiangJunQiZi[2] = -1;
                for (int qizi = 21; qizi <= 31; qizi++) //车(23,24)，马(21,22)，炮(25,26)，卒(27,28,29,30,31)
                {
                    if (GlobalValue.qiZiArray[qizi].Visibility != System.Windows.Visibility.Visible) continue; // 已死的棋子排除
                    thisPoints = MoveCheck.GetPathPoints(qizi, myQiPan);
                    int x = GlobalValue.qiZiArray[0].Col;
                    int y = GlobalValue.qiZiArray[0].Row;
                    if (thisPoints[x, y] == true)
                    {
                        jiangJunQiZi[0] = 0;
                        if (jiangJunQiZi[1] == -1)
                        {
                            jiangJunQiZi[1] = qizi; // 第一个发起将军的棋子
                        }
                        else
                        {
                            jiangJunQiZi[2] = qizi; // 双将，保存第二个发起将军的棋子
                            return jiangJunQiZi;
                        }
                    }
                }
            }
            return jiangJunQiZi;
        }
        /// <summary>
        /// 被将军时，在老将不能动的情况下，判断本方其他棋子能否解杀
        /// </summary>
        /// <param name="jiangJunQiZi">发起将军的棋子</param>
        /// <returns>true=能解杀，false=不能解杀</returns>
        private static bool JieSha(int jiangJunQiZi)
        {
            //黑方：车(7,8)，马(5,6)，炮(9,10)，卒(11,12,13,14,15)
            //红方：车(23,24)，马(21,22)，炮(25,26)，兵(27,28,29,30,31)
            //if (jiangJunQiZi is >= 11 and <= 15) return false;  //  黑方：卒(11,12,13,14,15)
            //if (jiangJunQiZi is >= 27 and <= 31) return false;  //  红方：兵(27,28,29,30,31)
            int jiangJunQiZi_Col = GlobalValue.qiZiArray[jiangJunQiZi].Col;
            int jiangJunQiZi_Row = GlobalValue.qiZiArray[jiangJunQiZi].Row;
            int blackJiang_Col = GlobalValue.qiZiArray[0].Col;
            int blackJiang_Row = GlobalValue.qiZiArray[0].Row;
            int redShuai_Col = GlobalValue.qiZiArray[16].Col;
            int redShuai_Row = GlobalValue.qiZiArray[16].Row;

            bool[,] points;

            #region 移子解杀。在被炮将军时，查找炮与将帅之间的被将军方的棋子，如可移开，且移开后不再将军，则解杀
            switch (jiangJunQiZi)  // 如果是炮将军时，查找炮与将帅之间的被将军方的棋子，如可移开，则解杀
            {
                case 9:
                case 10: // 攻击棋子为黑方炮(9,10)，查找黑炮与红帅之间的红方棋子，如可移开，则解杀
                    int findCol = -1;
                    int findRow = -1;
                    if (jiangJunQiZi_Col == redShuai_Col) // 黑炮与红帅在同一列时，攻击方向为纵向
                    {
                        // 在黑炮和红帅之间寻找红方棋子
                        for (int row = Math.Min(jiangJunQiZi_Row, redShuai_Row) + 1; row < Math.Max(jiangJunQiZi_Row, redShuai_Row); row++)
                        {
                            // 如果在黑炮和红帅之间找到了红方棋子，则记录该棋子位置，并停止查找
                            if (GlobalValue.QiPan[jiangJunQiZi_Col, row] is > 16 and < 32)
                            {
                                findCol = jiangJunQiZi_Col;
                                findRow = row;
                                break;
                            }
                        }
                    }
                    if (jiangJunQiZi_Row == redShuai_Row) // 黑炮与红帅在同一行时，攻击方向为横向
                    {
                        for (int col = Math.Min(jiangJunQiZi_Col, redShuai_Col) + 1; col < Math.Max(jiangJunQiZi_Col, redShuai_Col); col++)
                        {
                            if (GlobalValue.QiPan[col, jiangJunQiZi_Row] is > 16 and < 32)
                            {
                                findCol = col;
                                findRow = jiangJunQiZi_Row;
                                break;
                            }
                        }
                    }
                    // 如果没有找到可移动的棋子，则跳过。
                    if (findCol == -1 || findRow == -1) break;
                    // 否则，获取所找到棋子的可移动路径
                    points = MoveCheck.GetPathPoints(GlobalValue.QiPan[findCol, findRow], GlobalValue.QiPan);
                    for (int i = 0; i < 9; i++)
                    {
                        for (int j = 0; j < 10; j++)
                        {
                            // 逐个判断棋子的可移动路径，如果此路径点的行列位置与炮的行列均不相同，则可解杀成功。
                            if (points[i, j] == true && i != jiangJunQiZi_Col && j != jiangJunQiZi_Row)
                            {
                                if (!MoveCheck.AfterMoveStillJiangJun(GlobalValue.QiPan[findCol, findRow], findCol, findRow, GlobalValue.QiPan))
                                    return true;
                            }
                        }
                    }
                    break;

                case 25:
                case 26:    //  攻击棋子为红方炮(25,26)，查找红炮与黑将之间的黑方棋子，如可移开，则解杀
                    findCol = -1;
                    findRow = -1;
                    if (jiangJunQiZi_Col == blackJiang_Col) // 红炮与黑将在同一列时，攻击方向为纵向
                    {
                        for (int row = Math.Min(jiangJunQiZi_Row, blackJiang_Row) + 1; row < Math.Max(jiangJunQiZi_Row, blackJiang_Row); row++)
                        {
                            if (GlobalValue.QiPan[jiangJunQiZi_Col, row] is > 0 and < 16)
                            {
                                findCol = jiangJunQiZi_Col;
                                findRow = row;
                                break;
                            }
                        }
                    }
                    if (jiangJunQiZi_Row == blackJiang_Row) // 红炮与黑将在同一行时，攻击方向为横向
                    {
                        for (int col = Math.Min(jiangJunQiZi_Col, blackJiang_Col) + 1; col < Math.Max(jiangJunQiZi_Col, blackJiang_Col); col++)
                        {
                            if (GlobalValue.QiPan[col, jiangJunQiZi_Row] is > 0 and < 16)
                            {
                                findCol = col;
                                findRow = jiangJunQiZi_Row;
                                break;
                            }
                        }
                    }
                    if (findCol == -1 || findRow == -1) break;
                    points = MoveCheck.GetPathPoints(GlobalValue.QiPan[findCol, findRow], GlobalValue.QiPan);
                    for (int i = 0; i < 9; i++)
                    {
                        for (int j = 0; j < 10; j++)
                        {

                            if (points[i, j] == true && i != jiangJunQiZi_Col && j != jiangJunQiZi_Row)
                            {
                                if (!MoveCheck.AfterMoveStillJiangJun(GlobalValue.QiPan[findCol, findRow], findCol, findRow, GlobalValue.QiPan))
                                    return true;
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
            #endregion

            ArrayList jieShaPoints = new(); // 可解除攻击的点位

            #region  填子解杀。根据发起将军棋子的位置，以及被将军的将帅的位置，计算所有可解除将军的点位，存放到数组列表JieShaPoints中，以备进一步分析
            jieShaPoints.Add(new int[] { jiangJunQiZi_Col, jiangJunQiZi_Row }); // 把攻击棋子的位置先加进去
            //int[] jsPoint = new int[2];

            switch (jiangJunQiZi) // 根据发起将军棋子的位置，以及被将军的将帅的位置，计算或解除将军的所有点位，存放到数组列表中
            {
                case 5:
                case 6:     //  攻击棋子为黑方马(5,6)

                    if (jiangJunQiZi_Row - redShuai_Row == 2) // 马从上方攻击
                    {
                        jieShaPoints.Add(new int[] { jiangJunQiZi_Col, jiangJunQiZi_Row + 1 }); //  别马腿位置
                    }
                    if (redShuai_Row - jiangJunQiZi_Row == 2) // 马从下方攻击
                    {
                        jieShaPoints.Add(new int[] { jiangJunQiZi_Col, jiangJunQiZi_Row - 1 }); //  别马腿位置
                    }
                    if (jiangJunQiZi_Col - redShuai_Col == 2) // 马从右方攻击
                    {
                        jieShaPoints.Add(new int[] { jiangJunQiZi_Col - 1, jiangJunQiZi_Row }); //  别马腿位置
                    }
                    if (redShuai_Col - jiangJunQiZi_Col == 2) // 马从左方攻击
                    {
                        jieShaPoints.Add(new int[] { jiangJunQiZi_Col + 1, jiangJunQiZi_Row }); //  别马腿位置
                    }
                    break;
                case 21:
                case 22:    //  攻击棋子为红方马(21,22)
                    if (jiangJunQiZi_Row - blackJiang_Row == 2) // 马从上方攻击
                    {
                        jieShaPoints.Add(new int[] { jiangJunQiZi_Col, jiangJunQiZi_Row + 1 }); //  别马腿位置
                    }
                    if (blackJiang_Row - jiangJunQiZi_Row == 2) // 马从下方攻击
                    {
                        jieShaPoints.Add(new int[] { jiangJunQiZi_Col, jiangJunQiZi_Row - 1 }); //  别马腿位置
                    }
                    if (jiangJunQiZi_Col - blackJiang_Col == 2) // 马从右方攻击
                    {
                        jieShaPoints.Add(new int[] { jiangJunQiZi_Col - 1, jiangJunQiZi_Row }); //  别马腿位置
                    }
                    if (blackJiang_Col - jiangJunQiZi_Col == 2) // 马从左方攻击
                    {
                        jieShaPoints.Add(new int[] { jiangJunQiZi_Col + 1, jiangJunQiZi_Row }); //  别马腿位置
                    }
                    break;

                case 7:
                case 8:     //  攻击棋子为黑方车(7,8)
                case 9:
                case 10:    //  攻击棋子为黑方炮(9,10)

                    if (jiangJunQiZi_Col == redShuai_Col) // 攻击方向为纵向
                    {
                        for (int row = Math.Min(jiangJunQiZi_Row, redShuai_Row) + 1; row < Math.Max(jiangJunQiZi_Row, redShuai_Row); row++)
                        {
                            jieShaPoints.Add(new int[] { jiangJunQiZi_Col, row });
                        }
                    }
                    if (jiangJunQiZi_Row == redShuai_Row) // 攻击方向为横向
                    {
                        for (int col = Math.Min(jiangJunQiZi_Col, redShuai_Col) + 1; col < Math.Max(jiangJunQiZi_Col, redShuai_Col); col++)
                        {
                            jieShaPoints.Add(new int[] { col, jiangJunQiZi_Row });
                        }
                    }
                    break;
                case 23:
                case 24:    //  攻击棋子为红方车(23,24)
                case 25:
                case 26:    //  攻击棋子为红方炮(25,26)
                    if (jiangJunQiZi_Col == blackJiang_Col) // 攻击方向为纵向
                    {
                        for (int row = Math.Min(jiangJunQiZi_Row, blackJiang_Row) + 1; row < Math.Max(jiangJunQiZi_Row, blackJiang_Row); row++)
                        {
                            jieShaPoints.Add(new int[] { jiangJunQiZi_Col, row });
                        }
                    }
                    if (jiangJunQiZi_Row == blackJiang_Row) // 攻击方向为横向
                    {
                        for (int col = Math.Min(jiangJunQiZi_Col, blackJiang_Col) + 1; col < Math.Max(jiangJunQiZi_Col, blackJiang_Col); col++)
                        {
                            jieShaPoints.Add(new int[] { col, jiangJunQiZi_Row });
                        }
                    }
                    break;
                default:
                    break;
            }
            #endregion

            bool[,] thispoints;
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 10; j++)
                {
                    int qizi = GlobalValue.QiPan[i, j]; // 从棋盘上找到存活的本方棋子
                    if (jiangJunQiZi > 15 && qizi > 0 && qizi <= 15) // 黑方被将军时
                    {
                        thispoints = MoveCheck.GetPathPoints(qizi, GlobalValue.QiPan); // 获得本方棋子的可移动路径
                        foreach (int[] point in jieShaPoints) // 逐个取出可解除将军的点位坐标
                        {
                            if (thispoints[point[0], point[1]] == true) // 本方棋子的可移动路径是否包含解除攻击点
                            {
                                if (!MoveCheck.AfterMoveStillJiangJun(qizi, point[0], point[1], GlobalValue.QiPan))
                                    return true;  // true=能够解杀
                            }
                        }
                    }
                    if (jiangJunQiZi <= 15 && qizi > 16 && qizi <= 31) // 红方被将军时
                    {
                        thispoints = MoveCheck.GetPathPoints(qizi, GlobalValue.QiPan); // 获得本方棋子的可移动路径
                        foreach (int[] point in jieShaPoints) // 逐个取出可解除将军的点位坐标
                        {
                            if (thispoints[point[0], point[1]] == true) // 本方棋子的可移动路径是否包含解除攻击点
                            {
                                if (!MoveCheck.AfterMoveStillJiangJun(qizi, point[0], point[1], GlobalValue.QiPan))
                                    return true;  // true=能够解杀
                            }
                        }
                    }
                }
            return false;  // false=不能解杀
        }


        public static bool IsBetween(int num, int start, int end)
        {
            if (num >= Math.Min(start, end) && num <= Math.Max(start, end)) return true;
            return false;
        }
        /// <summary>
        /// 检查是否困毙
        /// </summary>
        /// <param name="qizi">最后一个走动的棋子</param>
        /// <returns>被困毙时，返回true。</returns>
        public static bool IsKunBi(bool side)
        {
            if (side == GlobalValue.REDSIDE)
            {
                // 黑棋走完后，检查红方是否困毙
                for (int row = 0; row <= 6; row++)
                    for (int col = 0; col <= 8; col++)
                    {
                        // 红方兵行线以上有棋子时，可走闲，不会被困毙
                        if (GlobalValue.QiPan[col, row] is > 16 and < 32) return false;
                    }
                int[,] redpos = new int[16, 2]{ {0,9}, {0,8},{0,7}, {1,9}, {1,8}, {1,7}, {2,8}, {2,7},
                    {6,8}, {6,7}, {7,9}, {7,8}, {7,7}, {8,9}, {8,8}, {8,7}};
                for (int i = 0; i < 16; i++)
                {
                    // 上述位置如果有红方棋子，可走闲，也不会被困毙
                    if (GlobalValue.QiPan[redpos[i, 0], redpos[i, 1]] is > 16 and < 32) return false;
                }
                // 仅仅以下位置有红方棋子时，有可能造成困毙
                int[,] redKunBiPos = new int[11, 2] { { 2, 9 }, { 3, 9 }, { 3, 8 }, { 3, 7 }, { 4, 9 }, { 4, 8 }, { 4, 7 }, { 5, 9 }, { 5, 8 }, { 5, 7 }, { 6, 9 } };
                for (int i = 0; i < 11; i++)
                {
                    // 逐个验证上述位置的红方棋子，是否可移动
                    int redqizi = GlobalValue.QiPan[redKunBiPos[i, 0], redKunBiPos[i, 1]];
                    if (redqizi is > 16 and < 32)
                    {
                        bool[,] PathBool = MoveCheck.GetPathPoints(redqizi, GlobalValue.QiPan);
                        for (int col = 0; col <= 8; col++)
                        {
                            for (int row = 0; row <= 9; row++)
                            {
                                if (PathBool[col, row] == true)
                                {
                                    // 找到一个可移动点，如果棋子移动到该点后，不再被将军，则没有困毙
                                    if (!MoveCheck.AfterMoveStillJiangJun(redqizi, col, row, GlobalValue.QiPan)) return false;
                                }
                            }
                        }
                    }
                }
            }
            if (side == GlobalValue.BLACKSIDE)
            {
                // 红棋走完后，检查黑方是否困毙
                for (int row = 3; row <= 9; row++)
                    for (int col = 0; col <= 8; col++)
                    {
                        // 黑方兵行线以上有棋子时，可走闲，不会被困毙
                        if (GlobalValue.QiPan[col, row] is > 0 and < 16) return false;
                    }
                int[,] blackpos = new int[16, 2]{ {0,0}, {0,1},{0,2}, {1,0}, {1,1}, {1,2}, {2,1}, {2,2},
                    {6,1}, {6,2}, {7,0}, {7,1}, {7,2}, {8,0}, {8,1}, {8,2}};
                for (int i = 0; i < 16; i++)
                {
                    // 上述位置如果有黑方棋子，可走闲，也不会被困毙
                    if (GlobalValue.QiPan[blackpos[i, 0], blackpos[i, 1]] is > 0 and < 16) return false;
                }
                // 仅仅以下位置有黑方棋子时，有可能造成困毙
                int[,] blackKunBiPos = new int[11, 2] { { 2, 0 }, { 3, 0 }, { 3, 1 }, { 3, 2 }, { 4, 0 }, { 4, 1 }, { 4, 2 }, { 5, 0 }, { 5, 1 }, { 5, 2 }, { 6, 0 } };
                for (int i = 0; i < 11; i++)
                {
                    // 逐个验证上述位置的黑方棋子，是否可移动
                    int blackqizi = GlobalValue.QiPan[blackKunBiPos[i, 0], blackKunBiPos[i, 1]];
                    if (blackqizi is >= 0 and < 16)
                    {
                        bool[,] PathBool = MoveCheck.GetPathPoints(blackqizi, GlobalValue.QiPan);
                        for (int col = 0; col <= 8; col++)
                        {
                            for (int row = 0; row <= 9; row++)
                            {
                                if (PathBool[col, row] == true)
                                {
                                    // 找到一个可移动点，如果棋子移动到该点后，不再被将军，则没有困毙
                                    if (!MoveCheck.AfterMoveStillJiangJun(blackqizi, col, row, GlobalValue.QiPan)) return false;
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }
    }

}

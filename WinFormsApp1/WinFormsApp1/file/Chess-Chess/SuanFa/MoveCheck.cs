using System;

namespace Chess.SuanFa // 算法
{
    public static class MoveCheck  // 走棋规则检查
    {
        public static bool[,] PathBool = new bool[9, 10];

        /// <summary>
        /// 获取棋子可移动路径，并在棋盘上显示标记
        /// </summary>
        /// <param name="qiZi"></param>
        public static void GetAndShowPathPoints(int qiZi)

        {
            for (int i = 0; i <= 8; i++)
            {
                for (int j = 0; j <= 9; j++)
                {
                    GlobalValue.pathPointImage[i, j].HasPoint = false;
                    PathBool[i, j] = false;
                }
            }
            if (qiZi > -1 && MainWindow.menuItem != GlobalValue.CANJU_DESIGN)
            {
                PathBool = GetPathPoints(qiZi, GlobalValue.QiPan);
                for (int i = 0; i <= 8; i++)
                {
                    for (int j = 0; j <= 9; j++)
                    {
                        GlobalValue.pathPointImage[i, j].HasPoint = PathBool[i, j];
                    }
                }
                // 自身当前位置排除在可移动路径之外
                GlobalValue.pathPointImage[GlobalValue.qiZiArray[qiZi].Col, GlobalValue.qiZiArray[qiZi].Row].HasPoint = false;
            }
            
            // 残局设计时
            if (MainWindow.menuItem == GlobalValue.CANJU_DESIGN)
            {
                for (int i = 0; i <= 8; i++)
                {
                    for (int j = 0; j <= 9; j++)
                    {
                        GlobalValue.pathPointImage[i, j].HasPoint = true;
                        PathBool[i, j] = true;
                    }
                }
            }
        }

        /// <summary>
        /// 计算棋子可移动的目标位置
        /// </summary>
        /// <param name="moveQiZi">棋子编号</param>
        /// <param name="qiPan">当前棋盘数据</param>
        /// <returns>返回bool二维数组，对应棋盘上的每一点位</returns>
        public static bool[,] GetPathPoints(int moveQiZi, in int[,] qiPan)
        {
            bool[,] points = new bool[9, 10];
            for (int i = 0; i <= 8; i++)
            {
                for (int j = 0; j <= 9; j++)
                {
                    points[i, j] = false;
                }
            }
            if (moveQiZi > 31)  // 如果没有预选棋子
            {
                return points;
            }
            if (moveQiZi < 0)
            {
                return points;
            }
            if (GlobalValue.qiZiArray[moveQiZi].Visibility != System.Windows.Visibility.Visible) return points;
            int moveQiZiCol = GlobalValue.qiZiArray[moveQiZi].Col;
            int moveQiZiRow = GlobalValue.qiZiArray[moveQiZi].Row;
            int side = 0;
            #region 单个棋子可移动路径的计算
            switch (moveQiZi)
            {
                case 7:
                case 8:
                case 23:
                case 24:    // 车的移动  ================================================
                    if (!JustOneIsThis(moveQiZi, qiPan))
                    {
                        for (int i = moveQiZiCol - 1; i >= 0; i--) // 同一行向左找
                        {
                            if (qiPan[i, moveQiZiRow] == -1)
                            {
                                points[i, moveQiZiRow] = true;
                            }
                            else
                            {
                                if (!IsTongBang(moveQiZi, qiPan[i, moveQiZiRow]))
                                {
                                    points[i, moveQiZiRow] = true;
                                }
                                break;
                            }
                        }
                        for (int i = moveQiZiCol + 1; i <= 8; i++) // 同一行向右找
                        {
                            if (qiPan[i, moveQiZiRow] == -1)
                            {
                                points[i, moveQiZiRow] = true;
                            }
                            else
                            {
                                if (!IsTongBang(moveQiZi, qiPan[i, moveQiZiRow]))
                                {
                                    points[i, moveQiZiRow] = true;
                                }
                                break;
                            }
                        }
                    }
                    for (int i = moveQiZiRow - 1; i >= 0; i--) // 同一列向上找
                    {
                        if (qiPan[moveQiZiCol, i] == -1)
                        {
                            points[moveQiZiCol, i] = true;
                        }
                        else
                        {
                            if (!IsTongBang(moveQiZi, qiPan[moveQiZiCol, i]))
                            {
                                points[moveQiZiCol, i] = true;
                            }
                            break;
                        }
                    }
                    for (int i = moveQiZiRow + 1; i <= 9; i++) // 同一列向下找
                    {
                        if (qiPan[moveQiZiCol, i] == -1)
                        {
                            points[moveQiZiCol, i] = true;
                        }
                        else
                        {
                            if (!IsTongBang(moveQiZi, qiPan[moveQiZiCol, i]))
                            {
                                points[moveQiZiCol, i] = true;
                            }
                            break;
                        }
                    }
                    break;

                case 5:
                case 6:
                case 21:
                case 22:
                    // 马的移动 ================================================
                    if (JustOneIsThis(moveQiZi, qiPan))
                    {
                        break;
                    }
                    // 八个方向，逐个检查
                    if (moveQiZiRow > 1 && qiPan[moveQiZiCol, moveQiZiRow - 1] == -1) // 检查上方，如不在边上，且没蹩马腿
                    {
                        if (moveQiZiCol > 0 && !IsTongBang(moveQiZi, qiPan[moveQiZiCol - 1, moveQiZiRow - 2]))
                        {
                            points[moveQiZiCol - 1, moveQiZiRow - 2] = true;
                        }
                        if (moveQiZiCol < 8 && !IsTongBang(moveQiZi, qiPan[moveQiZiCol + 1, moveQiZiRow - 2]))
                        {
                            points[moveQiZiCol + 1, moveQiZiRow - 2] = true;
                        }
                    }

                    if (moveQiZiRow < 8 && qiPan[moveQiZiCol, moveQiZiRow + 1] == -1) // 检查下方，如不在边上，且没蹩马腿
                    {
                        if (moveQiZiCol > 0 && !IsTongBang(moveQiZi, qiPan[moveQiZiCol - 1, moveQiZiRow + 2]))
                        {
                            points[moveQiZiCol - 1, moveQiZiRow + 2] = true;
                        }
                        if (moveQiZiCol < 8 && !IsTongBang(moveQiZi, qiPan[moveQiZiCol + 1, moveQiZiRow + 2]))
                        {
                            points[moveQiZiCol + 1, moveQiZiRow + 2] = true;
                        }
                    }

                    if (moveQiZiCol > 1 && qiPan[moveQiZiCol - 1, moveQiZiRow] == -1) // 检查左方，如不在边上，且没蹩马腿
                    {
                        if (moveQiZiRow > 0 && !IsTongBang(moveQiZi, qiPan[moveQiZiCol - 2, moveQiZiRow - 1]))
                        {
                            points[moveQiZiCol - 2, moveQiZiRow - 1] = true;
                        }
                        if (moveQiZiRow < 9 && !IsTongBang(moveQiZi, qiPan[moveQiZiCol - 2, moveQiZiRow + 1]))
                        {
                            points[moveQiZiCol - 2, moveQiZiRow + 1] = true;
                        }
                    }

                    if (moveQiZiCol < 7 && qiPan[moveQiZiCol + 1, moveQiZiRow] == -1) // 检查右方，如不在边上，且没蹩马腿
                    {
                        if (moveQiZiRow > 0 && !IsTongBang(moveQiZi, qiPan[moveQiZiCol + 2, moveQiZiRow - 1]))
                        {
                            points[moveQiZiCol + 2, moveQiZiRow - 1] = true;
                        }
                        if (moveQiZiRow < 9 && !IsTongBang(moveQiZi, qiPan[moveQiZiCol + 2, moveQiZiRow + 1]))
                        {
                            points[moveQiZiCol + 2, moveQiZiRow + 1] = true;
                        }
                    }
                    break;

                case 3:
                case 4:
                case 19:
                case 20: // 相的移动 ================================================
                    if (moveQiZiRow <= 4) // 如果是上方相，则上下边界设定为0--4，下方相的边界设定为5--9
                    {
                        side = 5;
                    }
                    if (JustOneIsThis(moveQiZi, qiPan))
                    {
                        break;
                    }
                    if (moveQiZiRow != 9 - side)  // 如果不在下边界上，则探查下方的可移动路径
                    {
                        if (moveQiZiCol > 0)
                        {
                            if (qiPan[moveQiZiCol - 1, moveQiZiRow + 1] == -1 && !IsTongBang(moveQiZi, qiPan[moveQiZiCol - 2, moveQiZiRow + 2])) // 左下方
                            {
                                points[moveQiZiCol - 2, moveQiZiRow + 2] = true;
                            }
                        }
                        if (moveQiZiCol < 8)
                        {
                            if (qiPan[moveQiZiCol + 1, moveQiZiRow + 1] == -1 && !IsTongBang(moveQiZi, qiPan[moveQiZiCol + 2, moveQiZiRow + 2])) // 右下方
                            {
                                points[moveQiZiCol + 2, moveQiZiRow + 2] = true;
                            }
                        }
                    }
                    if (moveQiZiRow != 5 - side)  // 如果不在上边界上，则探查上方的可移动路径
                    {
                        if (moveQiZiCol > 0)
                        {
                            if (qiPan[moveQiZiCol - 1, moveQiZiRow - 1] == -1 && !IsTongBang(moveQiZi, qiPan[moveQiZiCol - 2, moveQiZiRow - 2])) // 左上方
                            {
                                points[moveQiZiCol - 2, moveQiZiRow - 2] = true;
                            }
                        }
                        if (moveQiZiCol < 8)
                        {
                            if (qiPan[moveQiZiCol + 1, moveQiZiRow - 1] == -1 && !IsTongBang(moveQiZi, qiPan[moveQiZiCol + 2, moveQiZiRow - 2])) // 右上方
                            {
                                points[moveQiZiCol + 2, moveQiZiRow - 2] = true;
                            }
                        }
                    }
                    break;
                case 1:
                case 2:
                case 17:
                case 18: // 士的移动 ================================================
                    side = 0;
                    if (moveQiZiRow <= 4) // 如果是上方棋子，则上下边界设定为0--2，下方相的边界设定为7--9
                    {
                        side = 7;
                    }
                    if (JustOneIsThis(moveQiZi, qiPan))
                    {
                        break;
                    }
                    if (moveQiZiRow != 9 - side)  // 如果不在下边界上，则探查下方的可移动路径
                    {
                        if (moveQiZiCol > 3)
                        {
                            if (!IsTongBang(moveQiZi, qiPan[moveQiZiCol - 1, moveQiZiRow + 1])) // 左下方
                            {
                                points[moveQiZiCol - 1, moveQiZiRow + 1] = true;
                            }
                        }
                        if (moveQiZiCol < 5)
                        {
                            if (!IsTongBang(moveQiZi, qiPan[moveQiZiCol + 1, moveQiZiRow + 1])) // 右下方
                            {
                                points[moveQiZiCol + 1, moveQiZiRow + 1] = true;
                            }
                        }
                    }
                    if (moveQiZiRow != 7 - side)  // 如果不在上边界上，则探查上方的可移动路径
                    {
                        if (moveQiZiCol > 3)
                        {
                            if (!IsTongBang(moveQiZi, qiPan[moveQiZiCol - 1, moveQiZiRow - 1]))// 左上方
                            {
                                points[moveQiZiCol - 1, moveQiZiRow - 1] = true;
                            }
                        }
                        if (moveQiZiCol < 5)
                        {
                            if (!IsTongBang(moveQiZi, qiPan[moveQiZiCol + 1, moveQiZiRow - 1])) // 右上方
                            {
                                points[moveQiZiCol + 1, moveQiZiRow - 1] = true;
                            }
                        }
                    }
                    break;
                case 0:
                case 16: // 将帅的移动 ================================================
                    side = 0;
                    if (moveQiZiRow <= 4) // 黑将的上下移动边界为0--2，红帅的上下移动边界为7--9
                    {
                        side = 7;
                    }
                    if (moveQiZiRow != (9 - side))  // 如果不在下边界上，则探查下方的可移动路径
                    {
                        if (!IsTongBang(moveQiZi, qiPan[moveQiZiCol, moveQiZiRow + 1])) // 下方移一格
                        {
                            if (GlobalValue.qiZiArray[0].Col != GlobalValue.qiZiArray[16].Col)    // 如果将帅横向不同线
                            {
                                points[moveQiZiCol, moveQiZiRow + 1] = true;
                            }
                            else
                            {
                                if (moveQiZi == 0)
                                {
                                    for (int i = GlobalValue.qiZiArray[0].Row + 2; i < GlobalValue.qiZiArray[16].Row; i++)
                                    {
                                        if (qiPan[moveQiZiCol, i] != -1)
                                        {
                                            points[moveQiZiCol, moveQiZiRow + 1] = true;
                                            break;
                                        }
                                    }
                                }
                                if (moveQiZi == 16)
                                {
                                    points[moveQiZiCol, moveQiZiRow + 1] = true;
                                }
                            }
                        }
                    }
                    if (moveQiZiRow != (7 - side))  // 如果不在上边界上，则探查上方的可移动路径
                    {
                        if (!IsTongBang(moveQiZi, qiPan[moveQiZiCol, moveQiZiRow - 1]))// 上方移一格
                        {
                            if (GlobalValue.qiZiArray[0].Col != GlobalValue.qiZiArray[16].Col)    // 如果将帅横向不同线
                            {
                                points[moveQiZiCol, moveQiZiRow - 1] = true;
                            }
                            else
                            {
                                if (moveQiZi == 0)
                                {
                                    points[moveQiZiCol, moveQiZiRow - 1] = true;
                                }
                                if (moveQiZi == 16)
                                {
                                    for (int i = GlobalValue.qiZiArray[0].Row + 1; i < GlobalValue.qiZiArray[16].Row - 1; i++)
                                    {
                                        if (qiPan[moveQiZiCol, i] != -1)
                                        {
                                            points[moveQiZiCol, moveQiZiRow - 1] = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (moveQiZiCol > 3)  // 如果不在左边界上，则探查左方的可移动路径
                    {
                        if (!IsTongBang(moveQiZi, qiPan[moveQiZiCol - 1, moveQiZiRow])) // 左方移一格
                        {
                            if (((moveQiZiCol - 1) == GlobalValue.qiZiArray[0].Col) || ((moveQiZiCol - 1) == GlobalValue.qiZiArray[16].Col))    // 如果将帅横向移动一格
                            {
                                for (int i = GlobalValue.qiZiArray[0].Row + 1; i < GlobalValue.qiZiArray[16].Row; i++)
                                {
                                    if (qiPan[moveQiZiCol - 1, i] != -1)
                                    {
                                        points[moveQiZiCol - 1, moveQiZiRow] = true;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                points[moveQiZiCol - 1, moveQiZiRow] = true;
                            }
                        }

                    }
                    if (moveQiZiCol < 5)  // 如果不在右边界上，则探查右方的可移动路径
                    {
                        if (!IsTongBang(moveQiZi, qiPan[moveQiZiCol + 1, moveQiZiRow])) // 右方移一格
                        {
                            if (((moveQiZiCol + 1) == GlobalValue.qiZiArray[0].Col) || ((moveQiZiCol + 1) == GlobalValue.qiZiArray[16].Col))    // 如果将帅横向移动一格
                            {
                                for (int i = GlobalValue.qiZiArray[0].Row + 1; i < GlobalValue.qiZiArray[16].Row; i++)
                                {
                                    if (qiPan[moveQiZiCol + 1, i] != -1)
                                    {
                                        points[moveQiZiCol + 1, moveQiZiRow] = true;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                points[moveQiZiCol + 1, moveQiZiRow] = true;
                            }
                        }
                    }
                    for (int i = 0; i <= 8; i++)
                    {
                        for (int j = 0; j <= 9; j++)
                        {
                            if (points[i, j] == true && IsKilledPoint(moveQiZi, i, j, GlobalValue.QiPan) == true)
                            {
                                points[i, j] = false;
                            }
                        }
                    }


                    break;
                case 9:
                case 10:
                case 25:
                case 26: // 炮的移动 ================================================
                    int gezi = 0; // 隔子计数
                    if (!JustOneIsThis(moveQiZi, qiPan))
                    {
                        for (int i = moveQiZiCol - 1; i >= 0; i--) // 同一行向左找
                        {
                            if (qiPan[i, moveQiZiRow] == -1)
                            {
                                if (gezi == 0)
                                {
                                    points[i, moveQiZiRow] = true;
                                }
                            }
                            else
                            {
                                if (!IsTongBang(moveQiZi, qiPan[i, moveQiZiRow]))
                                {
                                    if (gezi == 1)
                                    {
                                        points[i, moveQiZiRow] = true;
                                    }
                                }
                                gezi++;
                            }
                        }
                        gezi = 0; // 隔子计数
                        for (int i = moveQiZiCol + 1; i <= 8; i++) // 同一行向右找
                        {
                            if (qiPan[i, moveQiZiRow] == -1)
                            {
                                if (gezi == 0)
                                {
                                    points[i, moveQiZiRow] = true;
                                }
                            }
                            else
                            {
                                if (!IsTongBang(moveQiZi, qiPan[i, moveQiZiRow]))
                                {
                                    if (gezi == 1)
                                    {
                                        points[i, moveQiZiRow] = true;
                                    }
                                }
                                gezi++;
                            }
                        }
                    }
                    gezi = 0; // 隔子计数
                    for (int i = moveQiZiRow - 1; i >= 0; i--) // 同一列向上找
                    {
                        if (qiPan[moveQiZiCol, i] == -1)
                        {
                            if (gezi == 0)
                            {
                                points[moveQiZiCol, i] = true;
                            }
                        }
                        else
                        {
                            if (!IsTongBang(moveQiZi, qiPan[moveQiZiCol, i]))
                            {
                                if (gezi == 1)
                                {
                                    points[moveQiZiCol, i] = true;
                                }
                            }
                            gezi++;
                        }
                    }
                    gezi = 0; // 隔子计数
                    for (int i = moveQiZiRow + 1; i <= 9; i++) // 同一列向下找
                    {
                        if (qiPan[moveQiZiCol, i] == -1)
                        {
                            if (gezi == 0)
                            {
                                points[moveQiZiCol, i] = true;
                            }
                        }
                        else
                        {
                            if (!IsTongBang(moveQiZi, qiPan[moveQiZiCol, i]))
                            {
                                if (gezi == 1)
                                {
                                    points[moveQiZiCol, i] = true;
                                }
                            }
                            gezi++;
                        }
                    }
                    break;
                case 11:
                case 12:
                case 13:
                case 14:
                case 15: // 黑方卒的移动
                    if (moveQiZiRow < 9 && !IsTongBang(moveQiZi, qiPan[moveQiZiCol, moveQiZiRow + 1])) // 下方一格
                    {
                        points[moveQiZiCol, moveQiZiRow + 1] = true;
                    }
                    if (!JustOneIsThis(moveQiZi, qiPan) && moveQiZiRow <= 9 && moveQiZiRow > 4) // 水平一格
                    {
                        if (moveQiZiCol > 0 && !IsTongBang(moveQiZi, qiPan[moveQiZiCol - 1, moveQiZiRow]))
                        {
                            points[moveQiZiCol - 1, moveQiZiRow] = true;
                        }

                        if (moveQiZiCol < 8 && !IsTongBang(moveQiZi, qiPan[moveQiZiCol + 1, moveQiZiRow]))
                        {
                            points[moveQiZiCol + 1, moveQiZiRow] = true;
                        }
                    }
                    break;
                case 27:
                case 28:
                case 29:
                case 30:
                case 31:// 红方兵的移动
                    if (moveQiZiRow > 0 && !IsTongBang(moveQiZi, qiPan[moveQiZiCol, moveQiZiRow - 1])) // 上方一格
                    {
                        points[moveQiZiCol, moveQiZiRow - 1] = true;
                    }
                    if (!JustOneIsThis(moveQiZi, qiPan) && moveQiZiRow >= 0 && moveQiZiRow < 5) // 水平一格
                    {
                        if (moveQiZiCol > 0 && !IsTongBang(moveQiZi, qiPan[moveQiZiCol - 1, moveQiZiRow]))
                        {
                            points[moveQiZiCol - 1, moveQiZiRow] = true;
                        }

                        if (moveQiZiCol < 8 && !IsTongBang(moveQiZi, qiPan[moveQiZiCol + 1, moveQiZiRow]))
                        {
                            points[moveQiZiCol + 1, moveQiZiRow] = true;
                        }
                    }
                    break;
                default:
                    return points;
            }
            #endregion
            return points;
        }

        // 判断是不是同一方棋子
        public static bool IsTongBang(int qz1, int qz2)
        {
            return qz1 >= 0 && qz2 >= 0 && ((qz1 <= 15 && qz2 <= 15) || (qz1 >= 16 && qz2 >= 16));
        }

        /// <summary>
        /// 将帅在同一列时，检查本棋子是否是将帅之间的唯一棋子。
        /// </summary>
        /// <param name="qiZi"></param>
        /// <returns>将帅同列时，如果本棋子是他们之间的唯一棋子，则返回ture。</returns>
        private static bool JustOneIsThis(int qiZi, in int[,] qiPan)
        {
            if (GlobalValue.qiZiArray[0].Col != GlobalValue.qiZiArray[16].Col)    // 如果将帅不在同一列
            {
                return false;
            }
            else
            {
                if (GlobalValue.qiZiArray[qiZi].Col != GlobalValue.qiZiArray[0].Col)    // 如果棋子不与将帅同列
                {
                    return false;
                }
                else
                {
                    if (GlobalValue.qiZiArray[qiZi].Row < GlobalValue.qiZiArray[0].Row)    // 如果在黑将上方
                    {
                        return false;
                    }
                    if (GlobalValue.qiZiArray[qiZi].Row > GlobalValue.qiZiArray[16].Row)    // 如果在红帅下方
                    {
                        return false;
                    }
                }
            }
            int count = 0;
            for (int i = GlobalValue.qiZiArray[0].Row + 1; i < GlobalValue.qiZiArray[16].Row; i++)
            {
                if (qiPan[GlobalValue.qiZiArray[0].Col, i] != -1)
                {
                    count++;
                }
            }
            return count == 1;
        }

        /// <summary>
        /// 在将帅的可移动路径中，排除对方车、马、炮、卒的攻击点。
        /// </summary>
        /// <param name="jiangOrShuai">0=将，16=帅</param>
        /// <param name="col">可移动点的列位置</param>
        /// <param name="row">可移动点的行位置</param>
        /// <param name="qiPan">当前棋盘数据</param>
        /// <returns></returns>
        public static bool IsKilledPoint(int jiangOrShuai, int col, int row, in int[,] qiPan)
        {
            // 注意：数组作为参数传递时，不是传递的副本，而是直接数组本身。
            int[,] myQiPan = new int[9, 10]; // 制作棋盘副本，防止破坏原棋盘数据数组。
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 10; j++)
                {
                    myQiPan[i, j] = qiPan[i, j];
                }
            myQiPan[col, row] = jiangOrShuai;
            myQiPan[GlobalValue.qiZiArray[jiangOrShuai].Col, GlobalValue.qiZiArray[jiangOrShuai].Row] = -1;

            bool[,] thisPoints;
            if (jiangOrShuai == 16)
            {
                for (int qizi = 5; qizi <= 15; qizi++) //黑方：车(7,8)，马(5,6)，炮(9,10)，卒(11,12,13,14,15)
                {
                    thisPoints = GetPathPoints(qizi, myQiPan);
                    if (thisPoints[col, row]) return true;
                }
            }
            if (jiangOrShuai == 0)
            {
                for (int qizi = 21; qizi <= 31; qizi++) //红方：车(23,24)，马(21,22)，炮(25,26)，兵(27,28,29,30,31)
                {
                    thisPoints = GetPathPoints(qizi, myQiPan);
                    if (thisPoints[col, row]) return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 棋子移动到指定位置后，是否还是被将军
        /// </summary>
        /// <param name="qiZi">棋子编号</param>
        /// <param name="x1">将要移动的列位置</param>
        /// <param name="y1">将要移动的行位置</param>
        /// <param name="qiPan">当前棋盘数据</param>
        /// <returns></returns>
        public static bool AfterMoveStillJiangJun(int qiZi, int x1, int y1, int[,] qiPan)
        {
            int x0 = GlobalValue.qiZiArray[qiZi].Col;
            int y0 = GlobalValue.qiZiArray[qiZi].Row;
            return AfterMoveStillJiangJun(qiZi, x0, y0, x1, y1, qiPan);
        }

        /// <summary>
        /// 检查棋子移动后，本方是否被将军。如果移动后被将军，则通过相关机制不允许该棋子移动到目标位置。
        /// 用于棋子移动前的检测。
        /// </summary>
        /// <param name="qiZi">棋子编号</param>
        /// <param name="x0"></param>
        /// <param name="y0"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="qiPan">棋盘数据</param>
        /// <returns> false=未将军，true=被将军 </returns>
        public static bool AfterMoveStillJiangJun(int qiZi, int x0, int y0, int x1, int y1, in int[,] qiPan)
        {
            // 注意：数组作为参数传递时，不是传递参数的副本，而是传递数组本身的地址，是传址而非传参。所以不要直接修改。
            int[,] myQiPan = new int[9, 10]; // 制作棋盘副本，防止破坏原棋盘数据数组。
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 10; j++)
                {
                    myQiPan[i, j] = qiPan[i, j];
                }
            myQiPan[x1, y1] = qiZi;
            myQiPan[x0, y0] = -1;

            bool[,] thisPoints;
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 10; j++)
                {
                    int localQiZi = myQiPan[i, j]; // 从棋盘副本上找棋子
                    if (qiZi > 15)
                    {
                        if (localQiZi is >= 5 and <= 15) //车(7,8)，马(5,6)，炮(9,10)，卒(11,12,13,14,15)
                        {
                            thisPoints = GetPathPoints(localQiZi, myQiPan);
                            int x = (qiZi == 16) ? x1 : GlobalValue.qiZiArray[16].Col;
                            int y = (qiZi == 16) ? y1 : GlobalValue.qiZiArray[16].Row;
                            if (thisPoints[x, y] == true) return true;
                        }
                    }
                    if (qiZi < 15)
                    {
                        if (localQiZi is >= 21 and <= 31) //车(23,24)，马(21,22)，炮(25,26)，卒(27,28,29,30,31)
                        {
                            thisPoints = GetPathPoints(localQiZi, myQiPan);
                            int x = (qiZi == 0) ? x1 : GlobalValue.qiZiArray[0].Col;
                            int y = (qiZi == 0) ? y1 : GlobalValue.qiZiArray[0].Row;
                            if (thisPoints[x, y] == true) return true;
                        }
                    }
                }
            return false; // false=未被将军
        }
        /// <summary>
        /// 残局等棋局设计在自由摆放棋子时，也要对棋子位置是否符合规则进行检查，例如：将帅、象相、仕士、兵卒等，不能超出了活动范围。
        /// </summary>
        /// <param name="qizi"></param>
        /// <param name="col"></param>
        /// <param name="row"></param>
        /// <returns>true=棋子位置合规，false=棋子位置不合规</returns>
        public static bool FreeMoveCheck(int qizi, int col, int row)
        {
            #region 将帅、象相、仕士、兵卒等的摆放位置，不能超出各自的活动范围
            switch (qizi)
            {
                case 3:
                case 4: // 象 ================================================
                    int[,] pos = new int[7, 2] { { 2, 0 }, { 6, 0 }, { 0, 2 }, { 4, 2 }, { 8, 2 }, { 2, 4 }, { 6, 4 } };
                    for (int i = 0; i < 7; i++)
                    {
                        if (col == pos[i, 0] && row == pos[i, 1]) return true;
                    }
                    break;
                case 19:
                case 20: // 相 ================================================
                    int[,] pos19 = new int[7, 2] { { 2, 0 }, { 6, 0 }, { 0, 2 }, { 4, 2 }, { 8, 2 }, { 2, 4 }, { 6, 4 } };
                    for (int i = 0; i < 7; i++)
                    {
                        if (col == pos19[i, 0] && row == pos19[i, 1] + 5) return true;
                    }
                    break;
                case 1:
                case 2:// 士 ================================================
                    int[,] pos1 = new int[5, 2] { { 3, 0 }, { 5, 0 }, { 4, 1 }, { 3, 2 }, { 5, 2 } };
                    for (int i = 0; i < 5; i++)
                    {
                        if (col == pos1[i, 0] && row == pos1[i, 1]) return true;
                    }

                    break;
                case 17:
                case 18: // 仕 ================================================
                    int[,] pos17 = new int[5, 2] { { 3, 0 }, { 5, 0 }, { 4, 1 }, { 3, 2 }, { 5, 2 } };
                    for (int i = 0; i < 5; i++)
                    {
                        if (col == pos17[i, 0] && row == pos17[i, 1] + 7) return true;
                    }
                    break;
                case 0: // 将 ===============================================
                    if (col >= 3 && col <= 5 && row >= 0 && row <= 2) return true;
                    break;
                case 16: // 帅 ================================================
                    if (col >= 3 && col <= 5 && row >= 7 && row <= 9) return true;
                    break;

                case 11:
                case 12:
                case 13:
                case 14:
                case 15: // 黑方卒
                    if (row < 3) return false;
                    if (row > 4) return true;
                    if (row is 3 or 4)
                    {
                        if (col is 0 or 2 or 4 or 6 or 8) return true;
                    }
                    break;
                case 27:
                case 28:
                case 29:
                case 30:
                case 31:// 红方兵
                    if (row > 6) return false;
                    if (row < 5) return true;
                    if (row is 5 or 6)
                    {
                        if (col is 0 or 2 or 4 or 6 or 8) return true;
                    }
                    break;
                default:
                    return true;
            }
            #endregion
            return false;
        }
    }
}

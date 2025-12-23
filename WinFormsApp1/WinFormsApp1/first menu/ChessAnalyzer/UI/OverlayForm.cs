using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WinFormsApp1.first_menu.ChessAnalyzer.Core;

namespace WinFormsApp1.first_menu.ChessAnalyzer.UI
{
    /// <summary>
    /// 悬浮窗口 - 显示棋盘和推荐走法
    /// </summary>
    public class OverlayForm : Form
    {
        private ChessBoard _currentBoard;
        private string _bestMove;
        private string _ponderMove;
        private int _score;
        private int _depth;

        // UI 控件
        private Panel _titlePanel;
        private Label _titleLabel;
        private Button _closeButton;
        private Button _minimizeButton;
        private Panel _boardPanel;
        private Label _turnLabel;
        private Label _scoreLabel;
        private Label _depthLabel;
        private Label _bestMoveLabel;
        private Label _ponderMoveLabel;

        // 棋子图片控件
        private PictureBox[,] _pieceBoxes;

        // 拖动相关
        private bool _isDragging = false;
        private Point _dragStartPoint;

        public OverlayForm()
        {
            InitializeWindow();
            InitializeUI();
        }

        private void InitializeWindow()
        {
            // 窗口属性（调整宽度和高度以容纳所有控件）
            this.Text = "象棋分析助手";
            this.Size = new Size(330, 485);  // 宽度330，高度485（容纳底部标签+边距）
            this.FormBorderStyle = FormBorderStyle.None;
            this.TopMost = true;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.Manual;
            this.BackColor = Color.FromArgb(240, 242, 245);
            this.Opacity = 0.95;

            // 圆角和阴影效果
            this.Region = CreateRoundedRegion(this.ClientRectangle, 10);
        }

        private void InitializeUI()
        {
            // 标题栏
            _titlePanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 35,
                BackColor = Color.FromArgb(48, 63, 159)
            };
            _titlePanel.MouseDown += TitlePanel_MouseDown;
            _titlePanel.MouseMove += TitlePanel_MouseMove;
            _titlePanel.MouseUp += TitlePanel_MouseUp;

            _titleLabel = new Label
            {
                Text = "♟ 象棋分析",
                Location = new Point(10, 8),
                Size = new Size(150, 20),
                Font = new Font("Microsoft YaHei UI", 10, FontStyle.Bold),
                ForeColor = Color.White
            };
            _titlePanel.Controls.Add(_titleLabel);

            _closeButton = new Button
            {
                Text = "✕",
                Location = new Point(290, 5),  // 调整位置
                Size = new Size(30, 25),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Transparent,
                ForeColor = Color.White,
                Font = new Font("Arial", 12, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            _closeButton.FlatAppearance.BorderSize = 0;
            _closeButton.Click += (s, e) => this.Hide();
            _titlePanel.Controls.Add(_closeButton);

            _minimizeButton = new Button
            {
                Text = "−",
                Location = new Point(255, 5),  // 调整位置
                Size = new Size(30, 25),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Transparent,
                ForeColor = Color.White,
                Font = new Font("Arial", 12, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            _minimizeButton.FlatAppearance.BorderSize = 0;
            _minimizeButton.Click += (s, e) => this.WindowState = FormWindowState.Minimized;
            _titlePanel.Controls.Add(_minimizeButton);

            this.Controls.Add(_titlePanel);

            // 信息面板
            Panel infoPanel = new Panel
            {
                Location = new Point(10, 45),
                Size = new Size(310, 60),  // 调整宽度
                BackColor = Color.White
            };

            _turnLabel = new Label
            {
                Text = "红方走棋",
                Location = new Point(10, 8),
                Size = new Size(100, 20),
                Font = new Font("Microsoft YaHei UI", 9, FontStyle.Bold),
                ForeColor = Color.Red
            };
            infoPanel.Controls.Add(_turnLabel);

            _scoreLabel = new Label
            {
                Text = "评分: +0.00",
                Location = new Point(120, 8),
                Size = new Size(170, 20),
                Font = new Font("Microsoft YaHei UI", 9),
                ForeColor = Color.DarkGreen
            };
            infoPanel.Controls.Add(_scoreLabel);

            _depthLabel = new Label
            {
                Text = "深度: 0",
                Location = new Point(10, 32),
                Size = new Size(100, 20),
                Font = new Font("Microsoft YaHei UI", 8),
                ForeColor = Color.Gray
            };
            infoPanel.Controls.Add(_depthLabel);

            this.Controls.Add(infoPanel);

            // 棋盘面板（增加尺寸以容纳边缘棋子）
            _boardPanel = new Panel
            {
                Location = new Point(10, 115),
                Size = new Size(310, 285),  // 增加宽度和高度，容纳边缘棋子
                BackColor = Color.FromArgb(245, 222, 179)
            };
            _boardPanel.Paint += BoardPanel_Paint;

            // 创建棋子显示控件
            _pieceBoxes = new PictureBox[9, 10];
            int offsetX = 19;  // 左边偏移量（足够容纳半个棋子 14px + 边距 5px）
            int offsetY = 18;  // 上边偏移量（足够容纳半个棋子 13px + 边距 5px）
            int cellWidth = 33;   // 格子宽度
            int cellHeight = 27;  // 格子高度
            
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    // 关键修正：Location 是左上角，需要减去半径让圆心在交叉点上
                    int intersectionX = offsetX + x * cellWidth;   // 交叉点 X
                    int intersectionY = offsetY + y * cellHeight;  // 交叉点 Y
                    int pieceWidth = 28;
                    int pieceHeight = 25;
                    
                    _pieceBoxes[x, y] = new PictureBox
                    {
                        Size = new Size(pieceWidth, pieceHeight),
                        // 左上角 = 交叉点 - 半径，让圆心对准交叉点
                        Location = new Point(intersectionX - pieceWidth / 2, intersectionY - pieceHeight / 2),
                        SizeMode = PictureBoxSizeMode.CenterImage,
                        BackColor = Color.Transparent
                    };
                    _boardPanel.Controls.Add(_pieceBoxes[x, y]);
                }
            }

            this.Controls.Add(_boardPanel);

            // 推荐走法标签
            _bestMoveLabel = new Label
            {
                Text = "推荐: 等待分析...",
                Location = new Point(10, 408),  // 棋盘底部 + 8px 边距
                Size = new Size(310, 28),
                Font = new Font("Microsoft YaHei UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(46, 125, 50),
                BackColor = Color.FromArgb(232, 245, 233),
                TextAlign = ContentAlignment.MiddleCenter
            };
            this.Controls.Add(_bestMoveLabel);

            _ponderMoveLabel = new Label
            {
                Text = "应对: 等待分析...",
                Location = new Point(10, 443),  // 推荐标签底部 + 7px 边距
                Size = new Size(310, 28),
                Font = new Font("Microsoft YaHei UI", 9),
                ForeColor = Color.FromArgb(25, 118, 210),
                BackColor = Color.FromArgb(227, 242, 253),
                TextAlign = ContentAlignment.MiddleCenter
            };
            this.Controls.Add(_ponderMoveLabel);
        }

        /// <summary>
        /// 绘制棋盘
        /// </summary>
        private void BoardPanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Pen pen = new Pen(Color.Black, 1);

            // 使用与棋子相同的偏移量和格子尺寸
            int offsetX = 19;
            int offsetY = 18;
            int cellWidth = 33;
            int cellHeight = 27;

            // 画横线（从第0列到第8列）
            int leftX = offsetX;
            int rightX = offsetX + 8 * cellWidth;
            for (int i = 0; i < 10; i++)
            {
                int y = offsetY + i * cellHeight;
                g.DrawLine(pen, leftX, y, rightX, y);
            }

            // 画竖线
            for (int i = 0; i < 9; i++)
            {
                int x = offsetX + i * cellWidth;
                // 上半部分（0-4行）
                g.DrawLine(pen, x, offsetY, x, offsetY + 4 * cellHeight);
                // 下半部分（5-9行）
                g.DrawLine(pen, x, offsetY + 5 * cellHeight, x, offsetY + 9 * cellHeight);
            }

            // 画楚河汉界
            using (Font font = new Font("KaiTi", 10, FontStyle.Bold))
            using (SolidBrush brush = new SolidBrush(Color.Black))
            {
                int riverY = offsetY + 4 * cellHeight + 5;
                g.DrawString("楚 河", font, brush, offsetX + 50, riverY);
                g.DrawString("汉 界", font, brush, offsetX + 165, riverY);
            }

            // 画九宫格斜线
            // 上方九宫格
            int palace3X = offsetX + 3 * cellWidth;
            int palace5X = offsetX + 5 * cellWidth;
            int palace0Y = offsetY;
            int palace2Y = offsetY + 2 * cellHeight;
            g.DrawLine(pen, palace3X, palace0Y, palace5X, palace2Y);  // 左上→右下
            g.DrawLine(pen, palace5X, palace0Y, palace3X, palace2Y);  // 右上→左下
            
            // 下方九宫格
            int palace7Y = offsetY + 7 * cellHeight;
            int palace9Y = offsetY + 9 * cellHeight;
            g.DrawLine(pen, palace3X, palace7Y, palace5X, palace9Y);  // 左上→右下
            g.DrawLine(pen, palace5X, palace7Y, palace3X, palace9Y);  // 右上→左下
        }

        /// <summary>
        /// 更新棋盘显示
        /// </summary>
        public void UpdateBoard(ChessBoard board, string bestMove, string ponderMove, int score, int depth)
        {
            // 自动转换中文
            string bestChinese = !string.IsNullOrEmpty(bestMove) 
                ? MoveConverter.ToChineseNotation(bestMove, board) 
                : "";
            string ponderChinese = !string.IsNullOrEmpty(ponderMove) 
                ? MoveConverter.ToChineseNotation(ponderMove, board) 
                : "";
                
            UpdateBoard(board, bestMove, ponderMove, score, depth, bestChinese, ponderChinese);
        }

        /// <summary>
        /// 更新棋盘显示（带中文走法）
        /// </summary>
        public void UpdateBoard(ChessBoard board, string bestMove, string ponderMove, int score, int depth, 
            string bestMoveChinese, string ponderMoveChinese)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => UpdateBoard(board, bestMove, ponderMove, score, depth, 
                    bestMoveChinese, ponderMoveChinese)));
                return;
            }

            _currentBoard = board;
            _bestMove = bestMove;
            _ponderMove = ponderMove;
            _score = score;
            _depth = depth;

            // 更新棋子
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    var piece = board.GetPiece(x, y);
                    if (piece != ChessPiece.Empty)
                    {
                        _pieceBoxes[x, y].Image = CreatePieceImage(piece);
                    }
                    else
                    {
                        _pieceBoxes[x, y].Image = null;
                    }
                }
            }

            // 更新标签
            _turnLabel.Text = board.IsRedTurn ? "红方走棋" : "黑方走棋";
            _turnLabel.ForeColor = board.IsRedTurn ? Color.Red : Color.Black;

            _scoreLabel.Text = $"评分: {(score / 100.0):+0.00;-0.00;+0.00}";
            _scoreLabel.ForeColor = score >= 0 ? Color.DarkGreen : Color.DarkRed;

            _depthLabel.Text = $"深度: {depth}";

            // 更新走法显示（使用传入的中文）
            if (!string.IsNullOrEmpty(bestMove))
            {
                _bestMoveLabel.Text = $"✓ {bestMoveChinese}";
                _bestMoveLabel.ForeColor = Color.Green;
                HighlightMove(bestMove, Color.Green);
            }
            else
            {
                _bestMoveLabel.Text = "推荐: 等待分析...";
                _bestMoveLabel.ForeColor = Color.Gray;
            }

            if (!string.IsNullOrEmpty(ponderMove))
            {
                _ponderMoveLabel.Text = $"→ {ponderMoveChinese}";
                _ponderMoveLabel.ForeColor = Color.Blue;
            }
            else
            {
                _ponderMoveLabel.Text = "应对: 等待分析...";
                _ponderMoveLabel.ForeColor = Color.Gray;
            }
        }

        /// <summary>
        /// 创建棋子图片
        /// </summary>
        private Image CreatePieceImage(ChessPiece piece)
        {
            Bitmap bmp = new Bitmap(28, 25);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;

                bool isRed = ChessPieceHelper.IsRedPiece(piece);
                Color bgColor = isRed ? Color.FromArgb(255, 230, 230) : Color.FromArgb(230, 230, 230);
                Color borderColor = isRed ? Color.Red : Color.Black;
                Color textColor = isRed ? Color.Red : Color.Black;

                // 绘制圆形背景
                using (SolidBrush brush = new SolidBrush(bgColor))
                {
                    g.FillEllipse(brush, 2, 2, 24, 21);
                }

                // 绘制边框
                using (Pen pen = new Pen(borderColor, 1.5f))
                {
                    g.DrawEllipse(pen, 2, 2, 24, 21);
                }

                // 绘制文字
                string text = ChessPieceHelper.GetPieceText(piece);
                using (Font font = new Font("KaiTi", 11, FontStyle.Bold))
                using (SolidBrush brush = new SolidBrush(textColor))
                {
                    SizeF size = g.MeasureString(text, font);
                    g.DrawString(text, font, brush, (28 - size.Width) / 2, (25 - size.Height) / 2);
                }
            }

            return bmp;
        }

        /// <summary>
        /// 高亮走法
        /// </summary>
        private void HighlightMove(string move, Color color)
        {
            var (from, to) = MoveConverter.ParseICCS(move);
            if (from != Point.Empty && to != Point.Empty)
            {
                // 可以添加箭头或高亮效果
                // 这里简化处理
            }
        }

        /// <summary>
        /// 创建圆角区域
        /// </summary>
        private Region CreateRoundedRegion(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
            path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
            path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
            path.CloseFigure();
            return new Region(path);
        }

        // 拖动窗口
        private void TitlePanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _isDragging = true;
                _dragStartPoint = e.Location;
            }
        }

        private void TitlePanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragging)
            {
                Point newLocation = this.Location;
                newLocation.X += e.X - _dragStartPoint.X;
                newLocation.Y += e.Y - _dragStartPoint.Y;
                this.Location = newLocation;
            }
        }

        private void TitlePanel_MouseUp(object sender, MouseEventArgs e)
        {
            _isDragging = false;
        }
    }
}

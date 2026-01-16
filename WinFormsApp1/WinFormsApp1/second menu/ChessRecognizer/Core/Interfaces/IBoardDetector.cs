using System.Drawing;

namespace WinFormsApp1.second_menu.ChessRecognizer.Core.Interfaces
{
    /// <summary>
    /// 棋盘检测接口
    /// </summary>
    public interface IBoardDetector
    {
        /// <summary>
        /// 检测棋盘区域
        /// </summary>
        /// <param name="image">输入图像</param>
        /// <returns>棋盘检测结果</returns>
        Task<BoardDetectionResult> DetectBoardAsync(Bitmap image);

        /// <summary>
        /// 提取棋子区域
        /// </summary>
        /// <param name="image">棋盘图像</param>
        /// <param name="boardInfo">棋盘信息</param>
        /// <returns>棋子区域列表</returns>
        List<PieceRegion> ExtractPieceRegions(Bitmap image, BoardInfo boardInfo);

        /// <summary>
        /// 设置检测参数
        /// </summary>
        void SetParameters(BoardDetectionParameters parameters);
    }

    /// <summary>
    /// 棋盘检测结果
    /// </summary>
    public class BoardDetectionResult
    {
        public bool Success { get; set; }
        public BoardInfo BoardInfo { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public double Confidence { get; set; }
        public TimeSpan ProcessTime { get; set; }
        
        /// <summary>
        /// 归一化后的棋盘图像
        /// </summary>
        public System.Drawing.Bitmap NormalizedBoardImage { get; set; }
    }

    /// <summary>
    /// 棋盘信息
    /// </summary>
    public class BoardInfo
    {
        /// <summary>
        /// 棋盘四角坐标
        /// </summary>
        public PointF[] Corners { get; set; } = new PointF[4];

        /// <summary>
        /// 网格交叉点 [9列, 10行]
        /// </summary>
        public PointF[,] GridPoints { get; set; } = new PointF[9, 10];

        /// <summary>
        /// 单元格宽度
        /// </summary>
        public float CellWidth { get; set; }

        /// <summary>
        /// 单元格高度
        /// </summary>
        public float CellHeight { get; set; }

        /// <summary>
        /// 是否需要透视矫正
        /// </summary>
        public bool NeedsPerspectiveCorrection { get; set; }

        /// <summary>
        /// 透视变换矩阵
        /// </summary>
        public float[,] PerspectiveMatrix { get; set; }
    }

    /// <summary>
    /// 棋子区域
    /// </summary>
    public class PieceRegion
    {
        /// <summary>
        /// 棋盘列位置 (0-8)
        /// </summary>
        public int Column { get; set; }

        /// <summary>
        /// 棋盘行位置 (0-9)
        /// </summary>
        public int Row { get; set; }

        /// <summary>
        /// 区域边界
        /// </summary>
        public Rectangle Bounds { get; set; }

        /// <summary>
        /// 棋子图像
        /// </summary>
        public Bitmap Image { get; set; }

        /// <summary>
        /// 是否可能有棋子
        /// </summary>
        public bool HasPiece { get; set; }

        /// <summary>
        /// 颜色类型（红/黑/未知）
        /// </summary>
        public PieceColorType ColorType { get; set; }
    }

    /// <summary>
    /// 棋子颜色类型
    /// </summary>
    public enum PieceColorType
    {
        Unknown,
        Red,
        Black
    }

    /// <summary>
    /// 棋盘检测参数
    /// </summary>
    public class BoardDetectionParameters
    {
        /// <summary>
        /// 边缘检测阈值下限
        /// </summary>
        public double CannyThreshold1 { get; set; } = 50;

        /// <summary>
        /// 边缘检测阈值上限
        /// </summary>
        public double CannyThreshold2 { get; set; } = 150;

        /// <summary>
        /// 霍夫变换阈值
        /// </summary>
        public int HoughThreshold { get; set; } = 100;

        /// <summary>
        /// 最小线段长度
        /// </summary>
        public double MinLineLength { get; set; } = 50;

        /// <summary>
        /// 最大线段间隙
        /// </summary>
        public double MaxLineGap { get; set; } = 10;

        /// <summary>
        /// 线合并距离阈值
        /// </summary>
        public double LineMergeThreshold { get; set; } = 20;

        /// <summary>
        /// 棋子区域扩展比例
        /// </summary>
        public double PieceRegionScale { get; set; } = 0.8;
    }
}

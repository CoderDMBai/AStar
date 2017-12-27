using System.Drawing;

namespace AStar
{
    public class Cell
    {
        private Point _location;

        public Cell()
        {

        }

        public Cell(Point Location)
        {
            _location = Location;
        }

        public bool Equals(Cell cell)
        {
            if (cell == null) return false;
            if (this.Location.X == cell.Location.X && this.Location.Y == cell.Location.Y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 位置
        /// </summary>
        public Point Location
        {
            get { return _location; }
            set { _location = value; }
        }

        private Color _cellColor;
        /// <summary>
        /// 格子颜色
        /// </summary>
        public Color CellColor
        {
            get { return _cellColor; }
            set { _cellColor = value; }
        }
        private Cell _state;
        public Cell State
        {
            get
            {
                return _state;
            }

            set
            {
                _state = value;
            }
        }
        private cellState _cellState;
        /// <summary>
        /// 格子的状态
        /// </summary>
        public cellState CellState
        {
            get { return _cellState; }
            set { _cellState = value; }
        }
        /// <summary>
        /// 判断是否有Cell
        /// </summary>
        /// <param name="cell">一个格子</param>
        /// <param name="pt">要比较的坐标点</param>
        /// <returns>true或false</returns>
        public static bool HasCell(Cell cell, Point pt)
        {
            if (cell.Location == pt)
            {
                return true;
            }
            else
                return false;
        }

        private int _id;

        /// <summary>
        /// 编号
        /// </summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private int _realDistance;
        /// <summary>
        /// 到起始点的实际代价
        /// </summary>
        public int RealDistance
        {
            get { return _realDistance; }
            set { _realDistance = value; }
        }

        private int _evaluateDistance;
        /// <summary>
        /// 估计代价
        /// </summary>
        public int EvaluateDistance
        {
            get { return _evaluateDistance; }
            set { _evaluateDistance = value; }
        }

        private int _finalDistance;
        /// <summary>
        ///  最终代价
        /// </summary>
        public int FinalDistance
        {
            get { return _finalDistance; }
            set { _finalDistance = value; }
        }
        private Cell _parentCell;
        /// <summary>
        /// 父亲节点
        /// </summary>
        public Cell ParentCell
        {
            get { return _parentCell; }
            set { _parentCell = value; }
        }

        private Cell _upCell;
        /// <summary>
        /// 上方格子
        /// </summary>
        public Cell UpCell
        {
            get { return _upCell; }
            set { _upCell = value; }
        }

        private Cell _downCell;
        /// <summary>
        /// 下方格子
        /// </summary>
        public Cell DownCell
        {
            get { return _downCell; }
            set { _downCell = value; }
        }

        private Cell _rightCell;
        /// <summary>
        /// 右边格子
        /// </summary>
        public Cell RightCell
        {
            get { return _rightCell; }
            set { _rightCell = value; }
        }

        private Cell _leftCell;

        /// <summary>
        /// 左边格子
        /// </summary>
        public Cell LeftCell
        {
            get { return _leftCell; }
            set { _leftCell = value; }
        }

        private Cell _upLeftCell;

        /// <summary>
        /// 左上格子
        /// </summary>
        public Cell UpLeftCell
        {
            get
            {
                return _upLeftCell;
            }

            set
            {
                _upLeftCell = value;
            }
        }

        private Cell _downLeftCell;

        /// <summary>
        /// 左下格子
        /// </summary>
        public Cell DownLeftCell
        {
            get
            {
                return _downLeftCell;
            }

            set
            {
                _downLeftCell = value;
            }
        }

        private Cell _upRightCell;

        /// <summary>
        /// 右上格子
        /// </summary>
        public Cell UpRightCell
        {
            get
            {
                return _upRightCell;
            }

            set
            {
                _upRightCell = value;
            }
        }

        private Cell _downRightCell;

        /// <summary>
        /// 右下格子
        /// </summary>
        public Cell DownRightCell
        {
            get
            {
                return _downRightCell;
            }

            set
            {
                _downRightCell = value;
            }
        }

        /// <summary>
        ///  判断两个格子是否是相同的
        /// </summary>
        /// <param name="cell1">格子1</param>
        /// <param name="cell2">格子2</param>
        /// <returns>true或false</returns>
        public static bool IsSampleCell(Cell cell1, Cell cell2)
        {
            if (cell1.Location == cell2.Location)
            {
                return true;
            }
            else
                return false;
        }
    }

    public enum cellState
    {
        起点, 终点, 石头, 通路, 安全间距
    }
}

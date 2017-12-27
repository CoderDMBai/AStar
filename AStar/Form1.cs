using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace AStar
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            //初始化窗体组件
            InitializeComponent();
            //禁止软件对于不符合原则的跨线程运行的程序进行检查 ----> 忽略程序跨越线程运行导致的错误
            Control.CheckForIllegalCrossThreadCalls = false;
        }
        //结束线程
        Thread shThread = null;

        //创建类型为障碍物的对象集合
        List<Cell> stoneCells = new List<Cell>();
        //创建类型为安全间距的对象集合（新增）
        List<Cell> safeCells = new List<Cell>();
        //Open列表集合
        List<Cell> openList = new List<Cell>();
        //Closed列表集合
        List<Cell> closedList = new List<Cell>();

        //计时器，记录用时
        Stopwatch stopWatch = new Stopwatch();

        //起始点
        Cell startCell = null;
        //目标点
        Cell goalCell = null;

        //上下左右格子
        Cell upCell;
        Cell downCell;
        Cell rightCell;
        Cell leftCell;

        //左上格子
        Cell upLeftCell;
        //左下格子
        Cell downLeftCell;
        //右上格子
        Cell upRightCell;
        //左下格子
        Cell downRightCell;

        //状态值 判断是否是手动添加障碍物，默认为false
        bool isCreteaFlage = false;
        
        //状态值 ALT按键的KeyValue，默认为0
        int isCreteaCell = 0;

        //状态值 是否启用安全距离功能
        bool isUseSafeMode = true;

        //状态值 是否启用全局最优模式
        bool isUseGlobal = true;

        //状态值 
        bool isState = false;

        #region 在panel控件上面画网格
        public void DrawPanelMap()
        {
            Point pt1;
            Point pt2;
            Pen pen = new Pen(Color.Black);
            //行
            for (int i = 1; i < 50; i++)
            {
                pt1 = new Point(0, i * 20);
                pt2 = new Point(1000, i * 20);
                //DrawLine方法绘制一条连接两个 Point 结构的线。
                //调用某控件或窗体的 CreateGraphics 方法以获取对 Graphics 对象的引用，该对象表示该控件或窗体的绘图图面。在已存在的窗体或控件上绘图
                this.panelMap.CreateGraphics().DrawLine(pen, pt1, pt2);
            }
            //列
            for (int i = 0; i < 50; i++)
            {
                pt1 = new Point(i * 20, 0);
                pt2 = new Point(i * 20, 1000);
                //绘制一条连接两个 Point 结构的线。
                this.panelMap.CreateGraphics().DrawLine(pen, pt1, pt2);
            }
            //当画板更新的收，更新障碍物
            foreach (var stoneCell in stoneCells)
            {
                this.panelMap.CreateGraphics().FillRectangle(new SolidBrush(stoneCell.CellColor), stoneCell.Location.X + 1, stoneCell.Location.Y + 1, 19, 19);
            }
            //画起点
            if (startCell != null)
            {
                this.panelMap.CreateGraphics().FillRectangle(new SolidBrush(startCell.CellColor), startCell.Location.X + 1, startCell.Location.Y + 1, 19, 19);
            }
            //画终点
            if (goalCell != null)
            {
                this.panelMap.CreateGraphics().FillRectangle(new SolidBrush(goalCell.CellColor), goalCell.Location.X + 1, goalCell.Location.Y + 1, 19, 19);
            }            
        }

        /// <summary>
        /// 自定义画图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panelMap_Paint(object sender, PaintEventArgs e)
        {
            this.DrawPanelMap();
        }
        #endregion

        #region 随机生成障碍button实现
        private void btnCreateStone_Click(object sender, EventArgs e)
        {
            //随机数
            Random rd = new Random();
            for (int i = 0; i < 20; i++)
            {
                //产生2个随机数
                int j = rd.Next(50);
                int k = rd.Next(50);
                Point pt = new Point();
                pt.X = j * 20;
                pt.Y = k * 20;
                //判断障碍物集合是否有元素
                if (stoneCells.Count != 0)
                {
                    foreach (Cell cell in stoneCells)
                    {
                        //判断障碍物集合中的格子与随机产生的点位置是否相同
                        if (pt == cell.Location)
                        {
                            continue;
                        }
                    }
                    //位置不同则在该位置画障碍
                    this.drawStone(pt);
                    continue;
                }
                //障碍物集合中元素为0个，则在该处画障碍
                this.drawStone(pt);
            }
        }
        #endregion

        #region 画石头（障碍物）
        private void drawStone(Point pt)
        {
            Cell stoneCell = new Cell();
            stoneCell.Location = pt;
            stoneCell.CellColor = Color.Black;
            stoneCell.CellState = cellState.石头;
            //获取对Graphics对象的引用，在已存在的窗体或控件上绘图
            this.panelMap.CreateGraphics().FillRectangle(new SolidBrush(stoneCell.CellColor), stoneCell.Location.X + 1, stoneCell.Location.Y + 1, 19, 19);
            //添加到障碍物对象集合中
            this.stoneCells.Add(stoneCell);
        }
        #endregion

        #region 手动生成障碍button实现
        private void btnCtStoneHand_Click(object sender, EventArgs e)
        {
            if (this.btnCtStoneHand.Text == "手动生成障碍")
            {
                this.btnCtStoneHand.Text = "取消生成障碍";
                this.btnCtStoneHand.ForeColor = Color.Red;
                isCreteaFlage = true;
            }
            else
            {
                isCreteaFlage = false;
                this.btnCtStoneHand.Text = "手动生成障碍";
                this.btnCtStoneHand.ForeColor = Color.Black;
            }
        }
        #endregion

        #region 鼠标操作事件
        private void panelMap_MouseClick(object sender, MouseEventArgs e)
        {
            //记录按下的位置所在的行和列
            int row, clo;
            clo = e.X / 20;
            row = e.Y / 20;
            Point pt = new Point(clo * 20, row * 20);
            Cell cell = new Cell();
            cell.Location = pt;
            
            //鼠标左键按下，并且是单击
            if (e.Button == MouseButtons.Left && e.Clicks == 1)
            {
                //判断是否手动添加障碍物，鼠标左键添加障碍物
                if (isCreteaFlage)
                {
                    if (stoneCells.Count != 0)
                    {
                        foreach (Cell cell1 in stoneCells)
                        {
                            if (Cell.HasCell(cell1, pt))
                            {
                                MessageBox.Show("此位置已存在障碍物");
                                return;
                            }
                        }
                    }
                    this.drawStone(pt);
                    return;
                }
                //判断ALT是否按下，ALT加鼠标左键设置起点
                if (isCreteaCell == 18)
                {
                    //画起始点
                    if (startCell == null)
                    {
                        //对画起始点的异常处理，如果起始点，点在了障碍物上面，则提示“请另选方格”
                        if (stoneCells.Count != 0)
                        {
                            foreach (Cell cell1 in stoneCells)
                            {
                                if (Cell.HasCell(cell1, pt))
                                {
                                    MessageBox.Show("请另选方格");
                                    return;
                                }
                            }
                        }
                        cell.CellColor = Color.Green;
                        cell.CellState = cellState.起点;
                        startCell = cell;
                        //获取对Graphics对象的引用，在已存在的窗体或控件上绘图
                        this.panelMap.CreateGraphics().FillRectangle(new SolidBrush(cell.CellColor), cell.Location.X + 1, cell.Location.Y + 1, 19, 19);
                        //将按钮“手动生成障碍”与“随机生成障碍”状态设置为不可用
                        this.btnCtStoneHand.Enabled = false;
                        this.btnCtStoneRodom.Enabled = false;
                        return;
                    }
                    else
                    {
                        MessageBox.Show("已存在起点");
                    }
                }
            }
            //鼠标右键按下，并且的单击
            if(e.Button == MouseButtons.Right && e.Clicks == 1)
            {
                //判断是否手动添加障碍物，鼠标右键移除障碍物
                if (isCreteaFlage)
                {
                    if (stoneCells.Count != 0)
                    {
                        foreach (Cell cell1 in stoneCells)
                        {
                            if (Cell.HasCell(cell1, pt))
                            {
                                //在障碍物的对象集合中移除鼠标右键点击的元素
                                stoneCells.Remove(stoneCells.Where(p => p.Location == pt).FirstOrDefault());
                                //获取对Graphics对象的引用，在已存在的窗体或控件上绘图
                                this.panelMap.CreateGraphics().FillRectangle(new SolidBrush(Color.WhiteSmoke), cell.Location.X + 1, cell.Location.Y + 1, 19, 19);
                                return;
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                    return;
                }
                //判断ALT是否按下，ALT加鼠标左键设置目标点
                if (isCreteaCell == 18)
                {
                    //画目标点
                    if (goalCell == null)
                    {
                        //对画目标点的异常处理，如果目标点，点在了障碍物上面，则提示“请另选方格”
                        if (stoneCells.Count != 0)
                        {
                            foreach (Cell cell1 in stoneCells)
                            {
                                if (Cell.HasCell(cell1, pt))
                                {
                                    MessageBox.Show("请另选方格");
                                    return;
                                }
                            }
                        }
                        cell.CellColor = Color.Red;
                        cell.CellState = cellState.终点;
                        goalCell = cell;
                        //获取对Graphics对象的引用，在已存在的窗体或控件上绘图
                        this.panelMap.CreateGraphics().FillRectangle(new SolidBrush(cell.CellColor), cell.Location.X + 1, cell.Location.Y + 1, 19, 19);
                    }
                    else
                    {
                        MessageBox.Show("已存在终点");
                    }
                }
            }
        }
        #endregion

        #region 键盘操作事件
        /// <summary>
        /// 接收键盘按键KeyValue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            isCreteaCell = e.KeyValue;
        }
        /// <summary>
        /// 移除键盘按键KeyValue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            isCreteaCell = 0;
        }

        #endregion

        #region 开始寻路
        private void btnStartSearch_Click(object sender, EventArgs e)
        {
            //判断起始点与终止点的异常事件
            if (startCell == null || goalCell == null)
            {
                MessageBox.Show("缺少起点或目标点");
                return;
            }
            //将按钮“重置”与“开始寻路”状态设置为不可用
            this.btnRest.Enabled = false;
            this.btnStartSearch.Enabled = false;
            //获取起始点的估计代价
            startCell.EvaluateDistance = this.h(startCell);
            //将起始节点添加到closedList集合中
            this.closedList.Add(startCell);

            //安全模式
            if (isUseSafeMode)
            {
                foreach(Cell scell in stoneCells){
                    List<Cell> tempCells = new List<Cell>();
                    tempCells.Add(new Cell(new Point(scell.Location.X - 20,scell.Location.Y)));
                    tempCells.Add(new Cell(new Point(scell.Location.X + 20, scell.Location.Y)));
                    tempCells.Add(new Cell(new Point(scell.Location.X, scell.Location.Y - 20)));
                    tempCells.Add(new Cell(new Point(scell.Location.X, scell.Location.Y + 20)));

                    //遍历四周点，如果不是障碍物，则标记为安全间距
                    foreach (Cell temp in tempCells)
                    {
                        bool isStone = false;
                        foreach (Cell stone in stoneCells)
                        {
                            if (stone.Equals(temp))
                            {
                                isStone = true;
                            }
                        }
                        if (!isStone)
                        {
                            temp.CellColor = Color.Blue;
                            temp.CellState = cellState.安全间距;
                            //获取对Graphics对象的引用，在已存在的窗体或控件上绘图
                            this.panelMap.CreateGraphics().FillRectangle(new SolidBrush(temp.CellColor), temp.Location.X + 1, temp.Location.Y + 1, 19, 19);
                            safeCells.Add(temp);
                        }
                    }
                }
                //将所有安全间距元素加入障碍物列表中（按照障碍物处理）
                foreach (Cell scell in safeCells)
                {
                    stoneCells.Add(scell);
                }
            }

            //为 BeginSearch 打开一个线程
            shThread = new Thread(new ThreadStart(this.BeginSearch));
            //启动该线程.
            shThread.Start();
            Console.WriteLine(closedList.Count);
            //MessageBox.Show(closedList.Count.ToString());
        }
        #endregion

        #region 估计代价
        /// <summary>
        /// F = G + H
        /// F - 方块的总移动代价
        /// G - 开始点到当前方块的移动代价
        /// H - 当前方块到结束点的预估移动代价
        /// 详细信息：http://www.cnblogs.com/leoin2012/p/3899822.html
        /// </summary>
        /// <param name="nCell"></param>
        /// <returns></returns>
        public int h(Cell nCell)
        {
            int x = Math.Abs(nCell.Location.X - goalCell.Location.X) / 20;
            int y = Math.Abs(nCell.Location.Y - goalCell.Location.Y) / 20;
            int value = x + y;
            return value;
        }
        #endregion

        #region 寻找路径估计消耗最小的格子
        public void SearchBestCell()
        {
            //为起始节点初始化上下左右方格
            upCell = new Cell();
            downCell = new Cell();
            rightCell = new Cell();
            leftCell = new Cell();

            upLeftCell = new Cell();
            downLeftCell = new Cell();
            upRightCell = new Cell();
            downRightCell = new Cell();

            upCell.Location = new Point(closedList[closedList.Count - 1].Location.X, closedList[closedList.Count - 1].Location.Y - 20);
            downCell.Location = new Point(closedList[closedList.Count - 1].Location.X, closedList[closedList.Count - 1].Location.Y + 20);
            rightCell.Location = new Point(closedList[closedList.Count - 1].Location.X + 20, closedList[closedList.Count - 1].Location.Y);
            leftCell.Location = new Point(closedList[closedList.Count - 1].Location.X - 20, closedList[closedList.Count - 1].Location.Y);

            if (isUseGlobal)
            {
                upLeftCell.Location = new Point(closedList[closedList.Count - 1].Location.X - 20, closedList[closedList.Count - 1].Location.Y - 20);
                downLeftCell.Location = new Point(closedList[closedList.Count - 1].Location.X - 20, closedList[closedList.Count - 1].Location.Y + 20);
                upRightCell.Location = new Point(closedList[closedList.Count - 1].Location.X + 20, closedList[closedList.Count - 1].Location.Y - 20);
                downRightCell.Location = new Point(closedList[closedList.Count - 1].Location.X + 20, closedList[closedList.Count - 1].Location.Y + 20);
            }
            
            //判断这四个方格是否处于边界是否是障碍物，如果不是则将方格加入到Open集合列表中
            if (this.IsStoneOrOutWord(stoneCells, upCell) && !IsInClosed(upCell) && !IsInOpen(upCell))
            {
                upCell.ParentCell = closedList[closedList.Count - 1];
                upCell.RealDistance = upCell.ParentCell.RealDistance + 10;
                upCell.EvaluateDistance = this.h(upCell);
                upCell.FinalDistance = upCell.EvaluateDistance + upCell.RealDistance;
                this.openList.Add(upCell);
                //this.panelMap.CreateGraphics().DrawString("开", new Font("宋体", 10), new SolidBrush(Color.Red), upCell.Location);
                //Thread.Sleep(100);
            }
            if (this.IsStoneOrOutWord(stoneCells, downCell) && !IsInClosed(downCell) && !IsInOpen(downCell))
            {
                downCell.ParentCell = closedList[closedList.Count - 1];
                downCell.RealDistance = downCell.ParentCell.RealDistance + 10;
                downCell.EvaluateDistance = this.h(downCell);
                downCell.FinalDistance = downCell.EvaluateDistance + downCell.RealDistance;
                this.openList.Add(downCell);
                //this.panelMap.CreateGraphics().DrawString("开", new Font("宋体", 10), new SolidBrush(Color.Red), downCell.Location);
                //Thread.Sleep(100);
            }
            if (this.IsStoneOrOutWord(stoneCells, rightCell) && !IsInClosed(rightCell) && !IsInOpen(rightCell))
            {
                rightCell.ParentCell = closedList[closedList.Count - 1];
                rightCell.RealDistance = rightCell.ParentCell.RealDistance + 10;
                rightCell.EvaluateDistance = this.h(rightCell);
                rightCell.FinalDistance = rightCell.EvaluateDistance + rightCell.RealDistance;
                this.openList.Add(rightCell);
                //this.panelMap.CreateGraphics().DrawString("开", new Font("宋体", 10), new SolidBrush(Color.Red), rightCell.Location);
                //Thread.Sleep(100);
            }
            if (this.IsStoneOrOutWord(stoneCells, leftCell) && !IsInClosed(leftCell) && !IsInOpen(leftCell))
            {
                leftCell.ParentCell = closedList[closedList.Count - 1];
                leftCell.RealDistance = leftCell.ParentCell.RealDistance + 10;
                leftCell.EvaluateDistance = this.h(leftCell);
                leftCell.FinalDistance = leftCell.EvaluateDistance + leftCell.RealDistance;
                this.openList.Add(leftCell);
                //this.panelMap.CreateGraphics().DrawString("开", new Font("宋体", 10), new SolidBrush(Color.Red), leftCell.Location);
                //Thread.Sleep(100);
            }
            if(isUseGlobal)
            {
                if (this.IsStoneOrOutWord(stoneCells, upLeftCell) && !IsInClosed(upLeftCell) && !IsInOpen(upLeftCell))
                {
                    upLeftCell.ParentCell = closedList[closedList.Count - 1];
                    upLeftCell.RealDistance = upLeftCell.ParentCell.RealDistance + 14;
                    upLeftCell.EvaluateDistance = this.h(upLeftCell);
                    upLeftCell.FinalDistance = upLeftCell.EvaluateDistance + upLeftCell.RealDistance;
                    this.openList.Add(upLeftCell);
                    //this.panelMap.CreateGraphics().DrawString("开", new Font("宋体", 10), new SolidBrush(Color.Red), upLeftCell.Location);
                    //Thread.Sleep(100);
                }
                if (this.IsStoneOrOutWord(stoneCells, downLeftCell) && !IsInClosed(downLeftCell) && !IsInOpen(downLeftCell))
                {
                    downLeftCell.ParentCell = closedList[closedList.Count - 1];
                    downLeftCell.RealDistance = downLeftCell.ParentCell.RealDistance + 14;
                    downLeftCell.EvaluateDistance = this.h(downLeftCell);
                    downLeftCell.FinalDistance = downLeftCell.EvaluateDistance + downLeftCell.RealDistance;
                    this.openList.Add(downLeftCell);
                    //this.panelMap.CreateGraphics().DrawString("开", new Font("宋体", 10), new SolidBrush(Color.Red), downLeftCell.Location);
                    //Thread.Sleep(100);
                }
                if (this.IsStoneOrOutWord(stoneCells, upRightCell) && !IsInClosed(upRightCell) && !IsInOpen(upRightCell))
                {
                    upRightCell.ParentCell = closedList[closedList.Count - 1];
                    upRightCell.RealDistance = upRightCell.ParentCell.RealDistance + 14;
                    upRightCell.EvaluateDistance = this.h(upRightCell);
                    upRightCell.FinalDistance = upRightCell.EvaluateDistance + upRightCell.RealDistance;
                    this.openList.Add(upRightCell);
                    //this.panelMap.CreateGraphics().DrawString("开", new Font("宋体", 10), new SolidBrush(Color.Red), upRightCell.Location);
                    //Thread.Sleep(100);
                }
                if (this.IsStoneOrOutWord(stoneCells, downRightCell) && !IsInClosed(downRightCell) && !IsInOpen(downRightCell))
                {
                    downRightCell.ParentCell = closedList[closedList.Count - 1];
                    downRightCell.RealDistance = downRightCell.ParentCell.RealDistance + 14;
                    downRightCell.EvaluateDistance = this.h(downRightCell);
                    downRightCell.FinalDistance = downRightCell.EvaluateDistance + downRightCell.RealDistance;
                    this.openList.Add(downRightCell);
                    //this.panelMap.CreateGraphics().DrawString("开", new Font("宋体", 10), new SolidBrush(Color.Red), downRightCell.Location);
                    //Thread.Sleep(100);
                }
            }
            
            List<Cell> minCells = new List<Cell>();
            List<Cell> minestCells = new List<Cell>();
            int min = 1000;
            int n = 1000;
            //在open集合中找出最小的f(n)值
            foreach (Cell cell in openList)
            {
                if (cell.FinalDistance <= min)
                {
                    min = cell.FinalDistance;
                }
            }
            //找出f(n)值最小的格子
            for (int i = 0; i < this.openList.Count; i++)
            {
                if (openList[i].FinalDistance == min)
                {
                    minCells.Add(openList[i]);
                }
            }
            //在f(n)值最小的格子集合中找出最小的g(n)值
            for (int i = 0; i < minCells.Count; i++)
            {
                if (minCells[i].EvaluateDistance <= n)
                {
                    n = minCells[i].EvaluateDistance;
                }
            }
            //在f(n)值最小的格子集合中找出g(n)值最小的格子
            for (int i = 0; i < minCells.Count; i++)
            {
                if (minCells[i].EvaluateDistance == n)
                {
                    minestCells.Add(minCells[i]);
                }
            }
            Random rd = new Random();
            //在f(n)值和g(n)的格子集合中随机选取一个格子
            if (minestCells.Count == 0)
            {
                MessageBox.Show("苦海无边，回头是岸！");
                isState = true;
                return;
            }
            int j = rd.Next(minestCells.Count);
            minestCells[j].CellColor = Color.Yellow;
            this.closedList.Add(minestCells[j]);
            this.panelMap.CreateGraphics().FillRectangle(new SolidBrush(Color.Orange), this.closedList[closedList.Count - 1].Location.X + 1, this.closedList[closedList.Count - 1].Location.Y + 1, 19, 19);
            //this.panelMap.CreateGraphics().DrawString("关", new Font("宋体", 10), new SolidBrush(Color.Red), this.closedList[closedList.Count - 1].Location);
            //Thread.Sleep(100);
            this.panelMap.CreateGraphics().FillRectangle(new SolidBrush(Color.Yellow), this.closedList[closedList.Count - 1].Location.X + 1, this.closedList[closedList.Count - 1].Location.Y + 1, 19, 19);
            //从open集合中移除刚刚添加到closed集合中的格子
            for (int i = 0; i < openList.Count; i++)
            {
                if (Cell.IsSampleCell(openList[i], minestCells[j]))
                {
                    openList.Remove(openList[i]);
                    break;
                }
            }
        }
        #endregion

        #region 开始搜索
        public void BeginSearch()
        {
            //开始搜索的时候，计时开始
            stopWatch.Start();
            //不停的搜索直到到达目的地
            while (true)
            {
                this.SearchBestCell();
                if (isState)
                { 
                    return;
                }
                Cell closedCell = this.closedList[closedList.Count - 1];
                Cell _upCell = new Cell();
                Cell _downCell = new Cell();
                Cell _rightCell = new Cell();
                Cell _leftCell = new Cell();

                Cell _upLeftCell = new Cell();
                Cell _downLeftCell = new Cell();
                Cell _upRightCell = new Cell();
                Cell _downRightCell = new Cell();

                _upCell.Location = new Point(closedCell.Location.X, closedCell.Location.Y + 20);
                _downCell.Location = new Point(closedCell.Location.X, closedCell.Location.Y - 20);
                _rightCell.Location = new Point(closedCell.Location.X + 20, closedCell.Location.Y);
                _leftCell.Location = new Point(closedCell.Location.X - 20, closedCell.Location.Y);

                if (isUseGlobal)
                {
                    _upLeftCell.Location = new Point(closedCell.Location.X - 20, closedCell.Location.Y - 20);
                    _downLeftCell.Location = new Point(closedCell.Location.X - 20, closedCell.Location.Y + 20);
                    _upRightCell.Location = new Point(closedCell.Location.X + 20, closedCell.Location.Y - 20);
                    _downRightCell.Location = new Point(closedCell.Location.X + 20, closedCell.Location.Y + 20);
                }

                if (Cell.IsSampleCell(_upCell, goalCell))
                {
                    break;
                }
                if (Cell.IsSampleCell(_downCell, goalCell))
                {
                    break;
                }
                if (Cell.IsSampleCell(_rightCell, goalCell))
                {
                    break;
                }
                if (Cell.IsSampleCell(_leftCell, goalCell))
                {
                    break;
                }
                if (isUseGlobal)
                {
                    if (Cell.IsSampleCell(_upLeftCell, goalCell))
                    {
                        break;
                    }
                    if (Cell.IsSampleCell(_downLeftCell, goalCell))
                    {
                        break;
                    }
                    if (Cell.IsSampleCell(_upRightCell, goalCell))
                    {
                        break;
                    }
                    if (Cell.IsSampleCell(_downRightCell, goalCell))
                    {
                        break;
                    }
                }
            }
            
            List<Cell> bestLoad = new List<Cell>();
            Cell lastClosedCell = new Cell();
            int n = 1000;

            for (int i = 0; i < this.closedList.Count; i++)
            {
                if (this.closedList[i].EvaluateDistance <= n)
                {
                    n = this.closedList[i].EvaluateDistance;
                }
            }

            for (int i = 0; i < this.closedList.Count; i++)
            {
                if (this.closedList[i].EvaluateDistance == n)
                {
                    lastClosedCell = this.closedList[i];
                    break;
                }
            }
            
            while (true)
            {
                if (Cell.IsSampleCell(lastClosedCell.ParentCell, startCell))
                {
                    bestLoad.Add(lastClosedCell);
                    break;
                }
                else
                {
                    bestLoad.Add(lastClosedCell);
                    lastClosedCell = lastClosedCell.ParentCell;
                }
            }
            
            for (int i = 0; i < bestLoad.Count; i++)
            {
                if (isUseGlobal == true)
                {
                    if (bestLoad[0].Location.Y > goalCell.Location.Y && bestLoad[0].Location.X == goalCell.Location.X)
                    {
                        this.panelMap.CreateGraphics().FillRectangle(new SolidBrush(Color.Purple), bestLoad[0].Location.X + 1, bestLoad[0].Location.Y + 1 - 20, 19, 19);
                    }
                    if (bestLoad[0].Location.Y < goalCell.Location.Y && bestLoad[0].Location.X == goalCell.Location.X)
                    {
                        this.panelMap.CreateGraphics().FillRectangle(new SolidBrush(Color.Purple), bestLoad[0].Location.X + 1, bestLoad[0].Location.Y + 1 + 20, 19, 19);
                    }
                    if (bestLoad[0].Location.X > goalCell.Location.X && bestLoad[0].Location.Y == goalCell.Location.Y)
                    {
                        this.panelMap.CreateGraphics().FillRectangle(new SolidBrush(Color.Purple), bestLoad[0].Location.X + 1 - 20, bestLoad[0].Location.Y + 1, 19, 19);
                    }
                    if (bestLoad[0].Location.X < goalCell.Location.X && bestLoad[0].Location.Y == goalCell.Location.Y)
                    {
                        this.panelMap.CreateGraphics().FillRectangle(new SolidBrush(Color.Purple), bestLoad[0].Location.X + 1 + 20, bestLoad[0].Location.Y + 1, 19, 19);
                    }        
                }
                this.panelMap.CreateGraphics().FillRectangle(new SolidBrush(Color.Purple), bestLoad[i].Location.X + 1, bestLoad[i].Location.Y + 1, 19, 19);
                //Thread.Sleep(100);
            }
            this.btnRest.Enabled = true;
            //结束计时
            stopWatch.Stop();
            //转换格式
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}.{1:00000000}s", ts.Seconds, ts.Milliseconds);
            MessageBox.Show("RunTime: " + elapsedTime);
        }
        #endregion

        #region 判断节点是否存在与closed表中
        public bool IsInClosed(Cell cell)
        {
            foreach (Cell cell1 in closedList)
            {
                if (Cell.IsSampleCell(cell, cell1))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion 

        #region 判断该节点是否已存在于Open列表中
        public bool IsInOpen(Cell cell)
        {
            foreach (Cell cell1 in openList)
            {
                if (Cell.IsSampleCell(cell, cell1))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region 判断节点是否存在于bestLoad列表中
        public bool IsInBestLoad(Cell cell, List<Cell> bestLoad)
        {
            foreach (Cell cell1 in bestLoad)
            {
                if (Cell.IsSampleCell(cell, cell1))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region  判断方格是否处于边界是否是障碍物是否已存在,假如两者都不是则返回true
        public bool IsStoneOrOutWord(List<Cell> cellList, Cell cell1)
        {
            if (cell1.Location.X >= 0 && cell1.Location.X < this.panelMap.Width && cell1.Location.Y >= 0 && cell1.Location.Y < this.panelMap.Height)
            {
                for (int i = 0; i < cellList.Count; i++)
                {
                    if (Cell.IsSampleCell(cell1, cellList[i]))
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }
        #endregion 

        #region 重置窗体
        private void btnRest_Click(object sender, EventArgs e)
        {
            if (startCell != null)
            {
                this.panelMap.CreateGraphics().FillRectangle(new SolidBrush(Color.WhiteSmoke), startCell.Location.X + 1, startCell.Location.Y + 1, 19, 19);
                startCell = null;
            }
            if (goalCell != null)
            {
                this.panelMap.CreateGraphics().FillRectangle(new SolidBrush(Color.WhiteSmoke), goalCell.Location.X + 1, goalCell.Location.Y + 1, 19, 19);
                goalCell = null;
            }
            for (int i = 0; i < stoneCells.Count; i++)
            {
                this.panelMap.CreateGraphics().FillRectangle(new SolidBrush(Color.WhiteSmoke), stoneCells[i].Location.X + 1, stoneCells[i].Location.Y + 1, 19, 19);
            }
            stoneCells.Clear();
            safeCells.Clear();
            for (int i = 0; i < closedList.Count; i++)
            {
                this.panelMap.CreateGraphics().FillRectangle(new SolidBrush(Color.WhiteSmoke), closedList[i].Location.X + 1, closedList[i].Location.Y + 1, 19, 19);
            }

            for (int i = 0; i < openList.Count; i++)
            {
                this.panelMap.CreateGraphics().FillRectangle(new SolidBrush(Color.WhiteSmoke), openList[i].Location.X + 1, openList[i].Location.Y + 1, 19, 19);
            }
            closedList.Clear();
            openList.Clear();
            this.btnCtStoneHand.Enabled = true;
            this.btnCtStoneRodom.Enabled = true;
            this.btnStartSearch.Enabled = true;
        }
        #endregion 

        /// <summary>
        /// 关闭窗口，结束线程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (shThread != null)
            {
                //终止线程
                shThread.Abort();
            }
        }

        private void cbSafeMode_CheckedChanged(object sender, EventArgs e)
        {
            isUseSafeMode = cbSafeMode.Checked;
        }

        private void cbGlobal_CheckedChanged(object sender, EventArgs e)
        {
            isUseGlobal = cbGlobal.Checked;
        }
    }
}

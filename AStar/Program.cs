using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AStar
{
    static class Program
    {
        //声明开始线程
        [STAThread]

        /// <summary>
        /// 应用程序的入口。
        /// </summary>
        static void Main()
        {
            //使控件样式与当前系统样式相同
            Application.EnableVisualStyles();
            //设置控件显示文本的默认方式 true:GDI+方式显示文本；false:GDI+方式显示文本
            Application.SetCompatibleTextRenderingDefault(false);
            //传参，匿名方式
            Application.Run(new Form1());
        }
    }
}

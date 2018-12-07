using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace back_check
{
    public partial class Form1 : Form
    {
        string exeName = "WindowsFormsApp1";
        public Form1()
        {
            InitializeComponent();
            // 背景自动执行
            System.Timers.Timer pTimer = new System.Timers.Timer(5000);//每隔5秒执行一次，没用winfrom自带的
            pTimer.Elapsed += pTimer_Elapsed;//委托，要执行的方法
            pTimer.AutoReset = true;//获取该定时器自动执行
            pTimer.Enabled = true;//这个一定要写，要不然定时器不会执行的
            Control.CheckForIllegalCrossThreadCalls = false;//这个不太懂，有待研究
        }
        private void pTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
              button1_Click(null, null);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Process[] processes = Process.GetProcessesByName(exeName);
            if (processes.Length == 0)
            {
               open_p(null,null);             
            }                          
        }

        private void open_p(object sender, EventArgs e)
        {                  
          System.Diagnostics.Process.Start(@"C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\StartUp\\WindowsFormsApp1.exe");            
        }
    }
   
}

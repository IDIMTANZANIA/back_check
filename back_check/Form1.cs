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
using System.Runtime.InteropServices;

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

            System.Timers.Timer pTTimer = new System.Timers.Timer(20000);//每隔5秒执行一次，没用winfrom自带的
            pTTimer.Elapsed += pTTimer_Elapsed;//委托，要执行的方法
            pTTimer.AutoReset = true;//获取该定时器自动执行
            pTTimer.Enabled = true;//这个一定要写，要不然定时器不会执行的
            Control.CheckForIllegalCrossThreadCalls = false;//这个不太懂，有待研究
          
        }
        private void pTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
         //   textBox2.Text += 111;
            button1_Click(null, null);

        }
        private void pTTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            DateTime dt = DateTime.Now;  //
            int y = 0; int yue = 0;
            int d = 0; int h = 0;
            int n = 0;
            y = dt.Year;      //
            yue = dt.Month;     //
            d = dt.Day;       //
            h = dt.Hour;      //
            n = dt.Minute;    // tring logging;        
            if ((h == 7)&&(n==0))
            {
                button2_Click(null, null);
            }
        }

            private void button1_Click(object sender, EventArgs e)
        {
            Process[] processes = Process.GetProcessesByName(exeName);
            if (processes.Length == 0)
            {
                open_p(null,null);
              //  textBox2.Text += 141;
            }
            GC.Collect();
        }

        private void open_p(object sender, EventArgs e)
        {                  
          System.Diagnostics.Process.Start(@"C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\StartUp\\WindowsFormsApp1.exe");            
        }

        public class ShutDownSys
        {
            //C#关机代码
            //这个结构体将会传递给API。使用StructLayout
            //（...特性，确保其中成员是按顺序排列的，C#编译器不会对其进行调整）
            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            internal struct TokPriv1Luid
            {
                public int Count; public long Luid; public int Attr;
            }

            //以下使用DLLImport特性导入了所需的Windows API。
            //导入这些方法必须是static extern的，并且没有方法体。
            //调用这些方法就相当于调用Windows API。
            [DllImport("kernel32.dll", ExactSpelling = true)]
            internal static extern IntPtr GetCurrentProcess();

            [DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]
            internal static extern bool OpenProcessToken(IntPtr h, int acc, ref IntPtr phtok);

            [DllImport("advapi32.dll", SetLastError = true)]
            internal static extern bool LookupPrivilegeValueA
            (string host, string name, ref long pluid);

            [DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]
            internal static extern bool
            AdjustTokenPrivileges(IntPtr htok, bool disall, ref TokPriv1Luid newst, int len, IntPtr prev, IntPtr relen);

            [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
            internal static extern bool ExitWindowsEx(int flg, int rea);

            //C#关机代码
            //以下定义了在调用WinAPI时需要的常数。
            //这些常数通常可以从Platform SDK的包含文件（头文件）中找到。
            public const int SE_PRIVILEGE_ENABLED = 0x00000002;
            public const int TOKEN_QUERY = 0x00000008;
            public const int TOKEN_ADJUST_PRIVILEGES = 0x00000020;
            public const string SE_SHUTDOWN_NAME = "SeShutdownPrivilege";
            public const int EWX_LOGOFF = 0x00000000;
            public const int EWX_SHUTDOWN = 0x00000001;
            public const int EWX_REBOOT = 0x00000002;
            public const int EWX_FORCE = 0x00000004;
            public const int EWX_POWEROFF = 0x00000008;
            public const int EWX_FORCEIFHUNG = 0x00000010;
            // 通过调用WinAPI实现关机，主要代码再最后一行ExitWindowsEx  //这调用了同名的WinAPI，正好是关机用的。


            public static void DoExitWin(int flg)
            {
                bool ok;
                TokPriv1Luid tp;
                IntPtr hproc = GetCurrentProcess();
                IntPtr htok = IntPtr.Zero;
                ok = OpenProcessToken(hproc, TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, ref htok);
                tp.Count = 1;
                tp.Luid = 0;
                tp.Attr = SE_PRIVILEGE_ENABLED;
                ok = LookupPrivilegeValueA(null, SE_SHUTDOWN_NAME, ref tp.Luid);
                ok = AdjustTokenPrivileges(htok, false, ref tp, 0, IntPtr.Zero, IntPtr.Zero);
                ok = ExitWindowsEx(flg, 0);
            }
        }

        //调用
        public void Reboot()
        {
            ShutDownSys.DoExitWin(ShutDownSys.EWX_FORCE | ShutDownSys.EWX_REBOOT);
        }

        public void ShutDown()
        {
            ShutDownSys.DoExitWin(ShutDownSys.EWX_FORCE | ShutDownSys.EWX_POWEROFF);
        }

        public void LogOff()
        {
            ShutDownSys.DoExitWin(ShutDownSys.EWX_FORCE | ShutDownSys.EWX_LOGOFF);
        }

        //关机
   //     private void button1_Click(object sender, EventArgs e)
   //     {
   //         ShutDown();
   //     }

        //重启
        private void button2_Click(object sender, EventArgs e)
        {
            Reboot();
         //   textBox1.Text += 17790;

        }

    }

}

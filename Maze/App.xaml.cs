using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Maze
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        [DllImport("user32.dll")]
        private static extern
         bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        private static extern
            bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll")]
        private static extern
            bool IsIconic(IntPtr hWnd);

        private const int SW_HIDE = 0;
        private const int SW_SHOWNORMAL = 1;
        private const int SW_SHOWMINIMIZED = 2;
        private const int SW_SHOWMAXIMIZED = 3;
        private const int SW_SHOWNOACTIVATE = 4;
        private const int SW_RESTORE = 9;
        private const int SW_SHOWDEFAULT = 10;


        private static Mutex mutex = new Mutex(true, "123MazeKey123");
        //private static MainWindow mainWindow = null;

        App()
        {
            InitializeComponent();
        }

        [STAThread]
        static void Main()
        {
            
            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                MainWindow window = new MainWindow();
                App app = new App();
                app.Run(window);
                mutex.ReleaseMutex();
            }
            else
            {
                string currentProcessName = Process.GetCurrentProcess().ProcessName;


                Process[] processes = Process.GetProcessesByName(currentProcessName);
                if (processes.Length > 1)
                {

                    // get our process
                    Process p = Process.GetCurrentProcess();
                    int n = 0;        // assume the other process is at index 0
                                      // if this process id is OUR process ID...
                    if (processes[0].Id == p.Id)
                    {
                        // then the other process is at index 1
                        n = 1;
                    }
                    // get the window handle
                    IntPtr hWnd = processes[n].MainWindowHandle;
                    // if iconic, we need to restore the window
                    if (IsIconic(hWnd))
                    {
                        ShowWindowAsync(hWnd, SW_RESTORE);
                    }
                    // bring it to the foreground
                    SetForegroundWindow(hWnd);
                    // exit our process
                    return;
                    //mainWindow.WindowState = WindowState.Normal;
                }
            }

            
        }
    }
}


using Maze;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Maze
{
    /// <summary>
    /// Interaction logic for Grid.xaml
    /// </summary>
    public partial class MazeScreen : Window
    {
        public Window gridWindow;
        private Grid mazeGrid;
        public static TextBox[ , ] TextBoxArray ;
        public static  int[,] result = null;
        private StackPanel timerPanel;
        private static TextBlock timerTextBlock;

        //Timer

        DispatcherTimer dt = new DispatcherTimer();
        Stopwatch sw = new Stopwatch();
        string currentTime = string.Empty;


        // for coluring
        public static BrushConverter bc = new BrushConverter();

        public MazeScreen()
        {
            InitializeComponent();
            gridWindow = GridWindow;
            mazeGrid = MazeGrid;
            timerPanel = TimerPanel;
            timerTextBlock = TimerTextBlock;
            String filePath = MainWindow.oFiles[MainWindow.filesList.SelectedIndex];
            Console.WriteLine(MainWindow.filesList.SelectedIndex);         
            Files.readfromfile(out result, filePath);
            TextBoxArray = new TextBox[result.GetLength(0), result.GetLength(1)];
            CreateDynamicWPFGrid(result.GetLength(0),result.GetLength(1));

            // Timer
            // Timer
            dt.Tick += new EventHandler(dt_Tick);
            dt.Interval = new TimeSpan(0, 0, 0, 0, 1);

        }

        void dt_Tick(object sender, EventArgs e)
        {
            if (sw.IsRunning)
            {
                TimeSpan ts = sw.Elapsed;
                currentTime = String.Format("{0:00}:{1:00}:{2:00}",
                    ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                timerTextBlock.Text = currentTime;
            }
        }



        private async void colorPath(List<int> points)
        {
            if(points.Count==0)
            {
                resetTimer();
                MessageBox.Show("There are No Solution");
            
            }
            else
            {
                for (int i = points.Count() - 2; i > 0; i--)
                {
                    int row = points[i] / result.GetLength(1);
                    int col = points[i] % result.GetLength(1);
                    await Task.Delay(250);
                    TextBoxArray[row, col].Background = (Brush)bc.ConvertFrom(Colors.path);
                }
                resetTimer();
            }
          
           
        }
        private  void solveMaze(object sender, RoutedEventArgs e)
        {
            ////timer 
            startTimer();

            MazeOperations m = new MazeOperations(result.GetLength(0), result.GetLength(1));
            int[] sourceAndDest = m.getSourceAndDest(result);
            if (MainWindow.flag == 1 )
            {
                SequentialMaze sq = new SequentialMaze(result,result.GetLength(0), result.GetLength(1));
            
                List<int> points = sq.findPath(sourceAndDest[0], sourceAndDest[1]);

                colorPath(points);
                //_timer.Stop();
            }
            else
            {
             
                ParallelMaze pm = new ParallelMaze(result, result.GetLength(0), result.GetLength(1));
                List<int> points = pm.findPath(sourceAndDest[0], sourceAndDest[1]);
                colorPath(points);
                //_timer.Stop();

            }
        }
       

        private void goToMain(object sender, RoutedEventArgs e)
        {
            // switch windows
            gridWindow.Hide();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }


        private void CreateDynamicWPFGrid(int row,int col)
        {
            TextBox cell;
            
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    cell = new TextBox();
                    TextBoxArray[i, j] = cell;

                    mazeGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star), MinHeight = 25 });
                    mazeGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star), MinWidth = 25 });

                    cell.BorderThickness = new Thickness(1);
                    cell.IsReadOnly = true;

                    //// check if it's the start then color it in red
                    if (result[i,j]==States.hoverCar)
                    {
                        cell.Background = (Brush)bc.ConvertFrom(Colors.hoverCar);
                    }
                    // check if it's a block then color it in dark blue
                    else if (result[i, j] == States.block)
                    {
                        cell.Background = (Brush)bc.ConvertFrom(Colors.block);
                    }
                    // check if it's the destination then color it in green
                    else if (result[i, j] == States._out)
                    {
                        cell.Background = (Brush)bc.ConvertFrom(Colors._out);
                    }
                    // check if it's free node then color it in blue
                    else
                    {
                        cell.Background = (Brush)bc.ConvertFrom(Colors.free);
                    }

                    Grid.SetColumn(cell, j);
                    Grid.SetRow(cell, i);

                    mazeGrid.Children.Add(cell);

                }
            }
        }

        void MazeScreen_Closeing(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void startTimer()
        {
            timerPanel.Visibility = Visibility.Visible;
            sw.Start();
            dt.Start();

        }

        private void resetTimer()
        {
            sw.Reset();
            timerTextBlock.Text = currentTime;
        }

    }

}

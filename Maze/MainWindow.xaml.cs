using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Maze
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Window window;
        private StackPanel buttonsPanel, filesPanel;
        private Button seqBtn, parlBtn, exitBtn;
        public static ListView filesList;
        public static int flag; // if flag = 1 then go to seq, if flag = 2 then go to parl
        private List<String> filesName;
        public static string[] oFiles;
        public static int fileIndex;

        // Random Maze
        //private Button gridBtn;
        //private StackPanel inputPanel;
        //public static int col, row;
        //private TextBox colTxt, rowTxt;



   
      
        public MainWindow()
        {
            InitializeComponent();
            window = Window;
            filesList = FilesList;
            buttonsPanel = ButtonsPanel;
            filesPanel = FilesPanel;
            seqBtn = SeqBtn;
            parlBtn = ParlBtn;
            exitBtn = ExitBtn;
            getFilesPath();


            //Random Maze
            //inputPanel = InputPanel;
            //colTxt = ColTxt;
            //rowTxt = RowTxt;
            //gridBtn = GridBtn;
        }

        public void getFilesPath()
        {
            oFiles = Directory.GetFiles(@"E:\Maze\Maze\Maze\mazes");
            filesName = new List<String>();

            for (int i= 0 ; i <oFiles.Length; i++)
            {
                string[] words=oFiles[i].Split('\\');
                filesName.Add(words[words.Length-1]);
                              
            }
            filesList.ItemsSource = filesName;
                       
        }

        private void gotToSeq(object sender, RoutedEventArgs e)
        {
            showFilesMenu();
            flag = 1;
        }

        private void gotToParl(object sender, RoutedEventArgs e)
        {
            showFilesMenu();
            flag = 2;
        }

        private void closeWindow(object sender, RoutedEventArgs e)
        {
            window.Close();
            System.Windows.Application.Current.Shutdown();
        }

        private void FilesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //switch windows
            goToMaze();
            fileIndex = filesList.SelectedIndex;
        
        }

        private void showFilesMenu()
        {
            buttonsPanel.Visibility = Visibility.Collapsed;
            filesPanel.Visibility = Visibility.Visible;
        }

        private void hideFilesMenu()
        {
            filesPanel.Visibility = Visibility.Collapsed;
            buttonsPanel.Visibility = Visibility.Visible;
        }

        private void goToMaze()
        {
            //switch windows
            window.Hide();
            MazeScreen mazeScreen = new MazeScreen();
            mazeScreen.Show();
        }

        //Random Maze
        //private void showInput()
        //{
        //    buttonsPanel.Visibility = Visibility.Collapsed;
        //    inputPanel.Visibility = Visibility.Visible;
        //}

        //Random Maze
        //private void hideInput()
        //{
        //    inputPanel.Visibility = Visibility.Collapsed;
        //    buttonsPanel.Visibility = Visibility.Visible;
        //}

        //Random Maze
        //private void goToGrid(object sender, RoutedEventArgs e)
        //{
        //    showInput();

        //    int value;

        //    if (Convert.ToInt32(colTxt.Text) <= 0 || Convert.ToInt32(rowTxt.Text) <= 0)
        //    {
        //        MessageBox.Show("Must not be 0");

        //    }
        //    else if (int.TryParse(colTxt.Text, out value) && int.TryParse(rowTxt.Text, out value))
        //    {
        //        col = Convert.ToInt32(colTxt.Text);
        //        row = Convert.ToInt32(rowTxt.Text);
        //        // switch windows
        //        window.Hide();
        //        MazeScreen mazeScreen = new MazeScreen();
        //        mazeScreen.Show();
        //    }
        //    else
        //    {
        //        MessageBox.Show("Pleae enter valid numbers");
        //    }
        //}

        // Random Maze
        //private void backToMain(object sender, RoutedEventArgs e)
        //{
        //    hideInput();
        //}

    }

}

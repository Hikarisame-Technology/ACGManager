using ACGManager.Pages;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;


namespace ACGManager
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private Frame Home_Page = new Frame() { Content = new Home() };
        private Frame Search_Page = new Frame() { Content = new Search() };
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //程序启动数量限制
            string appName = Process.GetCurrentProcess().ProcessName;
            int processTotal = Process.GetProcessesByName(appName).Length;
            if (processTotal > 1)
            {
                MessageBox.Show("有一个同名进程正在运行！", "程序冲突!", MessageBoxButton.OK);
                Close();
            }
            Title = "ACGManager Ver" + (Application.ResourceAssembly.GetName().Version.ToString());
            //设置默认启动Page页
            FrameContent.Content = Home_Page;
        }

        private void Home_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            FrameContent.Content = Home_Page;
        }

        private void GalGame_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        private void Game_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        private void Comic_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        private void Asmr_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        private void Anime_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        private void Search_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            FrameContent.Content = Search_Page;
        }
    }

}

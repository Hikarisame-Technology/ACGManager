using ACGManager.Pages.Search_Tools;
using iNKORE.UI.WPF.Modern.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using Frame = System.Windows.Controls.Frame;

namespace ACGManager.Pages
{
    /// <summary>
    /// Search.xaml 的交互逻辑
    /// </summary>
    public partial class Search : System.Windows.Controls.Page
    {
        private Frame RJ_Frame = new Frame() { Content = new RJ_Search() };
        private Frame Vndb_Frame = new Frame() { Content = new Vndb_Search() };

        public Search()
        {
            InitializeComponent();
            Search_FrameContent.Content = RJ_Frame;
        }

        private void RJ_Search_Page_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Search_FrameContent.Content = RJ_Frame;
        }

        private void Vndb_Search_Page_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Search_FrameContent.Content = Vndb_Frame;
        }

        private void Nyaa_Search_Page_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Aria2_Search_Page_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

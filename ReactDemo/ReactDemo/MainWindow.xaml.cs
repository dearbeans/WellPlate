using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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

namespace ReactDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>   
        /// 获取鼠标的坐标   
        /// </summary>   
        /// <param name="lpPoint">传址参数，坐标point类型</param>   
        /// <returns>获取成功返回真</returns>   
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetCursorPos(out POINT pt);
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWindowViewModel();
        }

   

        private void Button_AddRowClick(object sender, RoutedEventArgs e)
        {
            var a = this.DataContext as MainWindowViewModel;
            a.MyRowCount += 1;
        }

        private void Button_AddColumnClick(object sender, RoutedEventArgs e)
        {
            var a = this.DataContext as MainWindowViewModel;
            a.MyColumnCount += 1;
        }
        private void Button_SubRowClick(object sender, RoutedEventArgs e)
        {
            var a = this.DataContext as MainWindowViewModel;
            a.MyRowCount -= 1;
        }

        private void Button_SubColumnClick(object sender, RoutedEventArgs e)
        {
            var a = this.DataContext as MainWindowViewModel;
            a.MyColumnCount -= 1;
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            POINT p = new POINT();

            Point pp = Mouse.GetPosition(e.Source as FrameworkElement);//WPF方法
            Point ppp = (e.Source as FrameworkElement).PointToScreen(pp);//WPF方法

            if (GetCursorPos(out p))//API方法
            {
                this.Title =string.Format("GetCursorPos {0},{1}  GetPosition {2}   ,     {3}**********\r\n {4}   ,    {5}", p.X, p.Y, pp.X, pp.Y, ppp.X, ppp.Y);
                //MessageBox.Show(string.Format("GetCursorPos {0},{1}  GetPosition {2},{3}\r\n {4},{5}", p.X, p.Y, pp.X, pp.Y, ppp.X, ppp.Y));
            }
        }
    }
    public struct POINT
    {
        public int X;
        public int Y;
        public POINT(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}

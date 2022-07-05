using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;

namespace ReactDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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

    

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as MainWindowViewModel;
            var item = vm.WellCtrViewModel.Where(a => a.Label == txtb1.Text.Trim()).FirstOrDefault();
            if (item != null)
            {
                if (txtb2.Text == "0")
                {
                    item.Shape = ShapeType.Default;
                }
                //else if(txtb2.Text == "1")
                //{
                //    item.Shape = ShapeType.WithText;
                //}
                //else if (txtb2.Text == "2")
                //{
                //    item.Shape = ShapeType.WithText1;
                //}
                //item.Text = txtb2.Text;
               
            }
                

            //foreach (var item1 in vm.WellCtrViewModel)
            //{
            //    item1.IsChecked = !item1.IsChecked;
            //}

        }
    }
   
}

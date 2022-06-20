using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ReactDemo
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int MyRowCount { get; set; } = 5;
        public int MyColumnCount { get; set; } =5;
       public MainWindowViewModel()
        {
            //PropertyChanged += MainWindowViewModel_PropertyChanged;
        }

        private void MainWindowViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ReactDemo
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int MyRowCount { get; set; } = 3;
        public int MyColumnCount { get; set; } =4;

        public List<WellCtrViewModel> WellCtrViewModel { get; set; } 
        public MainWindowViewModel()
        {
          
            //this.CustomCtrViewModel = new CustomCtrViewModel();
            //PropertyChanged += MainWindowViewModel_PropertyChanged;
        }

        private void MainWindowViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }
    }
}

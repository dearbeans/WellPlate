using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ReactDemo
{
   public class CustomCtrViewModel:INotifyPropertyChanged
    {


        public List<WellCtrViewModel> WellCtrViewModel { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

using System.ComponentModel;
using System.Windows.Media;

namespace ReactDemo
{
    public class WellCtrViewModel : INotifyPropertyChanged
    {
        public int RowSortingIndex { get; set; }
        public int ColumnSortingIndex { get; set; }

        public string Label { get; set; }
        public string Text { get; set; }
        public bool IsChecked { get; set; }
        public Brush Background { get; set; }
        public ShapeType Shape { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
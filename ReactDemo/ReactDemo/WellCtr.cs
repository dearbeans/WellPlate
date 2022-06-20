using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ReactDemo
{
 
    public class WellCtr : ToggleButton
    {


        public int RowSortingIndex
        {
            get { return (int)GetValue(RowSortingIndexProperty); }
            set { SetValue(RowSortingIndexProperty, value); }
        }

        public static readonly DependencyProperty RowSortingIndexProperty =
            DependencyProperty.Register("RowSortingIndex", typeof(int), typeof(WellCtr), new PropertyMetadata(0));



        public int ColumnSortingIndex
        {
            get { return (int)GetValue(ColumnSortingIndexProperty); }
            set { SetValue(ColumnSortingIndexProperty, value); }
        }

        public static readonly DependencyProperty ColumnSortingIndexProperty =
            DependencyProperty.Register("ColumnSortingIndex", typeof(int), typeof(WellCtr), new PropertyMetadata(0));



        //选择框Border
        public Thickness SelectBoxBorderThickness
        {
            get { return (Thickness)GetValue(SelectBoxBorderThicknessProperty); }
            set { SetValue(SelectBoxBorderThicknessProperty, value); }
        }

        public static readonly DependencyProperty SelectBoxBorderThicknessProperty =
            DependencyProperty.Register("SelectBoxBorderThickness", typeof(Thickness), typeof(WellCtr), new PropertyMetadata(new Thickness(10)));



        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(WellCtr), new PropertyMetadata(string.Empty));



        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }


        static WellCtr()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WellCtr), new FrameworkPropertyMetadata(typeof(WellCtr)));
        }
    }
}

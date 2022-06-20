using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ReactDemo
{
    public class CustomCtr : Control
    {
        private const string PART_MAIN_GRID = "PART_MAIN_GRID";
        private Grid _mainGrid;
        private Canvas _partCanvas;

        private Border _currentBoxSelectedBorder = null;//拖动展示的提示框
        private bool _isCanMove = false;//鼠标是否移动
        private Point boxStartPoint;//起始坐标
        private List<WellCtr> _wellCtrS;//所有曹对象


        //组件内容边距Margin
        public Thickness ContentMargin
        {
            get { return (Thickness)GetValue(ContentMarginProperty); }
            set { SetValue(ContentMarginProperty, value); }
        }

        public static readonly DependencyProperty ContentMarginProperty =
            DependencyProperty.Register("ContentMargin", typeof(Thickness), typeof(CustomCtr), new FrameworkPropertyMetadata(new Thickness(10)));


        //选择框Border
        public Thickness SelectBoxBorderThickness
        {
            get { return (Thickness)GetValue(SelectBoxBorderThicknessProperty); }
            set { SetValue(SelectBoxBorderThicknessProperty, value); }
        }

        public static readonly DependencyProperty SelectBoxBorderThicknessProperty =
            DependencyProperty.Register("SelectBoxBorderThickness", typeof(Thickness), typeof(CustomCtr), new FrameworkPropertyMetadata(new Thickness(10), SelectBoxBorderThicknessPropertyChangedCallback));

        private static void SelectBoxBorderThicknessPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CustomCtr customCtr)
            {
                if (customCtr._mainGrid != null)
                {
                    customCtr.ClearMainGrid();
                    customCtr.OnApplyTemplate();
                }
            }
        }

        public Brush RowHeaderForeground
        {
            get { return (Brush)GetValue(RowHeaderForegroundProperty); }
            set { SetValue(RowHeaderForegroundProperty, value); }
        }

        public static readonly DependencyProperty RowHeaderForegroundProperty =
            DependencyProperty.Register("RowHeaderForeground", typeof(Brush), typeof(CustomCtr), new FrameworkPropertyMetadata(Brushes.LightGray, HeaderForegroundPropertyChangedCallback));

        private static void HeaderForegroundPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CustomCtr customCtr)
            {
                if (customCtr._mainGrid != null)
                {
                    customCtr.ClearMainGrid();
                    customCtr.OnApplyTemplate();
                }
            }
        }
        public Brush ColumnHeaderForeground
        {
            get { return (Brush)GetValue(ColumnHeaderForegroundProperty); }
            set { SetValue(ColumnHeaderForegroundProperty, value); }
        }

        public static readonly DependencyProperty ColumnHeaderForegroundProperty =
            DependencyProperty.Register("ColumnHeaderForeground", typeof(Brush), typeof(CustomCtr), new FrameworkPropertyMetadata(Brushes.LightGray, HeaderForegroundPropertyChangedCallback));

        public double RowHeaderFontSize
        {
            get { return (double)GetValue(RowHeaderFontSizeProperty); }
            set { SetValue(RowHeaderFontSizeProperty, value); }
        }

        public static readonly DependencyProperty RowHeaderFontSizeProperty =
            DependencyProperty.Register("RowHeaderFontSize", typeof(double), typeof(CustomCtr), new FrameworkPropertyMetadata(10d, RowHeaderFontSizePropertyChangedCallback));

        private static void RowHeaderFontSizePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CustomCtr customCtr)
            {
                if (customCtr._mainGrid != null)
                {
                    customCtr.ClearMainGrid();
                    customCtr.OnApplyTemplate();
                }
            }
        }

        public double ColumnHeaderFontSize
        {
            get { return (double)GetValue(ColumnHeaderFontSizeProperty); }
            set { SetValue(ColumnHeaderFontSizeProperty, value); }
        }

        public static readonly DependencyProperty ColumnHeaderFontSizeProperty =
            DependencyProperty.Register("ColumnHeaderFontSize", typeof(double), typeof(CustomCtr), new FrameworkPropertyMetadata(10d, ColumnHeaderFontSizePropertyChangedCallback));

        private static void ColumnHeaderFontSizePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CustomCtr customCtr)
            {
                if (customCtr._mainGrid != null)
                {
                    customCtr.ClearMainGrid();
                    customCtr.OnApplyTemplate();
                }
            }
        }

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(CustomCtr), new FrameworkPropertyMetadata(new CornerRadius(0, 0, 0, 0)));



        /// <summary>
        /// 是否开启多选模式
        /// </summary>
        public bool IsMultipleSelectMode
        {
            get { return (bool)GetValue(IsMultipleSelectModeProperty); }
            set { SetValue(IsMultipleSelectModeProperty, value); }
        }

        public static readonly DependencyProperty IsMultipleSelectModeProperty =
            DependencyProperty.Register("IsMultipleSelectMode", typeof(bool), typeof(CustomCtr), new FrameworkPropertyMetadata(true, IsMultipleSelectModeChangedCallback));

        private static void IsMultipleSelectModeChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            if (d is CustomCtr customCtr)
            {
                if (customCtr._wellCtrS == null)
                {
                    return;
                }
                else if (customCtr.IsMultipleSelectMode)
                {
                    foreach (var wellCtr in customCtr._wellCtrS)
                    {
                        wellCtr.AddHandler(WellCtr.PreviewMouseLeftButtonDownEvent, new RoutedEventHandler(customCtr.WellCtrPreviewMouseLeftButtonDown));
                        wellCtr.AddHandler(WellCtr.PreviewMouseUpEvent, new RoutedEventHandler(customCtr.WellCtrPreviewMouseUp));
                    }
                    customCtr._mainGrid.MouseLeftButtonDown += customCtr.SelectBox_MouseLeftButtonDown;
                }
                else
                {
                    foreach (var wellCtr in customCtr._wellCtrS)
                    {
                        wellCtr.RemoveHandler(WellCtr.PreviewMouseLeftButtonDownEvent, new RoutedEventHandler(customCtr.WellCtrPreviewMouseLeftButtonDown));
                        wellCtr.RemoveHandler(WellCtr.PreviewMouseUpEvent, new RoutedEventHandler(customCtr.WellCtrPreviewMouseUp));
                    }
                    customCtr._mainGrid.MouseLeftButtonDown -= customCtr.SelectBox_MouseLeftButtonDown;
                }
            }
        }


        public int RowCount
        {
            get { return (int)GetValue(RowCountProperty); }
            set { SetValue(RowCountProperty, value); }
        }

        public static readonly DependencyProperty RowCountProperty =
            DependencyProperty.Register("RowCount", typeof(int), typeof(CustomCtr), new FrameworkPropertyMetadata(0, RowCountPropertyChangedCallback) { BindsTwoWayByDefault = true });

        private static void RowCountPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CustomCtr customCtr)
            {
                if (customCtr._mainGrid != null)
                {
                    customCtr.ClearMainGrid();
                    customCtr.OnApplyTemplate();
                }
            }
        }

        public int ColumnCount
        {
            get { return (int)GetValue(ColumnCountProperty); }
            set { SetValue(ColumnCountProperty, value); }
        }

        public static readonly DependencyProperty ColumnCountProperty =
            DependencyProperty.Register("ColumnCount", typeof(int), typeof(CustomCtr), new FrameworkPropertyMetadata(0, ColumnCountPropertyChangedCallback) { BindsTwoWayByDefault = true });

        private static void ColumnCountPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            if (d is CustomCtr customCtr)
            {
                if (customCtr._mainGrid != null)
                {
                    customCtr.ClearMainGrid();
                    customCtr.OnApplyTemplate();
                }
            }
        }



        private void ClearMainGrid()
        {
            if (this.IsMultipleSelectMode)
            {
                _mainGrid.MouseLeftButtonDown -= SelectBox_MouseLeftButtonDown;
            }
            _mainGrid.Children.Clear();
            _mainGrid.RowDefinitions.Clear();
            _mainGrid.ColumnDefinitions.Clear();

        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _mainGrid = GetTemplateChild(PART_MAIN_GRID) as Grid;
            if (this.IsMultipleSelectMode)
            {
                _mainGrid.MouseLeftButtonDown += SelectBox_MouseLeftButtonDown;
            }
            InitMainGrid();
            InitHearder();
            InitWellCtr();
        }

        private void SelectBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_currentBoxSelectedBorder != null)
            {
                string curLable = string.Empty;

                //获取选框的矩形位置
                Point boxEndPoint = e.GetPosition(this._partCanvas);
                bool itemClickState = boxStartPoint.Equals(boxEndPoint);
                if (itemClickState)
                {
                    if (sender is WellCtr wellCtr)
                    {
                        curLable = wellCtr.Label;
                    }
                }

                Rect boxRect = new Rect(boxStartPoint, boxEndPoint);
                //获取子控件
                foreach (var child in _wellCtrS)
                {
                    var descendantBoundsRect = VisualTreeHelper.GetDescendantBounds(child);
                    //获取子控件矩形位置
                    GeneralTransform generalTransform = child.TransformToAncestor(_mainGrid);
                    Rect childRect = generalTransform.TransformBounds(descendantBoundsRect);
                    //点击按钮
                    if (itemClickState)
                    {
                        if (child.Label == curLable)
                        {
                            //child.IsChecked = !child.IsChecked;
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    //若子控件与选框相交则改变样式
                    if (boxRect.IntersectsWith(childRect))
                    {
                        child.IsChecked = true;
                    }
                }
                this._partCanvas.Children.Remove(_currentBoxSelectedBorder);
                _currentBoxSelectedBorder = null;

            }

            this._mainGrid.Children.Remove(_partCanvas);
            _partCanvas = null;
            _isCanMove = false;

            _mainGrid.MouseMove -= PartCanvas_MouseMove;
            _mainGrid.MouseUp -= SelectBox_MouseUp;
            RemoveWellCtrPreviewMouseMoveEventHandler();
        }

        public static List<T> GetChildObjects<T>(System.Windows.DependencyObject obj) where T : System.Windows.FrameworkElement
        {
            System.Windows.DependencyObject child = null;
            List<T> childList = new List<T>();
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);
                if (child is T)
                {
                    childList.Add((T)child);
                }
                childList.AddRange(GetChildObjects<T>(child));
            }
            return childList;
        }

        private void PartCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isCanMove)
            {
                Point tempEndPoint = e.GetPosition(this._partCanvas);
                //绘制跟随鼠标移动的方框
                DrawMultiSelectBorder(tempEndPoint, boxStartPoint);
            }
        }

        private void DrawMultiSelectBorder(Point endPoint, Point startPoint)
        {
            if (_currentBoxSelectedBorder == null)
            {
                _currentBoxSelectedBorder = new Border();
                _currentBoxSelectedBorder.Background = new SolidColorBrush(Colors.Orange);
                _currentBoxSelectedBorder.Opacity = 0.4;
                _currentBoxSelectedBorder.BorderThickness = new Thickness(2);
                _currentBoxSelectedBorder.BorderBrush = new SolidColorBrush(Colors.OrangeRed);
                Canvas.SetZIndex(_currentBoxSelectedBorder, 100);
                this._partCanvas.Children.Add(_currentBoxSelectedBorder);
            }
            _currentBoxSelectedBorder.Width = Math.Abs(endPoint.X - startPoint.X);
            _currentBoxSelectedBorder.Height = Math.Abs(endPoint.Y - startPoint.Y);
            if (endPoint.X - startPoint.X >= 0)
                Canvas.SetLeft(_currentBoxSelectedBorder, startPoint.X);
            else
                Canvas.SetLeft(_currentBoxSelectedBorder, endPoint.X);
            if (endPoint.Y - startPoint.Y >= 0)
                Canvas.SetTop(_currentBoxSelectedBorder, startPoint.Y);
            else
                Canvas.SetTop(_currentBoxSelectedBorder, endPoint.Y);

        }

        private void SelectBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            AddWellCtrPreviewMouseMoveEventHandler();
            _mainGrid.MouseMove += PartCanvas_MouseMove;
            _mainGrid.MouseUp += SelectBox_MouseUp;
            if (this._partCanvas != null)
            {
                if (_currentBoxSelectedBorder != null)
                {
                    this._partCanvas.Children.Remove(_currentBoxSelectedBorder);
                    _currentBoxSelectedBorder = null;
                }
                this._mainGrid.Children.Remove(_partCanvas);
                this._partCanvas = null;
            }
            _partCanvas = new Canvas();
            _partCanvas.Opacity = 0.5;
            //_partCanvas.MouseUp += _partCanvas_MouseUp;
            Panel.SetZIndex(_partCanvas, 100);
            Grid.SetColumn(_partCanvas, 0);
            Grid.SetRow(_partCanvas, 0);
            Grid.SetRow(_partCanvas, 0);
            Grid.SetColumnSpan(_partCanvas, this.ColumnCount + 1);
            Grid.SetRowSpan(_partCanvas, this.RowCount + 1);
            _mainGrid.Children.Add(_partCanvas);
            _isCanMove = true;
            boxStartPoint = e.GetPosition(this._partCanvas);

        }

        private void InitWellCtr()
        {
            for (int rowIndex = 1; rowIndex <= this.RowCount; rowIndex++)
            {
                for (int columnIndex = 1; columnIndex <= this.ColumnCount; columnIndex++)
                {
                    WellCtr wellCtr = new WellCtr();
                    if (this.IsMultipleSelectMode)
                    {
                        wellCtr.AddHandler(WellCtr.PreviewMouseLeftButtonDownEvent, new RoutedEventHandler(WellCtrPreviewMouseLeftButtonDown));
                        wellCtr.AddHandler(WellCtr.PreviewMouseUpEvent, new RoutedEventHandler(WellCtrPreviewMouseUp));
                    }

                    wellCtr.AddHandler(WellCtr.ClickEvent, new RoutedEventHandler(WellClick));
                    wellCtr.RowSortingIndex = (rowIndex - 1) *this.ColumnCount + columnIndex;//行排序索引
                    wellCtr.ColumnSortingIndex = (columnIndex - 1) * this.RowCount + rowIndex;//列排序索引
                    wellCtr.Label = $"{(char)(rowIndex + 64)}{columnIndex}";
                    wellCtr.SelectBoxBorderThickness = this.SelectBoxBorderThickness;
                    Grid.SetRow(wellCtr, rowIndex);
                    Grid.SetColumn(wellCtr, columnIndex);
                    _mainGrid.Children.Add(wellCtr);
                }
            }
            _wellCtrS = GetChildObjects<WellCtr>(this._mainGrid);
        }

        private void WellClick(object sender, RoutedEventArgs e)
        {
            if (sender is WellCtr wellCtr)
            {
                if (!IsMultipleSelectMode)
                {
                    foreach (var item in _wellCtrS)
                    {
                        if (item.Label == wellCtr.Label)
                        {
                            continue;
                        }
                        else
                        {
                            item.IsChecked = false;
                        }

                    }
                }
            }
        }

        private void WellCtrPreviewMouseUp(object sender, RoutedEventArgs e)
        {
            this.SelectBox_MouseUp(sender, e as MouseButtonEventArgs);
        }

        private void WellCtrPreviewMouseMove(object sender, RoutedEventArgs e)
        {
            this.PartCanvas_MouseMove(sender, e as MouseEventArgs);
        }

        private void WellCtrPreviewMouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            SelectBox_MouseLeftButtonDown(sender, e as MouseButtonEventArgs);
        }

        private void AddWellCtrPreviewMouseMoveEventHandler()
        {
            foreach (var wellCtr in _wellCtrS)
            {
                wellCtr.AddHandler(WellCtr.PreviewMouseMoveEvent, new RoutedEventHandler(WellCtrPreviewMouseMove));
            }
        }
        private void RemoveWellCtrPreviewMouseMoveEventHandler()
        {
            foreach (var wellCtr in _wellCtrS)
            {
                wellCtr.RemoveHandler(WellCtr.PreviewMouseMoveEvent, new RoutedEventHandler(WellCtrPreviewMouseMove));
            }
        }
        private void InitHearder()
        {
            for (int columnIndex = 1; columnIndex <= this.ColumnCount; columnIndex++)
            {
                TextBlock rowHearderText = new TextBlock();
                rowHearderText.HorizontalAlignment = HorizontalAlignment.Center;
                rowHearderText.VerticalAlignment = VerticalAlignment.Center;
                rowHearderText.FontSize = this.RowHeaderFontSize;
                rowHearderText.Foreground = this.RowHeaderForeground;
                if (this.IsMultipleSelectMode)
                {
                    rowHearderText.MouseDown += RowHearderText_MouseDown;
                }
                rowHearderText.Text = columnIndex.ToString();
                Grid.SetRow(rowHearderText, 0);
                Grid.SetColumn(rowHearderText, columnIndex);
                _mainGrid.Children.Add(rowHearderText);
            }
            for (int rowIndex = 1; rowIndex <= this.RowCount; rowIndex++)
            {
                TextBlock columnHearderText = new TextBlock();
                columnHearderText.HorizontalAlignment = HorizontalAlignment.Center;
                columnHearderText.VerticalAlignment = VerticalAlignment.Center;
                columnHearderText.FontSize = this.ColumnHeaderFontSize;
                columnHearderText.Foreground = this.ColumnHeaderForeground;
                if (this.IsMultipleSelectMode)
                {
                    columnHearderText.MouseDown += ColumnHearderText_MouseDown;
                }
                columnHearderText.Text = ((char)(rowIndex + 64)).ToString();
                Grid.SetRow(columnHearderText, rowIndex);
                Grid.SetColumn(columnHearderText, 0);
                _mainGrid.Children.Add(columnHearderText);
            }
        }

        private void ColumnHearderText_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBlock textBlock)
            {
                var rowHeaderText = textBlock.Text;

                for (int columnIndex = 1; columnIndex <= this.ColumnCount; columnIndex++)
                {
                    string columnHeaderText = columnIndex.ToString();
                    string label = string.Format("{0}{1}", rowHeaderText, columnHeaderText);
                    var wellCtr = _wellCtrS.FirstOrDefault(a => a.Label == label);
                    wellCtr.IsChecked = true;
                }
            }
        }

        private void RowHearderText_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBlock textBlock)
            {
                var columnHeaderText = textBlock.Text;

                for (int rowIndex = 1; rowIndex <= this.RowCount; rowIndex++)
                {
                    string rowHeaderText = ((char)(rowIndex + 64)).ToString();
                    string label = string.Format("{0}{1}", rowHeaderText, columnHeaderText);
                    var wellCtr = _wellCtrS.FirstOrDefault(a => a.Label == label);
                    wellCtr.IsChecked = true;
                }
            }
        }

        private void InitMainGrid()
        {
            for (int rowIndex = 0; rowIndex <= this.RowCount; rowIndex++)
            {
                RowDefinition row = new RowDefinition();
                if (rowIndex == 0)
                {
                    row.Height = GridLength.Auto;
                }
                _mainGrid.RowDefinitions.Add(row);
            }
            for (int columnIndex = 0; columnIndex <= this.ColumnCount; columnIndex++)
            {
                ColumnDefinition column = new ColumnDefinition();
                if (columnIndex == 0)
                {
                    column.Width = GridLength.Auto;
                }
                _mainGrid.ColumnDefinitions.Add(column);
            }
        }

        public CustomCtr()
        {
        }

        static CustomCtr()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomCtr), new FrameworkPropertyMetadata(typeof(CustomCtr)));
            BackgroundProperty.OverrideMetadata(typeof(CustomCtr), new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromRgb(178, 241, 255))));
        }
    }
}

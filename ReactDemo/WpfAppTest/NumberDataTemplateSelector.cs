using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfAppTest
{
    public class NumberDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate NumberTemplate { get; set; }
        public DataTemplate LargeNumberTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            // Null value can be passed by IDE designer
            if (item == null) return null;

            var num = Convert.ToInt32((string)item);

            // Select one of the DataTemplate objects, based on the 
            // value of the selected item in the ComboBox.
            if (num < 5)
            {
                return NumberTemplate;
            }
            else
            {
                return LargeNumberTemplate;
            }
        }
    }
}

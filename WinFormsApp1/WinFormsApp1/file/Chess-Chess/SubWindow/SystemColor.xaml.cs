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
using System.Windows.Shapes;

namespace Chess.SubWindow
{
    /// <summary>
    /// SystemColor.xaml 的交互逻辑
    /// </summary>
    public partial class SystemColor : Window
    {
        private SortedList<string, string> colorsSortByValue = new SortedList<string, string>();
        private SortedList<string, string> colorsSortByName = new SortedList<string, string>();
        public SystemColor()
        {
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Type type = typeof(Brushes);
            System.Reflection.PropertyInfo[] info = type.GetProperties();
            foreach (System.Reflection.PropertyInfo pi in info)
            {
                string colorName = pi.Name;
                colorStackPanel.Children.Add(new CustomClass.ColorListItem(colorName));
                Color color = (Color)ColorConverter.ConvertFromString(colorName);
                string colorvalue = color.ToString();
                
                if (!colorsSortByName.ContainsKey(colorName)) colorsSortByName.Add(colorName, colorvalue);
                if (!colorsSortByValue.ContainsKey(colorvalue)) colorsSortByValue.Add(colorvalue, colorName);
                //int Gray = (int)Math.Sqrt(.241 * color.R + .691 * color.G + .068 * color.B);
                //int Gray = (int)(299 * color.R + 587 * color.G + 114 * color.B+500)/1000; // 灰度计算公式，著名的心理学公式
                //int Gray = (19595 * color.R + 38469 * color.G + 7472 * color.B) >> 16; // 灰度的快速算法，移位比除法快
                //if (!colorsSortByValue.ContainsKey(Gray.ToString())) colorsSortByValue.Add(Gray.ToString(), colorName);

            }
        }

        private void SortByValue(object sender, RoutedEventArgs e)
        {
            colorStackPanel.Children.Clear();
            foreach (string key in colorsSortByValue.Keys)
            {
                colorStackPanel.Children.Add(new CustomClass.ColorListItem(colorsSortByValue[key]));
            }
        }
        private void SortByName(object sender, RoutedEventArgs e)
        {
            colorStackPanel.Children.Clear();
            foreach (string colorname in colorsSortByName.Keys)
            {
                colorStackPanel.Children.Add(new CustomClass.ColorListItem(colorname));
            }
        }
    }

}

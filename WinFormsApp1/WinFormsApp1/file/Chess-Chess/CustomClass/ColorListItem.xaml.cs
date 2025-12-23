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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Chess.CustomClass
{
    /// <summary>
    /// ColorListItem.xaml 的交互逻辑
    /// 本控件包含一个WrapperPanel容器，容器内有三个控件：TextBlock（颜色名称）、Border（色板）、TextBlock（十六进制颜色值）
    /// </summary>
    public partial class ColorListItem : UserControl
    {
        public ColorListItem()
        {
            InitializeComponent();
        }
        public ColorListItem(string colorname)
        {
            InitializeComponent();
            ColorNameTextBlock.Text = colorname;
        }
    }
}

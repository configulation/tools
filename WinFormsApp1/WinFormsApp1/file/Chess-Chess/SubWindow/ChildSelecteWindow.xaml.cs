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
    /// ChildSelecteWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ChildSelecteWindow : Window
    {
        public ChildSelecteWindow(int childCount)
        {
            InitializeComponent();
            for (int i = 0; i < childCount; i++)
            {
                Button btn = new();
                btn.Content = (i+1).ToString();
                btn.Width = 30;
                btn.Height = 30;
                btn.Margin = new Thickness(20,0,0,0);
                btn.Click += Btn_Click;
                //btn.Visibility = Visibility.Visible;
                wrapPanel.Children.Add(btn);

            }
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            Button button=sender as Button;
            Clipboard.Clear();
            Clipboard.SetText(button.Content.ToString());
            Close();
        }
    }
}

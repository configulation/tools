using System.Windows.Controls;

namespace Chess.CustomClass
{
    /// <summary>
    /// JueSha.xaml 的交互逻辑
    /// </summary>
    public partial class JueSha : UserControl
    {
        public JueSha()
        {
            InitializeComponent();
            jue1.Opacity = 0;
            jue5.Opacity = 0;
            sha1.Opacity = 0;
            sha5.Opacity = 0;
            tuoba.Opacity = 0;
            mainGrid.Opacity = 0;
        }
    }
}

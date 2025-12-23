using System.Collections.ObjectModel;
using System.Windows;

namespace Chess
{
    /// <summary>
    /// SpyWindow.xaml 的交互逻辑
    /// </summary>

    public partial class SpyWindow : Window
    {
        private static ObservableCollection<QiPanCols> obserArray;
        public SpyWindow()
        {
            InitializeComponent();
            Left = SystemParameters.WorkArea.Left + 60;
            Top = SystemParameters.WorkArea.Top + 200;

            obserArray = new();
            SpyQipan.ItemsSource = obserArray;
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            obserArray.Clear();
            for (int j = 0; j < 10; j++)
            {
                QiPanCols item = new();
                item.Id = j;
                item.Col0 = (GlobalValue.QiPan[0, j] == -1) ? "" : GlobalValue.QiPan[0, j].ToString();
                item.Col1 = (GlobalValue.QiPan[1, j] == -1) ? "" : GlobalValue.QiPan[1, j].ToString();
                item.Col2 = (GlobalValue.QiPan[2, j] == -1) ? "" : GlobalValue.QiPan[2, j].ToString();
                item.Col3 = (GlobalValue.QiPan[3, j] == -1) ? "" : GlobalValue.QiPan[3, j].ToString();
                item.Col4 = (GlobalValue.QiPan[4, j] == -1) ? "" : GlobalValue.QiPan[4, j].ToString();
                item.Col5 = (GlobalValue.QiPan[5, j] == -1) ? "" : GlobalValue.QiPan[5, j].ToString();
                item.Col6 = (GlobalValue.QiPan[6, j] == -1) ? "" : GlobalValue.QiPan[6, j].ToString();
                item.Col7 = (GlobalValue.QiPan[7, j] == -1) ? "" : GlobalValue.QiPan[7, j].ToString();
                item.Col8 = (GlobalValue.QiPan[8, j] == -1) ? "" : GlobalValue.QiPan[8, j].ToString();
                obserArray.Add(item);
            }
            SpyQipan.Items.Refresh();

        }
        public class QiPanCols
        {
            public int Id { get; set; }
            public string Col0 { get; set; }
            public string Col1 { get; set; }
            public string Col2 { get; set; }
            public string Col3 { get; set; }
            public string Col4 { get; set; }
            public string Col5 { get; set; }
            public string Col6 { get; set; }
            public string Col7 { get; set; }
            public string Col8 { get; set; }

        }

        private void DataRefresh(object sender, RoutedEventArgs e)
        {
            obserArray.Clear();
            for (int j = 0; j < 10; j++)
            {
                QiPanCols item = new();
                item.Id = j;
                item.Col0 = (GlobalValue.QiPan[0, j] == -1) ? "" : GlobalValue.QiPan[0, j].ToString();
                item.Col1 = (GlobalValue.QiPan[1, j] == -1) ? "" : GlobalValue.QiPan[1, j].ToString();
                item.Col2 = (GlobalValue.QiPan[2, j] == -1) ? "" : GlobalValue.QiPan[2, j].ToString();
                item.Col3 = (GlobalValue.QiPan[3, j] == -1) ? "" : GlobalValue.QiPan[3, j].ToString();
                item.Col4 = (GlobalValue.QiPan[4, j] == -1) ? "" : GlobalValue.QiPan[4, j].ToString();
                item.Col5 = (GlobalValue.QiPan[5, j] == -1) ? "" : GlobalValue.QiPan[5, j].ToString();
                item.Col6 = (GlobalValue.QiPan[6, j] == -1) ? "" : GlobalValue.QiPan[6, j].ToString();
                item.Col7 = (GlobalValue.QiPan[7, j] == -1) ? "" : GlobalValue.QiPan[7, j].ToString();
                item.Col8 = (GlobalValue.QiPan[8, j] == -1) ? "" : GlobalValue.QiPan[8, j].ToString();
                obserArray.Add(item);
            }
            //SpyQipan.Items.Refresh();
        }
    }
}

using System.Windows;
using Newtonsoft.Json;
using Chess.DataClass;
using Chess.OpenSource;
using System.Data;
using Chess.CustomClass;

namespace Chess.SubWindow
{
    /// <summary>
    /// Save_Window.xaml 的交互逻辑
    /// 保存棋谱窗口
    /// </summary>
    public partial class Save_Window : Window
    {
        public Save_Window()
        {
            InitializeComponent();
            qipustr.Text = Qipu.CnToString();
            DataTable dt = SqliteHelper.Select("mybook", "author");
            dt = new DataView(dt).ToTable(true, "author"); // 去除重复数据
            author.DisplayMemberPath = "author";
            author.ItemsSource = dt.DefaultView; // 作者下拉列表

            dt = SqliteHelper.Select("mybook", "type");
            dt = new DataView(dt).ToTable(true, "type"); // 去除重复数据
            type.DisplayMemberPath = "type";
            type.ItemsSource = dt.DefaultView; // 类型下拉列表

            dt = SqliteHelper.Select("mybook", "title");
            dt = new DataView(dt).ToTable(true, "title"); // 去除重复数据
            title.DisplayMemberPath = "title";
            title.ItemsSource = dt.DefaultView; // 标题下拉列表

        }

        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            QiPuBook book = new();
            book.author = author.Text;
            book.date = date.DisplayDate;
            book.type = type.Text;
            book.title = title.Text;
            book.video = videoLink.Text;
            book.memo = memoText.Text;
            book.record = Qipu.CnToString();
            GlobalValue.qiPuSimpleRecordRoot = GlobalValue.ConvertQiPuToSimple(GlobalValue.qiPuRecordRoot);  // 更新简易棋谱记录
            book.jsonrecord = JsonConvert.SerializeObject(GlobalValue.qiPuSimpleRecordRoot);
            _ = SqliteHelper.Insert("mybook", book.getDictionary());
            Close();
        }
    }
}

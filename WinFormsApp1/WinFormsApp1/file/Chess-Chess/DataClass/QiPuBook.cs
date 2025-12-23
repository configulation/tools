using System;
using System.Collections.Generic;

namespace Chess.DataClass
{
    internal class QiPuBook
    {
        public int RowID { get; set; }
        public DateTime date { get; set; }
        public string type { get; set; }
        public string title { get; set; }
        public string author { get; set; }
        public string video { get; set; }
        public string memo { get; set; }
        public string record { get; set; }
        public string jsonrecord { get; set; }
        public Dictionary <string,object> getDictionary()
        {
            Dictionary<string, object> dic = new();
            dic.Add("date", date);
            dic.Add("type", type);
            dic.Add("title", title);
            dic.Add("author", author);
            dic.Add("video", video);
            dic.Add("memo", memo);
            dic.Add("record", record);
            dic.Add("jsonrecord",jsonrecord);
            return dic;
  
        }
    }
}

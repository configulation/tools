using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    /// <summary>
    /// 属性变化事件测试模块
    /// </summary>
    internal class PropertyChangedClass
    {
        private static bool _name;
        public static bool Name { 
            get { return _name; } 
            set {
                _name = value;
                OnPropertyRaised(nameof(Name));
            } }
        public string Description { get; set; }


        public static event EventHandler<PropertyChangedEventArgs> PropertyChanged;
        private static void OnPropertyRaised(string propertyname)
        {
            PropertyChanged?.Invoke(null, new PropertyChangedEventArgs(propertyname));
        }
    }
}

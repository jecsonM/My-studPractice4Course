using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfLaundrySystemApp.Attributes
{
    //[AttributeUsage(AttributeTargets.Property)]
    //public class HiddenColumnAttribute : Attribute
    //{
    //}


    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DisplayNameAttribute : Attribute
    {
        public string DisplayName { get; }
        public bool Visible { get; set; } = true; // Видимость колонки

        public DisplayNameAttribute(string displayName)
        {
            DisplayName = displayName;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfLaundrySystemApp.Attributes
{
    


    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DisplayBehaviourAttribute : Attribute
    {
        public string DisplayName { get; }
        public bool Visible { get; set; } = true; // Видимость колонки
        public bool IsIncludeRequired { get; set; } = false; // Необходим ли Include по ключу
        public bool IsSeeAllButtonRequired { get; set; } = false; // Необходима ли кнопка просмотра всех зависимостей

        public DisplayBehaviourAttribute(string displayName)
        {
            DisplayName = displayName;
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DisplayClassNameAttribute : Attribute
    {
        public string DisplayName { get; }

        public DisplayClassNameAttribute(string displayName)
        {
            DisplayName = displayName;
        }
    }
}

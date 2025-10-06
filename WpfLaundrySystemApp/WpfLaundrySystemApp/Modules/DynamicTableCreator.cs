using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;    
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WpfLaundrySystemApp.Attributes;
using System.CodeDom;

namespace WpfLaundrySystemApp.Modules
{


    public class DynamicTableCreator
    {
        

        static public DataGrid GenerateTable<T>(IEnumerable<T> data)
        {

            DataGrid dataGrid = new DataGrid
            {
                AutoGenerateColumns = false,
                CanUserAddRows = false
            };

            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                if (property.CanRead)
                {
                    var column = CreateDataGridColumn(property);
                    if (column != null) 
                    {
                        dataGrid.Columns.Add(column);
                    }
                }
            }

            dataGrid.ItemsSource = data;

            
            return dataGrid;
        }

        static private DataGridColumn CreateDataGridColumn(PropertyInfo property)
        {
            DataGridColumn column;

            var displayNameAttribute = property.GetCustomAttribute<DisplayNameAttribute>();
            if (displayNameAttribute == null || !displayNameAttribute.Visible)
            {
                return null; // Возвращаем null для пропуска колонки
            }


            // Выбираем тип колонки в зависимости от типа свойства
            

            if (property.PropertyType == typeof(bool) ||
                property.PropertyType == typeof(bool?))
            {
                column = new DataGridCheckBoxColumn();
            }
            else if (property.PropertyType == typeof(DateTime) ||
                     property.PropertyType == typeof(DateTime?))
            {
                column = new DataGridTextColumn();
                // Можно добавить форматирование даты
            }
            else
            {
                column = new DataGridTextColumn();
            }

            // Настраиваем биндинг
            if (column is DataGridBoundColumn boundColumn)
            {
                boundColumn.Binding = new Binding(property.Name)
                {
                    Mode = BindingMode.OneWay
                };
            }

            // Устанавливаем заголовок
            column.Header = displayNameAttribute.DisplayName;

            // Дополнительные настройки
            column.IsReadOnly = !property.CanWrite;

            return column;
        }

        
    }
}

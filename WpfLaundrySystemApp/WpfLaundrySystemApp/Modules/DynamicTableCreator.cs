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
using WpfLaundrySystemApp.DBContext;

using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WpfLaundrySystemApp.Models;

namespace WpfLaundrySystemApp.Modules
{


    public class DynamicTableCreator : IDisposable
    {
        private DbContext db;

        public Type TypeOfTheDynamicallyCreatedTable { get; set; }

        public List<object> AdditionalPKConstraints = null;

        public DataGrid DataGrid { get; set; }

        
        public RoutedEventHandler SeeAllButton;

        public DynamicTableCreator(Microsoft.EntityFrameworkCore.DbContext dbContext, RoutedEventHandler routedEventHandler)
        {
            db = dbContext;
            SeeAllButton = routedEventHandler;
        }

        public DataGrid GenerateTable()
        {

            

            IEnumerable<dynamic> data = GetData();


            DataGrid dataGrid = new DataGrid
            {
                AutoGenerateColumns = false,
                CanUserAddRows = false
            };

            PropertyInfo[] properties = TypeOfTheDynamicallyCreatedTable.GetProperties(BindingFlags.Public | BindingFlags.Instance);

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

            DataGrid = dataGrid;


            return dataGrid;
        }

        private IEnumerable<object> GetData()
        {
            //Тип нашего дбКонтекста
            Type dbType = db.GetType();

            //ДБсет пропертя инфо
            PropertyInfo dbSetProperty = dbType.GetProperties()
            .Where(p => p.PropertyType.IsGenericType &&
                       p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>) &&
                       p.PropertyType.GetGenericArguments()[0] == TypeOfTheDynamicallyCreatedTable)
            .FirstOrDefault();

            if (dbSetProperty == null)
            {
                throw new InvalidOperationException($"DbSet<{TypeOfTheDynamicallyCreatedTable.Name}> not found in DbContext");
            }

            //ДБсет значение
            object dbSet = dbSetProperty.GetValue(db); // DbSet<T>

            
            object includedQuery = getRequiredIncludes(dbSet);


            object wheredQuery = getRequiredWheres(includedQuery);

            // Получаем метод ToList через рефлексию
            MethodInfo toListMethod = typeof(Enumerable)
                .GetMethod("ToList")
                .MakeGenericMethod(TypeOfTheDynamicallyCreatedTable);

             


            // Вызываем ToList (аналогично .ToList())
            return (IEnumerable<object>)toListMethod.Invoke(null, new object[] { wheredQuery });
        }

        public static List<string> GetKeyProperties(DbContext dbContext, Type entityType)
        {
            var entityEntry = dbContext.Entry(dbContext.Model.FindEntityType(entityType).ClrType);
            var keyProperties = dbContext.Model.FindEntityType(entityType)
                .FindPrimaryKey()
                .Properties
                .Select(p => p.Name)
                .ToList();

            return keyProperties;
        }

        private object getRequiredWheres(object query)
        {
            if (AdditionalPKConstraints == null) return query;

            List<string> keyProperties = GetKeyProperties(db, TypeOfTheDynamicallyCreatedTable);

            PropertyInfo[] propertiesPK = TypeOfTheDynamicallyCreatedTable.GetProperties()
                .Where(
                    p => keyProperties.Contains(p.Name)
                )
                .ToArray();


            MethodInfo findMethod = query.GetType()
               .GetMethods()
               .Where(m => m.Name == "Find" &&
                          m.IsGenericMethod &&
                          m.GetParameters().Length > 0)
               .FirstOrDefault();

            MethodInfo genericFindMethod = findMethod.MakeGenericMethod(TypeOfTheDynamicallyCreatedTable);

            // Вызываем Find с переданными значениями первичного ключа
            object result = genericFindMethod.Invoke(query, new object[] { AdditionalPKConstraints });

            return result;
        }
        private object getRequiredIncludes(object dbSet)
        {

            PropertyInfo[] propertiesToJoin = TypeOfTheDynamicallyCreatedTable.GetProperties()
                .Where(
                    p => p.GetCustomAttribute<DisplayBehaviourAttribute>().IsIncludeRequired
                )
                .ToArray();
            object objToreturn = dbSet;
            foreach (PropertyInfo property in propertiesToJoin)
            {

                

                //метод include
                MethodInfo includeMethod = typeof(EntityFrameworkQueryableExtensions)
                .GetMethods()
                .Where(m => m.Name == "Include" &&
                           m.IsGenericMethod &&
                           m.GetParameters().Length == 2)
                .First()
                .MakeGenericMethod(TypeOfTheDynamicallyCreatedTable, property.PropertyType);

                
                ParameterExpression parameter = System.Linq.Expressions.Expression.Parameter(TypeOfTheDynamicallyCreatedTable, "p");
                



                MemberExpression memberExpression = System.Linq.Expressions.Expression.Property(parameter, property);
                LambdaExpression lambdaExpression = System.Linq.Expressions.Expression.Lambda(memberExpression, parameter);

                // Вызываем Include (аналогично .Include(p => p.PartnerType))
                objToreturn = includeMethod.Invoke(null, new object[] { objToreturn, lambdaExpression });
            }

            return objToreturn;
        }

        private DataGridColumn CreateDataGridColumn(PropertyInfo property)
        {
            DataGridColumn column;

            DisplayBehaviourAttribute displayNameAttribute = property.GetCustomAttribute<DisplayBehaviourAttribute>();
            if (displayNameAttribute == null || !displayNameAttribute.Visible)
                return null; // Возвращаем null для пропуска колонки
            


            // Выбираем тип колонки в зависимости от типа свойства
            

            if (property.PropertyType == typeof(bool) ||
                property.PropertyType == typeof(bool?))
                column = new DataGridCheckBoxColumn();
            else if (property.PropertyType == typeof(DateTime) ||
                     property.PropertyType == typeof(DateTime?))
            {
                // Добавить форматирование даты
                column = new DataGridTextColumn();
            }
            else if (displayNameAttribute.IsSeeAllButtonRequired)
            {
                //Вставляем кнопку
                column = CreateButtonColumn(property.Name, property.PropertyType, displayNameAttribute.DisplayName);
            }
            else
                column = new DataGridTextColumn();


            // Настраиваем биндинг
            if (column is DataGridBoundColumn boundColumn && !displayNameAttribute.IsSeeAllButtonRequired)
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

        public string GetTitle()
        {
            DisplayClassNameAttribute displayClassNameAttribute = TypeOfTheDynamicallyCreatedTable.GetCustomAttributes<DisplayClassNameAttribute>(false).FirstOrDefault();
            if (displayClassNameAttribute == null) return TypeOfTheDynamicallyCreatedTable.ToString();
            return displayClassNameAttribute.DisplayName;
        }

        private DataGridTemplateColumn CreateButtonColumn(string propertyName, Type propertyType, string header)
        {
            var templateColumn = new DataGridTemplateColumn();

            
            templateColumn.Header = header;

            // Создаем DataTemplate для ячейки
            var cellTemplate = new DataTemplate();

            // Создаем FrameworkElementFactory для кнопки
            var buttonFactory = new FrameworkElementFactory(typeof(Button));

            buttonFactory.SetValue(Button.ContentProperty, "Просмотреть");
            buttonFactory.SetValue(Button.CommandProperty, new Binding("ViewCommand"));
            buttonFactory.SetValue(Button.CommandParameterProperty, new Binding(propertyName));
            buttonFactory.SetValue(Button.TagProperty, propertyType);


            buttonFactory.AddHandler(Button.ClickEvent, SeeAllButton);

            // Устанавливаем фабрику в VisualTree
            cellTemplate.VisualTree = buttonFactory;
            templateColumn.CellTemplate = cellTemplate;

            return templateColumn;
        }

        public void Dispose()
        {
            db?.Dispose();
        }
    }
}

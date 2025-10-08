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
using System.Diagnostics.CodeAnalysis;
using WpfLaundrySystemApp.Windows;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Windows.Media.Animation;

namespace WpfLaundrySystemApp.Modules
{


    public class DynamicTableCreator : IDisposable
    {
        private DbContext db;

        public Type TypeOfTheDynamicallyCreatedTable { get; set; }

        //public List<object> AdditionalPKConstraints { get; set; } = null;

        public Dictionary<PropertyInfo, object> WhereArguments { get; set; } = null;

        public DataGrid DataGrid { get; set; }

        public RoutedEventHandler EditMenuItem_Click { get; set; }
        public RoutedEventHandler DeleteMenuItem_Click { get; set; }
        public RoutedEventHandler AddMenuItem_Click { get; set; }


        public RoutedEventHandler SeeAllButton { get; set; }

        public DynamicTableCreator(Microsoft.EntityFrameworkCore.DbContext dbContext)
        {
            db = dbContext;
        }

        private class ProperyInfoComparerByNames : IEqualityComparer<PropertyInfo>
        {
            
            public bool Equals(PropertyInfo? x, PropertyInfo? y)
            {
                return string.Equals(x.Name, y.Name);
                
            }

            public int GetHashCode([DisallowNull] PropertyInfo obj)
            {
                return obj.Name.GetHashCode();
            }
        }

        public void SaveChanges()
        {
            db.SaveChanges();
        }

        public void ReCreateDbContext()
        {
            Type typeOfDBContext = db.GetType();

            db.Dispose();
            
            db = Activator.CreateInstance(typeOfDBContext) as DbContext;


        }
        public void SetNewTableToGenerate(Type typeToChange, Dictionary<PropertyInfo,object> whereArguments)
        {

            ReCreateDbContext();

            WhereArguments = whereArguments;
            TypeOfTheDynamicallyCreatedTable = typeToChange;
        }

        public void SetNewTableToGenerate(Type typeToChange, object getPKContraintsFrom = null)
        {

            ReCreateDbContext();


            if (getPKContraintsFrom is null)
            {
                WhereArguments = null;
            }
            
            
            Type typeOfAdditionalPK = getPKContraintsFrom.GetType();


            PropertyInfo[] pkProperties = GetPrimaryKeyProperties(typeOfAdditionalPK);


            
            PropertyInfo[] fkProperties = GetForeignKeyProperties(typeToChange, TypeOfTheDynamicallyCreatedTable);


            WhereArguments = new Dictionary<PropertyInfo, object>(fkProperties.Length);

            for (int i = 0; i < fkProperties.Length; i++)
            {
                WhereArguments[fkProperties[i]] = 
                    pkProperties
                    .Where(p => p.Name == fkProperties[i].Name)
                    .Select(p => p.GetValue(getPKContraintsFrom))
                    .FirstOrDefault();
            }

            TypeOfTheDynamicallyCreatedTable = typeToChange;
        }

        public DataGrid GenerateTable()
        {

            

            IEnumerable<dynamic> data = GetData();


            DataGrid dataGrid = new DataGrid
            {
                AutoGenerateColumns = false,
                CanUserAddRows = false,
                ContextMenu = CreateContextMenu()
            };

            PropertyInfo[] properties = TypeOfTheDynamicallyCreatedTable.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo property in properties)
            {
                if (property.CanRead)
                {
                    DataGridColumn column = CreateDataGridColumn(property);
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


        public ContextMenu CreateContextMenu()
        {
            ContextMenu contextMenu = new ContextMenu();

            MenuItem addItem = new MenuItem();
            addItem.Header = "Добавить";
            addItem.Click += AddMenuItem_Click;

            MenuItem editItem = new MenuItem();
            editItem.Header = "Редактировать";
            editItem.Click += EditMenuItem_Click;

            Separator separator1 = new Separator();

            MenuItem deleteItem = new MenuItem();
            deleteItem.Header = "Удалить";
            deleteItem.Click += DeleteMenuItem_Click;

            Separator separator2 = new Separator();


            contextMenu.Items.Add(addItem);
            contextMenu.Items.Add(separator1);
            contextMenu.Items.Add(editItem);
            contextMenu.Items.Add(separator2);
            contextMenu.Items.Add(deleteItem);

            return contextMenu;
        }

        public bool TryRemovingObject(object whatToRemove)
        {
            try
            {
                db.Remove(whatToRemove);

            }
            catch
            {
                return false;
            }


            return true;
        }

        public bool TryAddingNewObject(object whatToAdd)
        {
            try
            {
                db.Add(whatToAdd);

            }
            catch
            {
                return false;
            }
            

            return true;
        }

        public IEnumerable<object> GetData()
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


            object wheredQuery = getRequiredWhere(includedQuery, WhereArguments);

            // Получаем метод ToList через рефлексию
            MethodInfo toListMethod = typeof(Enumerable)
                .GetMethod("ToList")
                .MakeGenericMethod(TypeOfTheDynamicallyCreatedTable);

             


            // Вызываем ToList (аналогично .ToList())
            return (IEnumerable<object>)toListMethod.Invoke(null, new object[] { wheredQuery });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityTypeThatReferences">Модель в которое находятся FK</param>
        /// <param name="entityTypeToReference">Модель на которую ссылаются FK</param>
        /// <returns>Проперти класса entityTypeThatReferences, которые ссылаются на entityType</returns>
        public PropertyInfo[] GetForeignKeyProperties(Type entityTypeThatReferences)
        {
            IEntityType? entityTypeThatContainsFK = db.Model.FindEntityType(entityTypeThatReferences);

            if (entityTypeThatContainsFK == null)
                return new PropertyInfo[0];

            List<string> foreignKeyPropertyNames = entityTypeThatContainsFK
                .GetForeignKeys()
                .SelectMany(fk => fk.Properties)
                .Select(p => p.Name)
                .Distinct()
                .ToList();

            return entityTypeThatReferences.GetProperties().Where(p => foreignKeyPropertyNames.Contains(p.Name)).ToArray(); ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityTypeThatReferences">Модель в которое находятся FK</param>
        /// <param name="entityTypeToReference">Модель на которую ссылаются FK</param>
        /// <returns>Проперти класса entityTypeThatReferences, которые ссылаются на entityType</returns>
        public PropertyInfo[] GetForeignKeyProperties(Type entityTypeThatReferences, Type entityTypeToReference)
        {
            IEntityType? entityTypeThatIsBeingReferenced = db.Model.FindEntityType(entityTypeToReference);
            IEntityType? entityTypeThatContainsFK = db.Model.FindEntityType(entityTypeThatReferences);

            if (entityTypeThatContainsFK == null || entityTypeThatIsBeingReferenced == null)
                return new PropertyInfo[0];

            List<string> foreignKeyPropertyNames = entityTypeThatContainsFK
                .GetForeignKeys()
                .Where(fk => fk.PrincipalEntityType?.ClrType == entityTypeToReference)
                .SelectMany(fk => fk.Properties)
                .Select(p => p.Name)
                .Distinct()
                .ToList();

            return entityTypeThatReferences.GetProperties().Where(p => foreignKeyPropertyNames.Contains(p.Name)).ToArray(); ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityTypeThatReferences">Модель в которое находятся FK</param>
        /// <param name="entityTypeToReference">Модель на которую ссылаются FK</param>
        /// <returns>Словарь с ключами FK entityTypeThatReferences и значениями PK entityTypeToReference</returns>
        public Dictionary<PropertyInfo, PropertyInfo> GetFK_PK_pairsProperties(Type entityTypeThatReferences, Type entityTypeToReference)
        {
            IEntityType? entityTypeThatIsBeingReferenced = db.Model.FindEntityType(entityTypeToReference);
            IEntityType? entityTypeThatContainsFK = db.Model.FindEntityType(entityTypeThatReferences);

            if (entityTypeThatContainsFK == null || entityTypeThatIsBeingReferenced == null)
                return new Dictionary<PropertyInfo, PropertyInfo>();

            return entityTypeThatContainsFK
                .GetForeignKeys()
                .Where(fk => fk.PrincipalEntityType?.ClrType == entityTypeToReference)
                .SelectMany(
                    fk => fk.Properties
                        .Select(
                            (fkProp, index) => new
                            {
                                FKProperty = fkProp,
                                PKProperty = fk.PrincipalKey.Properties.ElementAtOrDefault(index)
                            }
                        )
                )
                .Where(pair => pair.PKProperty != null)
                .ToDictionary(
                    pair => entityTypeThatReferences.GetProperty(pair.FKProperty.Name),
                    pair => entityTypeToReference.GetProperty(pair.PKProperty.Name)
                )
                .Where(kvp => kvp.Key != null && kvp.Value != null)
                .ToDictionary(kvp => kvp.Key!, kvp => kvp.Value!);
        }

        public PropertyInfo[] GetPrimaryKeyProperties(Type entityType)
        {


            IEntityType iEntityType = db.Model.FindEntityType(entityType);
            List<string> keyPropertyNames = new List<string>();
            if ( !(iEntityType is null) )
                keyPropertyNames = iEntityType.FindPrimaryKey().Properties.Select(p => p.Name).ToList();

            return entityType.GetProperties().Where(p => keyPropertyNames.Contains(p.Name)).ToArray();
        }


        private object getRequiredWhere(object query, Dictionary<PropertyInfo,object> whereArguments)
        {
            //if (AdditionalPKConstraints == null) return query;

            

            if (whereArguments == null || whereArguments.Count == 0)
                return query;

            object objToReturn = query;

            foreach (KeyValuePair<PropertyInfo,object> kvp in whereArguments)
            {
                PropertyInfo property = kvp.Key;
                object value = kvp.Value;

                // Получаем метод Where
                MethodInfo whereMethod = typeof(Queryable)
                    .GetMethods()
                    .Where(m => m.Name == "Where" &&
                               m.IsGenericMethod &&
                               m.GetParameters().Length == 2)
                    .First()
                    .MakeGenericMethod(TypeOfTheDynamicallyCreatedTable);

                // Создаем выражение параметра: p => 
                ParameterExpression parameter = System.Linq.Expressions.Expression.Parameter(TypeOfTheDynamicallyCreatedTable, "p");

                // Создаем выражение свойства: p.Property
                MemberExpression propertyExpression = System.Linq.Expressions.Expression.Property(parameter, property);

                // Создаем константное выражение для значения
                ConstantExpression valueExpression = System.Linq.Expressions.Expression.Constant(value);

                // Создаем выражение равенства: p.Property == value
                BinaryExpression equalsExpression = System.Linq.Expressions.Expression.Equal(propertyExpression, valueExpression);

                // Создаем лямбда-выражение: p => p.Property == value
                LambdaExpression lambdaExpression = System.Linq.Expressions.Expression.Lambda(equalsExpression, parameter);

                // Вызываем Where
                objToReturn = whereMethod.Invoke(null, new object[] { objToReturn, lambdaExpression });
            }

            return objToReturn;

        }

        private object getRequiredIncludes(object dbSet)
        {

            PropertyInfo[] propertiesToInclude = TypeOfTheDynamicallyCreatedTable.GetProperties()
                .Where(
                    p => p.GetCustomAttribute<DisplayBehaviourAttribute>().IsIncludeRequired
                )
                .ToArray();

            object objToreturn = dbSet;
            foreach (PropertyInfo property in propertiesToInclude)
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
                // Могу добавить форматирование даты
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
            DataGridTemplateColumn templateColumn = new DataGridTemplateColumn();

            
            templateColumn.Header = header;

            // Создаем DataTemplate для ячейки
            DataTemplate cellTemplate = new DataTemplate();

            // Создаем FrameworkElementFactory для кнопки
            FrameworkElementFactory buttonFactory = new FrameworkElementFactory(typeof(Button));

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

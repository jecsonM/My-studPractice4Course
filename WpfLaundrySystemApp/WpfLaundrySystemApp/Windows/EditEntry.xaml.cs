using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfLaundrySystemApp.Attributes;
using WpfLaundrySystemApp.DBContext;
using WpfLaundrySystemApp.Models;
using WpfLaundrySystemApp.Modules;

namespace WpfLaundrySystemApp.Windows
{
    /// <summary>
    /// Логика взаимодействия для EditEntry.xaml
    /// </summary>
    
    public partial class EditEntry : Window
    {
        public enum EditMode
        {
            None,
            Edit,
            Add
        }

        private EditMode _editMode;
        public DisplayTable ParentialWindow { get; set; }
        private object _editModel;
        private StackPanel _mainPanel;
        private DynamicTableCreator TableCreator;

        public EditEntry(object editModel, DisplayTable parentialWindow, EditMode editMode = EditMode.Edit)
        {

            ParentialWindow = parentialWindow;
            _editModel = editModel;
            _editMode = editMode;
            TableCreator =new DynamicTableCreator(new LaundryDbContext());
            InitializeComponent();
            CreateDynamicFields();
        }

        private void ButtonSeeAll_Click(object sender, RoutedEventArgs e)
        {
            PropertyInfo propOfButton = (sender as Button).Tag as PropertyInfo;
            Type typeChangeTo = propOfButton.PropertyType.GetGenericArguments()[0];
            
            ParentialWindow.SetNewDisplayTable(typeChangeTo, _editModel);

            this.Close();
            
        }

        private void CreateDynamicFields()
        {
            _mainPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness(20)
            };

            Type type = _editModel.GetType();
            PropertyInfo[] nonEditablePKproperties = 
                TableCreator.GetPrimaryKeyProperties(type)
                    .Except(
                        TableCreator.GetForeignKeyProperties(type)
                    )
                    .ToArray();
            
            PropertyInfo[] properties = 
                type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Except(nonEditablePKproperties)
                    .ToArray();

            foreach (PropertyInfo property in nonEditablePKproperties)
            {
                if (!property.CanRead || !property.CanWrite || !property.GetCustomAttribute<DisplayBehaviourAttribute>().Visible) continue;

                UIElement fieldContainer = CreateFieldForProperty(property, isEnabled: false);
                _mainPanel.Children.Add(fieldContainer);
            }

            foreach (PropertyInfo property in properties)
            {
                if (!property.CanRead || !property.CanWrite || !property.GetCustomAttribute<DisplayBehaviourAttribute>().Visible) continue;

                UIElement fieldContainer = CreateFieldForProperty(property);
                _mainPanel.Children.Add(fieldContainer);
            }

            // Добавляем кнопки сохранения и отмены
            StackPanel buttonPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 20, 0, 0)
            };

            Button saveButton = new Button
            {
                Content = "Сохранить",
                Margin = new Thickness(0, 0, 10, 0),
                Padding = new Thickness(10, 5, 10, 5)
            };
            saveButton.Click += SaveButton_Click;

            Button cancelButton = new Button
            {
                Content = "Отмена",
                Padding = new Thickness(10, 5, 10, 5)
            };
            cancelButton.Click += (s, e) => this.Close();

            buttonPanel.Children.Add(saveButton);
            buttonPanel.Children.Add(cancelButton);
            _mainPanel.Children.Add(buttonPanel);

            Content = new ScrollViewer
            {
                Content = _mainPanel,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto
            };
        }

        private UIElement CreateFieldForProperty(PropertyInfo property, bool isEnabled = true)
        {
            StackPanel container = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness(0, 10, 0, 0)
            };

            // Создаем метку с именем свойства
            Label label = new Label
            {
                Content = property.GetCustomAttribute<DisplayBehaviourAttribute>().DisplayName,
                FontWeight = FontWeights.Bold
            };

            // Создаем поле ввода в зависимости от типа свойства
            FrameworkElement inputControl = CreateInputControl(property, isEnabled);

            // Устанавливаем текущее значение
            SetControlValue(inputControl, property.GetValue(_editModel));

            // Сохраняем ссылку на свойство для обновления значения
            inputControl.Tag = property;

            container.Children.Add(label);
            container.Children.Add(inputControl);

            return container;
        }

        private FrameworkElement CreateInputControl(PropertyInfo property, bool isEnabled = true)
        {
            Type propertyType = property.PropertyType;

            if (propertyType == typeof(string))
            {
                return new TextBox
                {
                    Height = 25,
                    Padding = new Thickness(5),
                    IsEnabled = isEnabled
                };
            }
            else if (propertyType == typeof(int) || propertyType == typeof(float) || propertyType == typeof(decimal))
            {
                return new TextBox
                {
                    Height = 25,
                    Padding = new Thickness(5),
                    IsEnabled = isEnabled
                };
            }
            else if (propertyType == typeof(DateTime) || propertyType == typeof(DateTime?))
            {
                return new DatePicker
                {
                    Height = 25,
                    IsEnabled = isEnabled
                };
            }
            else if (propertyType == typeof(TimeSpan))
            {
                TextBox textBox = new TextBox
                {
                    Height = 25,
                    Padding = new Thickness(5),
                    ToolTip = "Формат: чч:мм:сс",
                    IsEnabled = isEnabled
                };

                return textBox;
            }
            else if (propertyType == typeof(byte?[]) || propertyType == typeof(byte[]))
            {
                return new TextBox
                {
                    Height = 25,
                    Padding = new Thickness(5),
                    IsEnabled = false
                };
            }
            else if (propertyType == typeof(bool) || propertyType == typeof(bool?))
            {
                return new CheckBox() { IsEnabled = isEnabled };
            }
            else if (property.GetCustomAttribute<DisplayBehaviourAttribute>().IsIncludeRequired)
            {
                ComboBox listBox = new ComboBox()
                {
                    Tag = property,
                    IsEnabled = isEnabled
                };

                TableCreator.TypeOfTheDynamicallyCreatedTable = propertyType;
                listBox.ItemsSource = TableCreator.GetData();

                return listBox;
            }
            else if (property.GetCustomAttribute<DisplayBehaviourAttribute>().IsSeeAllButtonRequired)
            {
                return new Button()
                {
                    Content = "Просмотреть",
                    Tag = property
                };
            }
            else
            {
                // Для остальных типов используем TextBox
                return new TextBox
                {
                    Height = 25,
                    Padding = new Thickness(5),
                    ToolTip = "Будет преобразовано в строку"
                };
            }
        }

        private void SetControlValue(FrameworkElement control, object value)
        {
            if (value == null) return;

            switch (control)
            {
                case TextBox textBox:
                    if (value is TimeSpan timeSpan)
                    {
                        textBox.Text = timeSpan.ToString(@"hh\:mm\:ss");
                    }
                    else
                    {
                        textBox.Text = value.ToString();
                    }
                    break;
                case DatePicker datePicker when value is DateTime dateTime:
                    datePicker.SelectedDate = dateTime;
                    break;
                case CheckBox checkBox when value is bool boolValue:
                    checkBox.IsChecked = boolValue;
                    break;
                case Button button:
                    button.Click += ButtonSeeAll_Click;
                    
                    break;
                case ComboBox comboBox:


                    
                    if (comboBox.SelectedIndex == -1 && comboBox.Items.Count > 0)
                        comboBox.SelectedIndex = 0;
                        
                    int index = comboBox.SelectedIndex;
                    Type comboBoxItemType = comboBox.SelectedValue.GetType();
                    PropertyInfo[] keyProperties= TableCreator.GetPrimaryKeyProperties(comboBoxItemType);
                    List<PropertyInfo> propertyInfos = comboBoxItemType.GetProperties().Where(p => keyProperties.Contains(p)).ToList();

                    do
                    {
                        int matches = 0;
                        foreach (PropertyInfo property in propertyInfos)
                            if ((int)property.GetValue(comboBox.Items.CurrentItem) == (int)property.GetValue(value))
                                matches++;
                        if (matches == propertyInfos.Count)
                        {
                            comboBox.SelectedIndex = comboBox.Items.CurrentPosition;
                            break;
                        }
                    }
                    while (comboBox.Items.MoveCurrentToNext());
                    
                    break;
                    

            }
        }

        private object GetControlValue(FrameworkElement control, Type targetType)
        {
            try
            {
                switch (control)
                {
                    case TextBox textBox:
                        if (targetType == typeof(string))
                            return textBox.Text;
                        else if (targetType == typeof(int))
                            return int.Parse(textBox.Text);
                        else if (targetType == typeof(float))
                            return float.Parse(textBox.Text);
                        else if (targetType == typeof(decimal))
                            return decimal.Parse(textBox.Text);
                        else if (targetType == typeof(TimeSpan))
                            return TimeSpan.Parse(textBox.Text);
                        else if (targetType == typeof(byte[]))
                            return new byte[0]; ////ЗАГЛУШКА ДЛЯ КАРТИНОК
                        else
                            return Convert.ChangeType(textBox.Text, targetType);

                    case DatePicker datePicker:
                        return datePicker.SelectedDate ?? DateTime.Now;
                    case CheckBox checkBox:
                        return checkBox.IsChecked ?? false;

                    case ComboBox comboBox:
                        return comboBox.SelectedItem;
                    default:
                        return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка преобразования значения: {ex.Message}");
                return GetDefaultValue(targetType);
            }
        }

        private object GetDefaultValue(Type type)
        {
            if (type.IsValueType)
                return Activator.CreateInstance(type);
            return null;
        }


        private void SetModelFK(PropertyInfo property, object newValue)
        {
            object propertyValue = property.GetValue(_editModel);
            
            Dictionary<PropertyInfo, PropertyInfo> fk_pk_Properties = TableCreator.GetFK_PK_pairsProperties(_editModel.GetType(), property.PropertyType);
            foreach(KeyValuePair<PropertyInfo, PropertyInfo> fk_pk_property in fk_pk_Properties)
            {
                fk_pk_property.Key.SetValue(_editModel, fk_pk_property.Value.GetValue(newValue));
            }


        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (Panel container in _mainPanel.Children)
                {
                    if (container is StackPanel panel && panel.Children.Count > 1)
                    {
                        FrameworkElement inputControl = panel.Children[1] as FrameworkElement;
                        PropertyInfo property = inputControl?.Tag as PropertyInfo;
                        

                        if (property != null)
                        {
                            object newValue = GetControlValue(inputControl, property.PropertyType);

                            if (property.GetCustomAttribute<DisplayBehaviourAttribute>().IsIncludeRequired)
                            {
                                if (newValue is null)
                                    throw new FieldException("поле указано некорректно", property); 
                                SetModelFK(property, newValue);
                            }
                            else
                                property.SetValue(_editModel, newValue);
                        }
                    }
                }
                if (_editMode == EditMode.Add)
                    ParentialWindow.dynamicTableCreator.TryAddingNewObject(_editModel);
                this.DialogResult = true;
                this.Close();
            }
            catch (FieldException ex)
            {
                MessageBox.Show($"Ошибка при сохранении: в поле \"{ex.PropertyInfoThatCausedException.GetCustomAttribute<DisplayBehaviourAttribute>().DisplayName}\" {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}");
            }
            
        }
        private class FieldException : Exception
        {

            public FieldException(string message, PropertyInfo propertyInfoThatCausedException) : base(message) 
            {
                PropertyInfoThatCausedException = propertyInfoThatCausedException;


            }

            public PropertyInfo PropertyInfoThatCausedException;
        }
        
    }
}

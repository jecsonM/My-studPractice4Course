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
using Microsoft.EntityFrameworkCore;
using WpfLaundrySystemApp.DBContext;
using WpfLaundrySystemApp.Models;
using WpfLaundrySystemApp.Modules;

namespace WpfLaundrySystemApp.Windows
{
    /// <summary>
    /// Логика взаимодействия для EditDisplay.xaml
    /// </summary>
    public partial class EditDisplay : Window
    {
        DynamicTableCreator dynamicTableCreator;
        public EditDisplay()
        {
            InitializeComponent();


            
            dynamicTableCreator = new DynamicTableCreator(new LaundryDbContext(), new RoutedEventHandler(Button_Click));
            dynamicTableCreator.TypeOfTheDynamicallyCreatedTable = typeof(Partner);

            using (var context = new LaundryDbContext())
            {
                context.AttendedServices.Find()
            }

            UpdateTable();


            
        }

        private void UpdateTable()
        {
            pageTitle.Text = dynamicTableCreator.GetTitle();
            mainContent.Content = dynamicTableCreator.GenerateTable();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            /////////////////////////////////////////////////////////////////////////////////////////////////////////Отсевиать PK по "посмотреть"
            dynamicTableCreator.TypeOfTheDynamicallyCreatedTable = (button.Tag as Type).GetGenericArguments()[0];
            var dataItem = button.DataContext;
            Type itemType = dataItem.GetType();
            PropertyInfo property = itemType.GetProperty("НазваниеСвойства");
            if (property != null)
            {
                object propertyValue = property.GetValue(dataItem);
                Console.WriteLine($"Значение свойства: {propertyValue}");
            }


            UpdateTable();
        }
    }
}

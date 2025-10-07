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
    /// Логика взаимодействия для DisplayTable.xaml
    /// </summary>
    public partial class DisplayTable : Window
    {
        public DynamicTableCreator dynamicTableCreator;
        public DisplayTable(Type typeToOpenTable)
        {
            InitializeComponent();



            dynamicTableCreator = new DynamicTableCreator(new LaundryDbContext());

            dynamicTableCreator.SeeAllButton = new RoutedEventHandler(ButtonSeeAll_Click);
            dynamicTableCreator.TypeOfTheDynamicallyCreatedTable = typeToOpenTable;
            dynamicTableCreator.EditMenuItem_Click = new RoutedEventHandler(EditMenuItem_Click);
            dynamicTableCreator.DeleteMenuItem_Click = new RoutedEventHandler(DeleteMenuItem_Click);
            dynamicTableCreator.AddMenuItem_Click = new RoutedEventHandler(AddMenuItem_Click);



            UpdateTable();



        }

        public DisplayTable() : this(typeof(Partner)) 
        {   
        }

        public void SetNewDisplayTable(Type typeToChangeTableFor, object getPKconstraintsFrom = null)
        {
            dynamicTableCreator.SetNewTableToGenerate(typeToChangeTableFor, getPKconstraintsFrom);
            UpdateTable();
        }

        private void UpdateTable()
        {
            pageTitle.Text = dynamicTableCreator.GetTitle();
            mainContent.Content = dynamicTableCreator.GenerateTable();
        }

        private void ButtonSeeAll_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;
            SetNewDisplayTable((button.Tag as Type).GetGenericArguments()[0], button.DataContext);

        }
        private void EditMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem ItemToEdit = sender as MenuItem;

            EditEntry editWindow = new EditEntry(dynamicTableCreator.DataGrid.SelectedItem, this);
            if (editWindow.ShowDialog() == true)
            {
                dynamicTableCreator.SaveChanges();
                dynamicTableCreator.ReCreateDbContext();
            }
            else
                dynamicTableCreator.ReCreateDbContext();
            UpdateTable();
        }
        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
        }

        private void AddMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem ItemToEdit = sender as MenuItem;

            object newItem = Activator.CreateInstance(dynamicTableCreator.TypeOfTheDynamicallyCreatedTable);

            EditEntry editWindow = new EditEntry(newItem, this, editMode: EditEntry.EditMode.Add);
            if (editWindow.ShowDialog() == true)
            {
                dynamicTableCreator.SaveChanges();
                dynamicTableCreator.ReCreateDbContext();
            }
            else
                dynamicTableCreator.ReCreateDbContext();
            UpdateTable();
        }
    }
}

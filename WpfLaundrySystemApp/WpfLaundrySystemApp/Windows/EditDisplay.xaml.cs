using System;
using System.Collections.Generic;
using System.Linq;
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
        public EditDisplay()
        {
            InitializeComponent();
            using (LaundryDbContext db = new LaundryDbContext())
            {
                Content = DynamicTableCreator.GenerateTable<Workshop>(db.Workshops.ToList<Workshop>());
            }
        }



    }
}

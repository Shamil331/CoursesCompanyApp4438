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
using DanceCompnayApp4438;

namespace CoursesCompanyApp4438.Windows
{
    /// <summary>
    /// Логика взаимодействия для Courses.xaml
    /// Класс предназначенный для просмотра списка Курсов
    /// </summary>
    public partial class Courses : Window
    {
        public Courses()
        {
            InitializeComponent();
            using (DanceCompanyEntities ent = new DanceCompanyEntities())
                CoursesGrid.ItemsSource = ent.Course.ToList();
        }
        /// <summary>
        /// Обработчик события на нажатие кнопки BackBtn
        /// Возвращает на навигационную страницу
        /// </summary>
        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow MW = new MainWindow();
            MW.Show();
            this.Close();
        }
    }
}

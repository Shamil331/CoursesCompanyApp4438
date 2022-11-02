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
using System.Windows;
using DanceCompnayApp4438;

namespace CoursesCompanyApp4438.Windows
{
    /// <summary>
    /// Логика взаимодействия для TraineeExclude.xaml
    /// Класс, предназначенный для просмотра обучающихся с возможностью их удаления и переходом на страницу с более подробной информации об обучающемся
    /// </summary>
    public partial class TraineeExclude : Window
    {
        public TraineeExclude()
        {
            InitializeComponent();
            RefreshGrid();
        }
        /// <summary>
        /// Функция для обновления содержимого TraineeGrid, в котором содержится информация ою обучающихся
        /// </summary>
        private void RefreshGrid()
        {
            using (DanceCompanyEntities ent = new DanceCompanyEntities())
                TraineeGrid.ItemsSource = ent.Trainee.ToList();
        }
        /// <summary>
        /// Обработчик события на нажатие кнопки AboutBtn
        /// Переход на страницу подробной информации об обучающемся
        /// В качестве параметра передаём объект типа данных Trainee
        /// </summary>
        private void AboutBtn_Click(object sender, RoutedEventArgs e)
        {
            AboutTrainee at = new AboutTrainee((sender as Button).DataContext as Trainee);
            at.ShowDialog();
        }

        /// <summary>
        /// Обработчик события на кнопку DeleteBtn
        /// Удаление выбранного обучающегося с подтверждением пользователя
        /// </summary>
        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены что хотите исключить даннного обучающегося?", "Удаление пользователя", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                using (DanceCompanyEntities ent = new DanceCompanyEntities())
                {
                    Trainee trainee = (sender as Button).DataContext as Trainee;
                    ent.Entry(trainee).State = System.Data.Entity.EntityState.Deleted;
                    ent.Trainee.Remove(trainee);
                    TraineeCourse tc = new TraineeCourse();
                    tc = ent.TraineeCourse.Where(x => x.EndDate > DateTime.Now && x.Trainee_Id == trainee.Id).FirstOrDefault();
                    if (tc != null)
                    {
                        int CourseId = tc.Course_Id;
                        Course course = ent.Course.Where(x => x.Id == CourseId).FirstOrDefault();
                        ent.Course.Attach(course);
                        course.PeopleQuantity--;
                    }
                    ent.SaveChanges();
                    RefreshGrid();
                }
            }
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

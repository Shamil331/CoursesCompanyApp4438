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

namespace CoursesCompanyApp4438.Windows
{
    /// <summary>
    /// Логика взаимодействия для AboutTrainee.xaml
    /// В данном классе реализуются просмотр подробных данных об обучающемся и возможность смены срока его обучения
    /// </summary>
    public partial class AboutTrainee : Window
    {
        private Trainee _trainee;
        private TraineeCourse _traineeCourse;
        private Course _course;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="trainee">Получаемый параметр trainee является типом данных Trainee и содержит в себе информацию об обучающемся</param>
        public AboutTrainee(Trainee trainee)
        {
            InitializeComponent();
            _trainee = trainee;
            DataRefresh();
        }
        /// <summary>
        /// Функция для обновления информации на странице
        /// </summary>
        private void DataRefresh()
        {
            StartDateDp.Visibility = Visibility.Collapsed;
            EndDateDp.Visibility = Visibility.Collapsed;
            DateChangeBtn.Content = "Изменить сроки обучения";
            using (DanceCompanyEntities ent = new DanceCompanyEntities())
            {
                FullNameTb.Text += _trainee.FullName;
                PostTb.Text += _trainee.Post;
                string TraineeCompany = ent.Request.Where(x => x.Id == _trainee.Request_Id).Select(x => x.OrganizationName).FirstOrDefault();
                CompanyTb.Text += TraineeCompany;
                TraineeCourse tc = new TraineeCourse();
                tc = ent.TraineeCourse.Where(x => x.EndDate > DateTime.Now && x.Trainee_Id == _trainee.Id).FirstOrDefault();
                _traineeCourse = tc;
                if (tc != null)
                {
                    int CourseId = tc.Course_Id;
                    Course course = ent.Course.Where(x => x.Id == CourseId).FirstOrDefault();
                    _course = course;
                    CourseTb.Text += course.Name;
                    StartDateTb.Text += tc.StartDate.Date.ToString().Split(' ')[0];
                    EndDateTb.Text += tc.EndDate.Date.ToString().Split(' ')[0];
                }
                if (tc == null)
                {
                    CourseTb.Text += "Нет активных";
                    StartDateTb.Visibility = Visibility.Collapsed;
                    EndDateTb.Visibility = Visibility.Collapsed;
                    DateChangeBtn.Visibility = Visibility.Collapsed;
                }
            }
        }
        /// <summary>
        /// Обработчик события на нажатие кнопки DataChangeBtn
        /// Эта кнопкаиспользуется два раза:
        /// 1. Для возможности изменения сроков обучения (начальной и конечной даты обучения)
        /// 2. Для сохранения изменений, внесённых на странице (в DatePicker'ах)
        /// </summary>
        private void DateChangeBtn_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).Content == "Сохранить изменения")
            {
                using (DanceCompanyEntities ent = new DanceCompanyEntities())
                {
                    List<TeacherCourse> teacherCourses = new List<TeacherCourse>();
                    if (ent.TeacherCourse.Where(t => t.Course_Id == _course.Id && t.StartDate <= StartDateDp.SelectedDate).ToList().Count != 0 && ent.TeacherCourse.Where(t => t.Course_Id == _course.Id && t.EndDate >= EndDateDp.SelectedDate).ToList().Count != 0)
                    {
                        if (EndDateDp.SelectedDate > StartDateDp.SelectedDate)
                        {
                            ent.TraineeCourse.Attach(_traineeCourse);
                            _traineeCourse.StartDate = (DateTime)StartDateDp.SelectedDate;
                            _traineeCourse.EndDate = (DateTime)EndDateDp.SelectedDate;
                            ent.SaveChanges();
                            AboutTrainee AT = new AboutTrainee(_trainee);
                            AT.Show();
                            this.Close();
                        }
                        else MessageBox.Show("Дата начала должны быть раньше даты окончания");
                    }
                    else MessageBox.Show("На данный срок нет преподавателей, пожалуйста выберите другой");
                }
            }
            if ((sender as Button).Content=="Изменить сроки обучения")
            {
                StartDateTb.Text = "Дата начала обчуения: ";
                EndDateTb.Text = "Дата окончания обучения: ";
                StartDateDp.Visibility = Visibility.Visible;
                EndDateDp.Visibility = Visibility.Visible;
                StartDateDp.SelectedDate = _traineeCourse.StartDate.Date;
                EndDateDp.SelectedDate = _traineeCourse.EndDate.Date;
                DateChangeBtn.Content = "Сохранить изменения";
            }
            
        }
    }
}

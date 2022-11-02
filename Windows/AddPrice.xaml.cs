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
    /// Логика взаимодействия для AddPrice.xaml
    /// Класс предназначен для изменения цены у курса
    /// </summary>
    public partial class AddPrice : Window
    {
        private Course _SelectedCourse;
        public AddPrice()
        {
            InitializeComponent();
            using (DanceCompanyEntities ent = new DanceCompanyEntities())
            {
                List<Course> courses= new List<Course>();
                courses.AddRange(ent.Course.OrderBy(x => x.Name).ToList());
                CoursesList.ItemsSource=courses;
                CoursesList.SelectedIndex = 0;
            }
        }
        /// <summary>
        /// Обработчик события на то, поменялся ли элемент в ComboBox под названием CourseList, т.е. вызывается, когда мы выбираем там какой-либо элемент
        /// </summary>
        private void CoursesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _SelectedCourse = CoursesList.SelectedItem as Course;
            RefreshPrice();
        }
        /// <summary>
        /// Обработчик события на нажате кнопки ChahgePriceBtn
        /// Добавляется новая запись в таблицу Price с сегодняшней датой, указанной новой ценой и айдишником курса, цену которой мы меняем
        /// Также добавляется запись в таблицу, отвечающая за цены с НДС
        /// </summary>
        private void ChahgePriceBtn_Click(object sender, RoutedEventArgs e)
        {
            if(NewPriceTb.Visibility==Visibility.Collapsed)
                NewPriceTb.Visibility = Visibility.Visible;
            else
            {
                using (DanceCompanyEntities ent = new DanceCompanyEntities())
                {
                    int variable=int.TryParse(NewPriceTb.Text, out variable) ? variable : 0;
                    if (variable == 0)
                        MessageBox.Show("Цена должна быть натуральным числом");
                    else
                    {
                        Price newPrice = new Price();
                        newPrice.Course_Id = _SelectedCourse.Id;
                        newPrice.Date = DateTime.Now.Date;
                        newPrice.Price1 = variable;
                        ent.Price.Add(newPrice);
                        ent.PricePlusVAT.Add(new PricePlusVAT()
                        {
                            Price_Id=newPrice.Id,
                            PriceVAT=newPrice.Price1+(int)(newPrice.Price1*0.2)
                        });
                        ent.SaveChanges();
                        RefreshPrice();
                    }
                }
            }
            
        }
        /// <summary>
        /// Функция для обновления информации на странице
        /// </summary>
        private void RefreshPrice()
        {
            using (DanceCompanyEntities ent = new DanceCompanyEntities())
            {
                Course course = new Course();
                course = ent.Course.Where(x => x.Name == _SelectedCourse.Name).FirstOrDefault();
                List<Price> priceList = new List<Price>();
                priceList = ent.Price.OrderBy(p => p.Date).Where(p => p.Course_Id == course.Id).ToList();
                Price actualPrice = priceList.Last();
                List<PricePlusVAT> pricePlusVATs = new List<PricePlusVAT>();
                pricePlusVATs = ent.PricePlusVAT.Where(p => p.Price_Id==actualPrice.Id).ToList();
                PricePlusVAT actualPricePlusVAT=pricePlusVATs.FirstOrDefault();
                ActualPriceTb.Text="Актуальная цена: без НДС-"+actualPrice.Price1.ToString()+"; с НДС-"+actualPricePlusVAT.PriceVAT.ToString();
                NewPriceTb.Visibility = Visibility.Collapsed;
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

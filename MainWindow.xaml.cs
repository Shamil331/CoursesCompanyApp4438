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
using System.Windows.Navigation;
using System.Windows.Shapes;
using CoursesCompanyApp4438.Windows;
using Word = Microsoft.Office.Interop.Word;
using CoursesCompanyApp4438;

namespace DanceCompnayApp4438
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// В данном классе производится навигация по различным страницам и возможен экспорт таблицы Преподавателей в PDF файл
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Обработчик события на нажатие кнопки CourseBtn
        /// Открывается окно со списком Курсов
        /// </summary>
        private void CourseBtn_Click(object sender, RoutedEventArgs e)
        {
            Courses c = new Courses();
            c.Show();
            this.Close();
        }
        /// <summary>
        /// Обработик события на нажатие кнопки RequestBtn
        /// Открывается страница со списком Заявок
        /// </summary>
        private void RequestBtn_Click(object sender, RoutedEventArgs e)
        {
            Requests r = new Requests();
            r.Show();
            this.Close();
        }
        /// <summary>
        /// Обработчик события на нажатие кнопки PriceBtn
        /// Открывается страница для изменения Цен
        /// </summary>
        private void PriceBtn_Click(object sender, RoutedEventArgs e)
        {
            AddPrice ap = new AddPrice();
            ap.Show();
            this.Close();
        }
        /// <summary>
        /// Обработчик события на нажатие кнопки TraineeBtn
        /// Открывается страница со списком Обучающихся, где можно их удалить или изменить их сроки обучения
        /// </summary>
        private void TraineeBtn_Click(object sender, RoutedEventArgs e)
        {
            TraineeExclude TE = new TraineeExclude();
            TE.Show();
            this.Close();
        }
        /// <summary>
        /// Обработчик события на нажатие кнопки TeacherPDF
        /// Выполняется экспорт таблицы преподавателей в PDF файл
        /// </summary>
        private void TeacherPDFBtn_Click(object sender, RoutedEventArgs e)
        {
            using (DanceCompanyEntities ent = new DanceCompanyEntities())
            {
                List<Teacher> teachers = ent.Teacher.OrderBy(x => x.FullName).ToList();
                var app = new Word.Application();
                Word.Document doc = app.Documents.Add();
                Word.Paragraph paragraph = doc.Paragraphs.Add();
                Word.Range range = paragraph.Range;
                range.Text = "Преподаватели";
                paragraph.set_Style("Заголовок 1");
                range.InsertParagraphAfter();
                Word.Paragraph tableParagraph = doc.Paragraphs.Add();
                Word.Range tableRange = tableParagraph.Range;
                Word.Table teachersTable = doc.Tables.Add(tableRange, teachers.Count()+1, 6);
                teachersTable.Borders.InsideLineStyle = teachersTable.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                teachersTable.Range.Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                Word.Range cellRange;
                cellRange = teachersTable.Cell(1, 1).Range;
                cellRange.Text = "Порядковый номер";
                cellRange = teachersTable.Cell(1, 2).Range;
                cellRange.Text = "ФИО";
                cellRange = teachersTable.Cell(1, 3).Range;
                cellRange.Text = "Дата рождения";
                cellRange = teachersTable.Cell(1, 4).Range;
                cellRange.Text = "Пол";
                cellRange = teachersTable.Cell(1, 5).Range;
                cellRange.Text = "Образование";
                cellRange = teachersTable.Cell(1, 6).Range;
                cellRange.Text = "Категория";
                teachersTable.Rows[1].Range.Bold = 1;
                teachersTable.Rows[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                int i = 1;
                foreach(var currentTeacher in teachers)
                {
                    cellRange = teachersTable.Cell(i + 1, 1).Range;
                    cellRange.Text = currentTeacher.Id.ToString();
                    cellRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    cellRange = teachersTable.Cell(i + 1, 2).Range;
                    cellRange.Text = currentTeacher.FullName.ToString();
                    cellRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    cellRange = teachersTable.Cell(i + 1, 3).Range;
                    cellRange.Text = currentTeacher.DateOfBirth.ToString().Split(' ')[0];
                    cellRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    cellRange = teachersTable.Cell(i + 1, 4).Range;
                    cellRange.Text = currentTeacher.Sex.ToString();
                    cellRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    cellRange = teachersTable.Cell(i + 1, 5).Range;
                    cellRange.Text = currentTeacher.Education.ToString();
                    cellRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    cellRange = teachersTable.Cell(i + 1, 6).Range;
                    cellRange.Text = currentTeacher.Category.ToString();
                    cellRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    i++;
                }
                app.Visible = true;
                doc.SaveAs2(@"C:\Users\79172\Desktop\TeachersPDF.pdf", Word.WdExportFormat.wdExportFormatPDF);
            }
        }
    }
}

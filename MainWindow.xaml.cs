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
using System.Configuration;
using System.Data.Linq;

namespace LinqToSQL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        LinqToSqlDataClassesDataContext dataContext;
        public MainWindow()
        {
            InitializeComponent();

            string connectionString = ConfigurationManager.ConnectionStrings["LinqToSQL.Properties.Settings.LoivpMSSQLDB_1ConnectionString"].ConnectionString;
            dataContext = new LinqToSqlDataClassesDataContext(connectionString);

            /*InsertUniversities();*/

            InsertStudent();
            /*InserLectures();*/
            /*InsertStudentLectureAssociations();*/
            /*GetUnivsersityOFCarla();*/
            /*GetLecturesFromCarla();*/
            UpdateToni();
            DeleteAnotonio();
        }

        public void InsertUniversities()
        {
            dataContext.ExecuteCommand("delete from University");

            University yale = new University();
            yale.Name = "Yale";
            dataContext.Universities.InsertOnSubmit(yale);

            University beijingTech = new University();
            beijingTech.Name = "Beijing Tech";
            dataContext.Universities.InsertOnSubmit(beijingTech);

            dataContext.SubmitChanges();

            MainDataGrid.ItemsSource = dataContext.Universities;
        }

        public void InsertStudent()
        {
            University yale = dataContext.Universities.First(un => un.Name.Equals("Yale"));
            University beijingTech = dataContext.Universities.First(un => un.Name.Equals("Beijing Tech"));
            List<Student> students = new List<Student>();

            students.Add(new Student { Name = "Carla", Gender = "female", UniversityId = yale.Id });
            students.Add(new Student { Name = "Toni", Gender = "male", UniversityId = yale.Id });
            students.Add(new Student { Name = "Kit", Gender = "male", UniversityId = beijingTech.Id });
            students.Add(new Student { Name = "Sara", Gender = "female", UniversityId = yale.Id });

            dataContext.Students.InsertAllOnSubmit(students);
            dataContext.SubmitChanges();

            MainDataGrid.ItemsSource = dataContext.Students;
        }
        public void InserLectures()
        {

            dataContext.Lectures.InsertOnSubmit(new Lecture { Name = "Math" });
            dataContext.Lectures.InsertOnSubmit(new Lecture { Name = "Science" });
            dataContext.Lectures.InsertOnSubmit(new Lecture { Name = "English" });
            dataContext.Lectures.InsertOnSubmit(new Lecture { Name = "History" });

            dataContext.SubmitChanges();

            MainDataGrid.ItemsSource = dataContext.Lectures;
        }

        public void InsertStudentLectureAssociations()
        {
            Student Carla =  dataContext.Students.First(st => st.Name.Equals("Carla"));
            Student Toni =  dataContext.Students.First(st => st.Name.Equals("Toni"));
            Student Kit =  dataContext.Students.First(st => st.Name.Equals("Kit"));
            Student Sara =  dataContext.Students.First(st => st.Name.Equals("Sara"));

            Lecture Math = dataContext.Lectures.First(lc => lc.Name.Equals("Math"));
            Lecture Science = dataContext.Lectures.First(lc => lc.Name.Equals("Science"));

            dataContext.StudentLectures.InsertOnSubmit(new StudentLecture { StudentId = Carla.Id, LectureId = Math.Id });

            StudentLecture slToni= new StudentLecture();
            slToni.StudentId = Toni.Id;
            slToni.LectureId = Science.Id;
            dataContext.StudentLectures.InsertOnSubmit(slToni);


            dataContext.SubmitChanges();

            MainDataGrid.ItemsSource = dataContext.StudentLectures;


        }

        public void GetUnivsersityOFCarla()
        {
            Student Carla = dataContext.Students.First(st => st.Name.Equals("Carla"));

            University CarlaUni = Carla.University;

            List<University> unis = new List<University>();
            unis.Add(CarlaUni);

            MainDataGrid.ItemsSource = unis;


        }

        public void GetLecturesFromCarla()
        {
            Student Carla = dataContext.Students.First(student => student.Name.Equals("Carla"));

            var carlaLectures = from sl in Carla.StudentLectures select sl.Lecture;

            MainDataGrid.ItemsSource = carlaLectures;
        }

        public void GetAllLecturesFromBeijinTec()
        {
            var allLecturesFromBeijinTech = from sl in dataContext.StudentLectures
                                            join student in dataContext.Students
                                            on sl.StudentId equals student.Id
                                            where student.University.Name == "Beijing Tech"
                                            select sl.Lecture;

        }

        public void UpdateToni()
        {
            Student Anotonio = dataContext.Students.FirstOrDefault(st => st.Name == "Toni");

            Anotonio.Name = "Anotonio";

            dataContext.SubmitChanges();

            MainDataGrid.ItemsSource = dataContext.Students;
        }

        public void DeleteAnotonio()
        {
            Student student = dataContext.Students.FirstOrDefault(st => st.Name == "Anotonio");

            dataContext.Students.DeleteOnSubmit(student);
            dataContext.SubmitChanges();
            MainDataGrid.ItemsSource = dataContext.Students;


        }

    }
}

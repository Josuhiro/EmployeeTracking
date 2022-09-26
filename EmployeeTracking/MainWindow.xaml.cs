using EmployeeTracking.DB;
using EmployeeTracking.ViewModels;
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

namespace EmployeeTracking
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (TrackingEmployeeContext db=new TrackingEmployeeContext())
            {

            }
        }

        private void buttonDepartment_Click(object sender, RoutedEventArgs e)
        {
            labelWindowName.Content = "Department List";
            DataContext = new DepartmentViewModel();
        }
        private void buttonPosition_Click(object sender, RoutedEventArgs e)
        {
            labelWindowName.Content = "Position List";
            DataContext = new PositionViewModel();
        }

        private void buttonEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (!UserStatic.isAdmin)
            {
                TrackingEmployeeContext db = new TrackingEmployeeContext();
                Employee employee = db.Employees.Find(UserStatic.EmployeeId);
                EmployeeDetailModel model = new EmployeeDetailModel();
                model.Adress = employee.Address;
                model.BirthDay = (DateTime)employee.Birthday;
                model.DepartmentId = employee.DepartmentId;
                model.Id = employee.Id;
                model.ImagePath = employee.ImagePath;
                model.isAdmin = (bool)employee.IsAdmin;
                model.Name = employee.Name;
                model.Password = employee.Password;
                model.PositionId = employee.PositionId;
                model.Salary = employee.Salary;
                model.Surname = employee.Surname;
                model.EmployeeNumber = employee.EmployeeNumber;
                EmployeePage page = new EmployeePage();
                page.model = model;
                page.ShowDialog();
            }
            else
            {
                labelWindowName.Content = "Employee List";
                DataContext = new EmployeeViewModel();
            }
        }
        
        private void buttonTask_Click(object sender, RoutedEventArgs e)
        {


            labelWindowName.Content = "Task List";
            DataContext = new TaskViewModel();

        }

        private void buttonSalary_Click(object sender, RoutedEventArgs e)
        {
            labelWindowName.Content = "Salary List";
            DataContext = new SalaryViewModel();
        }

        private void buttonPermission_Click(object sender, RoutedEventArgs e)
        {
            labelWindowName.Content = "Permission List";
            DataContext = new PermissionViewModel();
        }

        private void buttonLogOut_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }



        private void PersonalMainWindow_Closed(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PersonalMainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (!UserStatic.isAdmin)
            {
                stackDepartment.Visibility = Visibility.Hidden;
                stackPosition.Visibility = Visibility.Hidden;
                stackLogOut.SetValue(Grid.RowProperty, 5);
                stackExit.SetValue(Grid.RowProperty, 6);

            }
        }
    }
}

using EmployeeTracking.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EmployeeTracking
{
    /// <summary>
    /// Logika interakcji dla klasy LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Window
    {
        public LoginPage()
        {
            InitializeComponent();
        }
        TrackingEmployeeContext db = new TrackingEmployeeContext();
        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void buttonLogin_Click(object sender, RoutedEventArgs e)
        {
            if (txtEmployeeNumber.Text.Trim() == "" || txtPassword.Text.Trim() == "")
                MessageBox.Show("Please fill the Employee number and password area");
            else
            {
                Employee employee = db.Employees.FirstOrDefault(x => x.EmployeeNumber == Convert.ToInt32(txtEmployeeNumber.Text) &&
                  x.Password.Equals(txtPassword.Text));
                if (employee != null && employee.Id != 0)
                {
                    this.Visibility = Visibility.Collapsed;
                    MainWindow main = new MainWindow();
                    UserStatic.EmployeeId = employee.Id;
                    UserStatic.isAdmin = (bool)employee.IsAdmin;
                    UserStatic.Name = employee.Name;
                    UserStatic.Surname = employee.Surname;
                    UserStatic.EmployeeNumber = employee.EmployeeNumber;
                    main.ShowDialog();
                    txtPassword.Clear();
                    txtEmployeeNumber.Clear();
                    this.Visibility = Visibility.Visible;
                }
                else
                    MessageBox.Show("Please make sure that your passwrod and Employee number is correct");
            }
        }
        private void txtEmployeeNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }
    }
}

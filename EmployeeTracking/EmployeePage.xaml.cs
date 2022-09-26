using EmployeeTracking.DB;
using EmployeeTracking.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Logika interakcji dla klasy EmployeePage.xaml
    /// </summary>
    public partial class EmployeePage : Window
    {
        public EmployeePage()
        {
            InitializeComponent();
        }
        TrackingEmployeeContext db = new TrackingEmployeeContext();
        List<Position> positions = new List<Position>();
        public EmployeeDetailModel model;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cmbDepartment.ItemsSource = db.Departments.ToList();
            cmbDepartment.DisplayMemberPath = "DepartmentName";
            cmbDepartment.SelectedValuePath = "Id";
            cmbDepartment.SelectedIndex = -1;
            positions = db.Positions.ToList();
            cmbPosition.ItemsSource = positions;
            cmbPosition.DisplayMemberPath = "PositionName";
            cmbPosition.SelectedValuePath = "Id";
            cmbPosition.SelectedIndex = -1;
            if (model != null && model.Id != 0)
            {
                cmbDepartment.SelectedValue = model.DepartmentId;
                cmbPosition.SelectedValue = model.PositionId;
                txtEmployeeNumber.Text = model.EmployeeNumber.ToString();
                txtPassword.Text = model.Password;
                txtName.Text = model.Name;
                txtSurname.Text = model.Surname;
                txtSalary.Text = model.Salary.ToString();
                txtAdress.AppendText(model.Adress);
                picker1.SelectedDate = model.BirthDay;
                chisAdmin.IsChecked = model.isAdmin;
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri(@"Images/" + model.ImagePath, UriKind.RelativeOrAbsolute);
                image.EndInit();
                EmployeeImage.Source = image;
            }
            if (!UserStatic.isAdmin)
            {
                chisAdmin.IsEnabled = false;
                txtEmployeeNumber.IsEnabled = false;
                txtSalary.IsEnabled = false;
                cmbDepartment.IsEnabled = false;
                cmbPosition.IsEnabled = false;
            }
        }
        

        private void cmbDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int DepartmentId = Convert.ToInt32(cmbDepartment.SelectedValue);
            if (cmbDepartment.SelectedIndex != -1)
            {
                cmbPosition.ItemsSource = positions.Where(x => x.DepartmentId == DepartmentId).ToList();
                cmbPosition.DisplayMemberPath = "PositionName";
                cmbPosition.SelectedValuePath = "Id";
                cmbPosition.SelectedIndex = -1;
            }
        }
        OpenFileDialog dialog = new OpenFileDialog();
        private void buttonChoose_Click(object sender, RoutedEventArgs e)
        {
            if (dialog.ShowDialog() == true)
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri(dialog.FileName);
                image.EndInit();
                EmployeeImage.Source = image;
                txtImage.Text = dialog.FileName;
            }
        }

        
        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void txtEmployeeNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            {
                e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
            }
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            if (txtEmployeeNumber.Text.Trim() == "" || txtPassword.Text.Trim() == "" || txtName.Text.Trim() == ""
               || txtSurname.Text.Trim() == "" || txtSalary.Text.Trim() == "" || cmbDepartment.SelectedIndex == -1
               || cmbPosition.SelectedIndex == -1)
            {
                MessageBox.Show("Please fill the necessary areas");
            }
            else
            {
                if (model != null && model.Id != 0)
                {
                    Employee employee = db.Employees.Find(model.Id);
                    List<Employee> employeelist = db.Employees.Where(x => x.EmployeeNumber == Convert.ToInt32(txtEmployeeNumber.Text) &&
                     x.Id != employee.Id).ToList();
                    if (employeelist.Count > 0)
                    {
                        MessageBox.Show("This Employee number is already used by another Employee");
                    }
                    else
                    {


                        if (txtImage.Text.Trim() != "")
                        {
                            if (File.Exists(@"Images//" + employee.ImagePath))
                            {
                                File.Delete(@"Images//" + employee.ImagePath);
                                string filename = "";
                                string Unique = Guid.NewGuid().ToString();
                                filename += Unique + System.IO.Path.GetFileName(txtImage.Text);
                                File.Copy(txtImage.Text, @"Images//" + filename);
                                employee.ImagePath = filename;
                            }
                        }
                        employee.EmployeeNumber = Convert.ToInt32(txtEmployeeNumber.Text);
                        employee.Password = txtPassword.Text;
                        employee.IsAdmin = (bool)chisAdmin.IsChecked;
                        TextRange adres = new TextRange(txtAdress.Document.ContentStart, txtAdress.Document.ContentEnd);
                        employee.Address = adres.Text;
                        employee.Birthday = picker1.SelectedDate;
                        employee.DepartmentId = Convert.ToInt32(cmbDepartment.SelectedValue);
                        employee.PositionId = Convert.ToInt32(cmbPosition.SelectedValue);
                        employee.Name = txtName.Text;
                        employee.Surname = txtSurname.Text;
                        employee.Salary = Convert.ToInt32(txtSalary.Text);
                        db.SaveChanges();
                        MessageBox.Show("Employee was updated");


                    }
                }
                else
                {
                    var Uniquelist = db.Employees.Where(x => x.EmployeeNumber == Convert.ToInt32(txtEmployeeNumber.Text)).ToList();
                    if (Uniquelist.Count > 0)
                    {
                        MessageBox.Show("This Employee number is already used by another employee");
                    }
                    else
                    {

                        Employee employee = new Employee();
                        employee.EmployeeNumber = Convert.ToInt32(txtEmployeeNumber.Text);
                        employee.Password = txtPassword.Text;
                        employee.IsAdmin = (bool)chisAdmin.IsChecked;
                        TextRange adres = new TextRange(txtAdress.Document.ContentStart, txtAdress.Document.ContentEnd);
                        employee.Address = adres.Text;
                        employee.Birthday = picker1.SelectedDate;
                        employee.DepartmentId = Convert.ToInt32(cmbDepartment.SelectedValue);
                        employee.PositionId = Convert.ToInt32(cmbPosition.SelectedValue);
                        employee.Name = txtName.Text;
                        employee.Salary = Convert.ToInt32(txtSalary.Text);
                        employee.Surname = txtSurname.Text;
                        string filename = "";
                        string Unique = Guid.NewGuid().ToString();
                        filename += Unique + dialog.SafeFileName;
                        employee.ImagePath = filename;
                        db.Employees.Add(employee);
                        db.SaveChanges();
                        File.Copy(txtImage.Text, @"Images//" + filename);
                        MessageBox.Show("Employee was added");
                        txtEmployeeNumber.Clear();
                        txtPassword.Clear();
                        txtName.Clear();
                        txtSurname.Clear();
                        txtSalary.Clear();
                        picker1.SelectedDate = DateTime.Today;
                        cmbDepartment.SelectedIndex = -1;
                        cmbPosition.ItemsSource = positions;
                        cmbPosition.SelectedIndex = -1; txtAdress.Document.Blocks.Clear();
                        chisAdmin.IsChecked = false;
                        EmployeeImage.Source = new BitmapImage();
                        txtImage.Clear();
                    }
                }
               
               

                
            }
           
        }

        private void buttonCheck_Click(object sender, RoutedEventArgs e)
        {
            bool isUnique = false;
            var Uniquelist = db.Employees.Where(x => x.EmployeeNumber == Convert.ToInt32(txtEmployeeNumber.Text)).ToList();
            if (Uniquelist.Count > 0)
            {
                MessageBox.Show("This Employee number is already used by another employee");
            }
            else
            {
                MessageBox.Show("This Employee number is avaliable");
            }
        }

        
    }
    }



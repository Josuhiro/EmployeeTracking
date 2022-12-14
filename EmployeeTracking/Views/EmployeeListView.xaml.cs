using EmployeeTracking.DB;
using EmployeeTracking.ViewModels;
using Microsoft.EntityFrameworkCore;
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

namespace EmployeeTracking.Views
{
    /// <summary>
    /// Logika interakcji dla klasy EmployeeListView.xaml
    /// </summary>
    public partial class EmployeeListView : UserControl
    {
        public EmployeeListView()
        {
            InitializeComponent();
            FillDatagrid();
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            EmployeePage page = new EmployeePage();
            page.ShowDialog();
            FillDatagrid();

        }
       
        
        TrackingEmployeeContext db = new TrackingEmployeeContext();
        List<Position> positions = new List<Position>();
        List<EmployeeDetailModel> list = new List<EmployeeDetailModel>();
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            FillDatagrid();
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

       
        void FillDatagrid()
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
            list = db.Employees.Include(x => x.Position).Include(x => x.Department).Select(x => new EmployeeDetailModel()
            {
                Id = x.Id,
                Name = x.Name,
                Adress = x.Address,
                BirthDay = (DateTime)x.Birthday,
                DepartmentId = x.DepartmentId,
                DepartmentName = x.Department.DepartmentName,
                isAdmin = (bool)x.IsAdmin,
                Password = x.Password,
                PositionId = x.PositionId,
                PositionName = x.Position.PositionName,
                Salary = x.Salary,
                Surname = x.Surname,
                EmployeeNumber = x.EmployeeNumber,
                ImagePath = x.ImagePath
            }).ToList();
            gridEmployee.ItemsSource = list;
        }

        private void buttonSearch_Click(object sender, RoutedEventArgs e)
        {
            List<EmployeeDetailModel> searchlist = list;
            if (txtEmployeeNumber.Text.Trim() != "")
                searchlist = searchlist.Where(x => x.EmployeeNumber == Convert.ToInt32(txtEmployeeNumber.Text)).ToList();
            if (txtName.Text.Trim() != "")
                searchlist = searchlist.Where(x => x.Name.Contains(txtName.Text)).ToList();
            if (txtSurname.Text.Trim() != "")
                searchlist = searchlist.Where(x => x.Surname.Contains(txtSurname.Text)).ToList();
            if (cmbPosition.SelectedIndex != -1)
                searchlist = searchlist.Where(x => x.PositionId == Convert.ToInt32(cmbPosition.SelectedValue)).ToList();
            if (cmbDepartment.SelectedIndex != -1)
                searchlist = searchlist.Where(x => x.DepartmentId == Convert.ToInt32(cmbDepartment.SelectedValue)).ToList();
            gridEmployee.ItemsSource = searchlist;
        }

        private void buttonClear_Click(object sender, RoutedEventArgs e)
        {
            txtName.Clear();
            txtEmployeeNumber.Clear();
            txtSurname.Clear();
            cmbDepartment.SelectedIndex = -1;
            cmbPosition.ItemsSource = positions;
            cmbPosition.SelectedIndex = -1;
            gridEmployee.ItemsSource = list;
        }

        private void buttonUpdate_Click(object sender, RoutedEventArgs e)
        {
            EmployeeDetailModel model = (EmployeeDetailModel)gridEmployee.SelectedItem;
            EmployeePage page = new EmployeePage();
            page.model = model;
            page.ShowDialog();
            FillDatagrid();
        }


        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            EmployeeDetailModel model = (EmployeeDetailModel)gridEmployee.SelectedItem;
            if(model!=null && model.Id != 0)
            {
                if (MessageBox.Show("Are you sure to delete?", "Question", MessageBoxButton.YesNo
                , MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                     
                        
                        Employee emp = db.Employees.Find(model.Id);
                        db.Employees.Remove(emp);
                        db.SaveChanges();
                        MessageBox.Show("Employee was deleted");
                        FillDatagrid();

                    
                }
            }
           
        }
    }
}

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
    /// Logika interakcji dla klasy PermissionList.xaml
    /// </summary>
    public partial class PermissionList : UserControl
    {
        public PermissionList()
        {
            InitializeComponent();
        }
        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            PermissionPage page = new PermissionPage();
            page.ShowDialog();
            FillDataGrid();


        }
        List<PermissionDetailModel> permissions = new List<PermissionDetailModel>();
        void FillDataGrid()
        {
            permissions = db.Permissions.Include(x => x.Employee).Include(x => x.PermissionStateNavigation)
                .Select(x => new PermissionDetailModel()
                {
                    DayAmount = x.PermissionDay,
                    DepartmentId = x.Employee.DepartmentId,
                    EmployeeId = x.EmployeeId,
                    EndDate = x.PermissionEndDate,
                    Explanation = x.PermissionExplanation,
                    Id = x.Id,
                    Name = x.Employee.Name,
                    PermissionState = x.PermissionState,
                    PositionId = x.Employee.PositionId,
                    StartDate = (DateTime)x.PermissionStartDate,
                    StateName = x.PermissionStateNavigation.StateName,
                    Surname = x.Employee.Surname,
                    EmployeeNumber = x.Employee.EmployeeNumber

                }).OrderByDescending(x => x.StartDate).ToList();
            if (!UserStatic.isAdmin)
            {
                permissions = permissions.Where(x => x.EmployeeId == UserStatic.EmployeeId).ToList();
                txtEmployeeNumber.IsEnabled = false;
                txtName.IsEnabled = false;
                txtSurname.IsEnabled = false;
                cmbDepartment.IsEnabled = false;
                cmbPosition.IsEnabled = false;
                buttonDelete.Visibility = Visibility.Hidden;
                buttonApprove.Visibility = Visibility.Hidden;
                buttonDisapprove.Visibility = Visibility.Hidden;
                buttonAdd.SetValue(Grid.ColumnProperty, 1);

            }
            gridPermission.ItemsSource = permissions;
        }
        TrackingEmployeeContext db = new TrackingEmployeeContext();
        List<Position> positions = new List<Position>();
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            FillDataGrid();
            cmbDepartment.ItemsSource = db.Departments.ToList();
            cmbDepartment.DisplayMemberPath = "DepartmentName";
            cmbDepartment.SelectedValuePath = "Id";
            cmbDepartment.SelectedIndex = -1;
            positions = db.Positions.ToList();
            cmbPosition.ItemsSource = positions;
            cmbPosition.DisplayMemberPath = "PositionName";
            cmbPosition.SelectedValuePath = "Id";
            cmbPosition.SelectedIndex = -1;

            List<Permissionstate> states = db.Permissionstates.ToList();
            cmbState.ItemsSource = states;
            cmbState.DisplayMemberPath = "PermissionState";
            cmbState.SelectedValuePath = "Id";
            cmbState.SelectedIndex = -1;
        }

        private void buttonSearch_Click(object sender, RoutedEventArgs e)
        {
            List<PermissionDetailModel> search = permissions;
            if (txtEmployeeNumber.Text.Trim() != "")
                search = search.Where(x => x.EmployeeNumber == Convert.ToInt32(txtEmployeeNumber.Text)).ToList();
            if (txtName.Text.Trim() != "")
                search = search.Where(x => x.Name.Contains(txtName.Text)).ToList();
            if (txtSurname.Text.Trim() != "")
                search = search.Where(x => x.Surname.Contains(txtSurname.Text)).ToList();
            if (cmbDepartment.SelectedIndex != -1)
                search = search.Where(x => x.DepartmentId == Convert.ToInt32(cmbDepartment.SelectedValue)).ToList();
            if (cmbPosition.SelectedIndex != -1)
                search = search.Where(x => x.PositionId == Convert.ToInt32(cmbPosition.SelectedValue)).ToList();
            if (rbStart.IsChecked == true)
                search = search.Where(x => x.StartDate > dpStart.SelectedDate && x.StartDate < dpEnd.SelectedDate).ToList();
            if (rbEndDate.IsChecked == true)
                search = search.Where(x => x.EndDate > dpStart.SelectedDate && x.EndDate < dpEnd.SelectedDate).ToList();

            if (cmbState.SelectedIndex != -1)
                search = search.Where(x => x.PermissionState == Convert.ToInt32(cmbState.SelectedValue)).ToList();
            if (txtDayAmount.Text.Trim() != "")
                search = search.Where(x => x.DayAmount == Convert.ToInt32(txtDayAmount.Text)).ToList();
            gridPermission.ItemsSource = search;
        }

        private void buttonClear_Click(object sender, RoutedEventArgs e)
        {
            txtDayAmount.Clear();
            txtName.Clear();
            txtSurname.Clear();
            txtEmployeeNumber.Clear();
            cmbDepartment.SelectedIndex = -1;
            cmbState.SelectedIndex = -1;
            cmbPosition.ItemsSource = positions;
            cmbPosition.SelectedIndex = -1;
            dpEnd.SelectedDate = DateTime.Today;
            dpStart.SelectedDate = DateTime.Today;
            rbEndDate.IsChecked = false;
            rbStart.IsChecked = false;
            gridPermission.ItemsSource = permissions;
        }

        private void buttonUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (model != null && model.Id != 0)
            {
                PermissionPage page = new PermissionPage();
                page.model = model;
                page.ShowDialog();
                FillDataGrid();
            }
            else
                MessageBox.Show("Pkease select a permission from table");
        }
        PermissionDetailModel model = new PermissionDetailModel();
        private void gridPermission_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            model = (PermissionDetailModel)gridPermission.SelectedItem;
        }

        private void buttonApprove_Click(object sender, RoutedEventArgs e)
        {
            if (model != null && model.Id != 0 && model.PermissionState == Definitions.PermissionStates.Working)
            {
                Permission permission = db.Permissions.Find(model.Id);
                permission.PermissionState = Definitions.PermissionStates.Approved;
                db.SaveChanges();
                MessageBox.Show("Permission was approved");
                FillDataGrid();
            }
        }

        private void buttonDisapprove_Click(object sender, RoutedEventArgs e)
        {
            if (model != null && model.Id != 0 && model.PermissionState == Definitions.PermissionStates.Working)
            {
                Permission permission = db.Permissions.Find(model.Id);
                permission.PermissionState = Definitions.PermissionStates.Disapproved;
                db.SaveChanges();
                MessageBox.Show("Permission was disapproved");
                FillDataGrid();
            }
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (model != null && model.Id != 0)
            {
                if (MessageBox.Show("Are you sure to delete?", "Question", MessageBoxButton.YesNo
                    , MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    Permission permission = db.Permissions.Find(model.Id);
                    db.Permissions.Remove(permission);
                    db.SaveChanges();
                    MessageBox.Show("Permission was deleted");
                    FillDataGrid();
                }
            }
            else
                MessageBox.Show("Please select an item from table");
        }
    }
}

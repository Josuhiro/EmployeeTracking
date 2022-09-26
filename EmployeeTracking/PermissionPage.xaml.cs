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
using System.Windows.Shapes;

namespace EmployeeTracking
{
    /// <summary>
    /// Logika interakcji dla klasy PermissionPage.xaml
    /// </summary>
    public partial class PermissionPage : Window
    {
        public PermissionPage()
        {
            InitializeComponent();
        }
        TimeSpan tspermissionday = new TimeSpan();
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtEmployeeNumber.Text = UserStatic.EmployeeNumber.ToString();
            if (model != null && model.Id != 0)
            {
                txtEmployeeNumber.Text = model.EmployeeNumber.ToString();
                txtDayAmount.Text = model.DayAmount.ToString();
                txtExplanation.Text = model.Explanation;
                dpEnd.SelectedDate = model.EndDate;
                dpStart.SelectedDate = model.StartDate;
            }
        }

        private void dpStart_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dpEnd.SelectedDate != null)
            {
                tspermissionday = (TimeSpan)(dpEnd.SelectedDate - dpStart.SelectedDate);
                txtDayAmount.Text = tspermissionday.TotalDays.ToString();
            }
        }

        private void dpEnd_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dpStart.SelectedDate != null)
            {
                tspermissionday = (TimeSpan)(dpEnd.SelectedDate - dpStart.SelectedDate);
                txtDayAmount.Text = tspermissionday.TotalDays.ToString();
            }
        }
        TrackingEmployeeContext db = new TrackingEmployeeContext();
        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            if (txtDayAmount.Text.Trim() == "")
            {
                MessageBox.Show("Please select start and end date");
            }
                
            else if (Convert.ToInt32(txtDayAmount.Text) <= 0)
            {
                MessageBox.Show("Permission day must be bigger than zero");
            }
            else if (txtExplanation.Text.Trim() == "")
            {
                MessageBox.Show("Please write your permission reason");
            }

            else
            {
                if (model != null && model.Id != 0)
                {
                    Permission permission = db.Permissions.Find(model.Id);
                    permission.PermissionStartDate = (DateTime)dpStart.SelectedDate;
                    permission.PermissionEndDate = (DateTime)dpEnd.SelectedDate;
                    permission.PermissionDay = Convert.ToInt32(txtDayAmount.Text);
                    permission.PermissionExplanation = txtExplanation.Text;
                    db.SaveChanges();
                    MessageBox.Show("Permssion was updated");
                }
                else
                {
                    Permission permission = new Permission();
                    permission.EmployeeId = UserStatic.EmployeeId;
                    permission.EmployeeNumber = UserStatic.EmployeeNumber;
                    permission.PermissionState = Definitions.PermissionStates.Working;
                    permission.PermissionStartDate = (DateTime)dpStart.SelectedDate;
                    permission.PermissionEndDate = (DateTime)dpEnd.SelectedDate;
                    permission.PermissionDay = Convert.ToInt32(txtDayAmount.Text);
                    permission.PermissionExplanation = txtExplanation.Text;
                    db.Permissions.Add(permission);
                    db.SaveChanges();
                    MessageBox.Show("Permission was Added");
                    dpEnd.SelectedDate = DateTime.Today;
                    dpStart.SelectedDate = DateTime.Today;
                    txtExplanation.Clear();
                    txtDayAmount.Clear();
                }

            }
        }

        public PermissionDetailModel model;
        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

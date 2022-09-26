using EmployeeTracking.DB;
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
    /// Logika interakcji dla klasy DepartmentList.xaml
    /// </summary>
    public partial class DepartmentList : UserControl
    {
        public DepartmentList()
        {
            InitializeComponent();
            using (TrackingEmployeeContext db = new TrackingEmployeeContext())
            {
                List<Department> list = db.Departments.OrderBy(x => x.DepartmentName).ToList();
                gridDepartment.ItemsSource = list;
            }
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            DepartmentPage page = new DepartmentPage();
            page.ShowDialog();
            using (TrackingEmployeeContext db = new TrackingEmployeeContext())
            {
                List<Department> list = db.Departments.OrderBy(x => x.DepartmentName).ToList();
                gridDepartment.ItemsSource = list;
            }
        }

        private void buttonUpdate_Click(object sender, RoutedEventArgs e)
        {
            Department dpt = (Department)gridDepartment.SelectedItem;
            DepartmentPage page = new DepartmentPage();
            page.department = dpt;
            page.ShowDialog();
            using (TrackingEmployeeContext db = new TrackingEmployeeContext())
            {
                gridDepartment.ItemsSource = db.Departments.OrderBy(x => x.DepartmentName).ToList();
            }
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            Department model = (Department)gridDepartment.SelectedItem;
            if (model != null && model.Id != 0)
            {
                if (MessageBox.Show("Are you sure to delete", "Question", MessageBoxButton.YesNo
                    , MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    TrackingEmployeeContext db = new TrackingEmployeeContext();
                    List<Position> positions = db.Positions.Where(x => x.DepartmentId == model.Id).ToList();
                    foreach (var item in positions)
                    {
                        db.Positions.Remove(item);
                    }
                    db.SaveChanges();

                    Department department = db.Departments.Find(model.Id);
                    db.Departments.Remove(department);
                    db.SaveChanges();
                    MessageBox.Show("Department was deleted");
                    gridDepartment.ItemsSource = db.Departments.ToList();
                }

            }
        }
    }
}

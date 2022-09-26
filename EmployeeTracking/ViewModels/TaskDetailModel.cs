using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTracking.ViewModels
{
    public class TaskDetailModel
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime TaskStartDate { get; set; }
        public DateTime? TaskEndDate { get; set; }
        public int TaskState { get; set; }
        public string TaskTitle { get; set; }
        public string TaskContent { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int EmployeeNumber { get; set; }
        public string StateName { get; set; }
        public int DepartmentId { get; set; }
        public int PositionId { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace EmployeeTracking.DB
{
    public partial class Task
    {
        public int Id { get; set; }
        public string TaskTitle { get; set; } = null!;
        public string TaskContent { get; set; } = null!;
        public DateTime TaskStartDate { get; set; }
        public DateTime TaskEndDate { get; set; }
        public int TaskState { get; set; }
        public int EmployeeId { get; set; }

        public virtual Employee Employee { get; set; } = null!;
        public virtual Taskstate TaskStateNavigation { get; set; } = null!;
    }
}

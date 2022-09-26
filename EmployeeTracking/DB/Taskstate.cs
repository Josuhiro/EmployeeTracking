using System;
using System.Collections.Generic;

namespace EmployeeTracking.DB
{
    public partial class Taskstate
    {
        public Taskstate()
        {
            Tasks = new HashSet<Task>();
        }

        public int Id { get; set; }
        public string StateName { get; set; } = null!;

        public virtual ICollection<Task> Tasks { get; set; }
    }
}

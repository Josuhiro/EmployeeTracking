using System;
using System.Collections.Generic;

namespace EmployeeTracking.DB
{
    public partial class Month
    {
        public Month()
        {
            Salaries = new HashSet<Salary>();
        }

        public int Id { get; set; }
        public string MonthName { get; set; } = null!;

        public virtual ICollection<Salary> Salaries { get; set; }
    }
}

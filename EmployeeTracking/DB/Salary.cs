using System;
using System.Collections.Generic;

namespace EmployeeTracking.DB
{
    public partial class Salary
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public int Year { get; set; }
        public int MonthId { get; set; }
        public int EmployeeId { get; set; }

        public virtual Employee Employee { get; set; } = null!;
        public virtual Month Month { get; set; } = null!;
    }
}

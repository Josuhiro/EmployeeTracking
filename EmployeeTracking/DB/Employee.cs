using System;
using System.Collections.Generic;

namespace EmployeeTracking.DB
{
    public partial class Employee
    {
        public Employee()
        {
            Permissions = new HashSet<Permission>();
            Salaries = new HashSet<Salary>();
            Tasks = new HashSet<Task>();
        }

        public int Id { get; set; }
        public int EmployeeNumber { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string ImagePath { get; set; } = null!;
        public int Salary { get; set; }
        public DateTime? Birthday { get; set; }
        public string? Address { get; set; }
        public string? Password { get; set; }
        public bool? IsAdmin { get; set; }
        public int DepartmentId { get; set; }
        public int PositionId { get; set; }

        public virtual Department Department { get; set; } = null!;
        public virtual Position Position { get; set; } = null!;
        public virtual ICollection<Permission> Permissions { get; set; }
        public virtual ICollection<Salary> Salaries { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
    }
}

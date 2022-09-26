using System;
using System.Collections.Generic;

namespace EmployeeTracking.DB
{
    public partial class Permissionstate
    {
        public Permissionstate()
        {
            Permissions = new HashSet<Permission>();
        }

        public int Id { get; set; }
        public string StateName { get; set; } = null!;

        public virtual ICollection<Permission> Permissions { get; set; }
    }
}

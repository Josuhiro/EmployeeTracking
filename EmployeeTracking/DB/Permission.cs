using System;
using System.Collections.Generic;

namespace EmployeeTracking.DB
{
    public partial class Permission
    {
        public int Id { get; set; }
        public DateTime PermissionStartDate { get; set; }
        public DateTime PermissionEndDate { get; set; }
        public int PermissionState { get; set; }
        public string? PermissionExplanation { get; set; }
        public int PermissionDay { get; set; }
        public int EmployeeId { get; set; }
        public int EmployeeNumber { get; set; }

        public virtual Employee Employee { get; set; } = null!;
        public virtual Permissionstate PermissionStateNavigation { get; set; } = null!;
    }
}

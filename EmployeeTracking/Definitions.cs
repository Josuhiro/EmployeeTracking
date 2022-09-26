using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTracking
{
    public static class Definitions
    {
        public static class TaskStates
        {
            public static int Working = 1;
            public static int Delivered = 2;
            public static int Approved = 3;
        }
        public static class PermissionStates
        {
            public static int Working = 1;
            public static int Approved = 2;
            public static int Disapproved = 3;
        }
    }
}

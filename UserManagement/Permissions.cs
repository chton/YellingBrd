using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserManagement
{
    public enum Permissions : int
    {
        Ignore = 0,
        Basic = 10,
        HalfOp = 20,
        Operator = 40,
        Admin = 80,
        SuperAdmin = 160,
        Deity = 9000   
    }
}

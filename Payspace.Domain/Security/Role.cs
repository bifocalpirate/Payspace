using System;
using System.Collections.Generic;
using System.Text;

namespace Payspace.Domain.Security
{
    public class Role:BaseEntity<long>
    {
        public Role()
        {
            Users = new HashSet<UserRole>();
        }
        public string RoleName { get; set; }
        public HashSet<UserRole> Users { get; set; }
    }
}

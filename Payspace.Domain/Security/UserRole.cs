using System;
using System.Collections.Generic;
using System.Text;

namespace Payspace.Domain.Security
{
    public class UserRole : BaseEntity<long>
    {
        public long UserId { get; set; }
        public long RoleId { get; set; }
        public User User { get; set; }
        public Role Role { get; set; }
    }
}

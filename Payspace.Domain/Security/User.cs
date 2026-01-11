using System;
using System.Collections.Generic;
using System.Text;

namespace Payspace.Domain.Security
{
    public class User : BaseEntity<long>
    {
        public User()
        {
            UserRoles = new HashSet<UserRole>();           
        }      
        public string EmailAddress { get; set; }
        public string PasswordHash { get; set; }
        public virtual HashSet<UserRole> UserRoles { get; set; }
    }
}

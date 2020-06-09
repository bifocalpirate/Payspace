using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payspace.UI.Models.DTO
{
    public class UserForAuthentication
    {        
        public string EmailAddress { get; set; }        
        public string PasswordHash { get; set; }   
        
    }
}

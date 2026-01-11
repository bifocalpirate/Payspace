using Microsoft.Extensions.Localization.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payspace.API.Models.DTO
{
    public class UserForAuthentication
    {
        public long Id { get; set; }
        public string EmailAddress { get; set; }
        public string PasswordHash { get; set; } 
        //just a shortcut
        public string[] Roles { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payspace.UI.Models.DTO
{
    public class AuthenticatedUser
    {
        public string JsonToken { get; set; }
        public string EmailAddress { get; set; }
        public long Id { get; set; }

        //this is just to make it easy for the UI. The actual roles are encrypted in the token.
        public string[] Roles { get; set; }
    }
}

using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace DAL.Entities
{
    public class User : IdentityUser<string>
    {
        public virtual IEnumerable<Role> Roles { get; set; }
    }
}

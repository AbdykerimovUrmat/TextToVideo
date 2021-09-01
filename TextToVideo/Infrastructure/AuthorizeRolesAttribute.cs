using Microsoft.AspNetCore.Authorization;

namespace Uploader.Infrastructure
{
    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        public AuthorizeRolesAttribute(string roles)
        {
            Roles = roles;
        }
    }
}

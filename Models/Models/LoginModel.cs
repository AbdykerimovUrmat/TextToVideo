using System;

namespace Models.Models
{
    public static class LoginModel
    {
        public class LoginIn
        {
            public string Email { get; set; }

            public string Password { get; set; }
        }

        public class LoginOut
        {
            public string AccessToken { get; set; }

            public DateTime ExpirationDate { get; set; }

            public string UserName { get; set; }
        }
    }
}

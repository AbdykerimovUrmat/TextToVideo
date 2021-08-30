using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Models.Models;

namespace Uploader.Services
{
    public class AuthService
    {
        private UserManager<User> UserManager { get; set; }

        public AuthService(UserManager<User> userManager)
        {
            UserManager = userManager;
        }

        public async Task<LoginModel.LoginOut> AccessToken(LoginModel.LoginIn model) 
        {
            var (user, claims, _) = await UserClaimsRoleNames(model.Email, model.Password);

            var (accessToken, expirationDate) = AccessToken(claims);

            return new LoginModel.LoginOut
            {
                AccessToken = accessToken,
                ExpirationDate = expirationDate,
                UserName = user.UserName
            };
        }

        private async Task<(User User, IEnumerable<Claim> Claims, IEnumerable<string> RoleNames)> UserClaimsRoleNames(string email, string password)
        {
            var user = await UserManager.FindByNameAsync(email);
            var roles = await UserManager.GetRolesAsync(user);
            if(!await UserManager.CheckPasswordAsync(user, password))
            {
                throw new Exception("Password is incorrect");
            }
            var claims = new List<Claim>
            { 
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName),
            };
            claims.AddRange(roles.Select(x => new Claim(ClaimTypes.Role, x)));

            return (user, claims, roles);
        }

        public (string AccessToken, DateTime ExpirationDate) AccessToken(IEnumerable<Claim> claims)
        {
            var utcNow = DateTime.UtcNow;
            var expirationDate = utcNow.Add(TimeSpan.FromMinutes(4320));
            var jwt = new JwtSecurityToken(
                issuer: "me",
                audience: "you",
                claims: claims,
                notBefore: utcNow,
                expires: expirationDate,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes("6f912127-6566-4378-acc7-79ba0d3fe892")), SecurityAlgorithms.HmacSha256Signature)
                );

            return (new JwtSecurityTokenHandler().WriteToken(jwt), expirationDate);
        }
    }
}

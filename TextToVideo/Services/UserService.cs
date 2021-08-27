
using DAL.EF;
using DAL.Entities;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Models.Models;
using System.Threading.Tasks;

namespace Uploader.Services
{
    public class UserService
    {
        private UserManager<User> UserManager { get; set; }

        public UserService(UserManager<User> userManager)
        {
            UserManager = userManager;
        }

        public async Task Add(UserModel.AddIn model)
        {
            var user = new User { Email = model.Email, UserName = model.Email };
            await UserManager.CreateAsync(user, model.Password);
            await UserManager.AddToRoleAsync(user, "user");
        }
    }
}

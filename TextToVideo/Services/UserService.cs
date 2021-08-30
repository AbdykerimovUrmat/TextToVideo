using System.Threading.Tasks;
using Common.Exceptions;
using Common.Extensions;
using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Models.Models;

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
            if((await UserManager.FindByNameAsync(model.Email)).IsNotNull())
            {
                throw new InnerException($"User with email {model.Email} already exists.", "10001", nameof(model.Email));
            }
            await UserManager.CreateAsync(user, model.Password);
            await UserManager.AddToRoleAsync(user, "user");
        }
    }
}

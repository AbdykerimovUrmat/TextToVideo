using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using Uploader.Services;

namespace Uploader.Controllers
{
    [ApiController]
    [Route("api/User")]
    [AllowAnonymous]
    public class UserController : ControllerBase
    {
        private UserService UserService { get; set; }

        public UserController(UserService userService)
        {
            UserService = userService;
        }

        [Route("")]
        [HttpPost]
        public async Task Add(UserModel.AddIn model)
        {
            await UserService.Add(model);
        }
    }
}

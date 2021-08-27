using Microsoft.AspNetCore.Mvc;
using Models.Models;
using System.Threading.Tasks;
using Uploader.Services;

namespace Uploader.Controllers
{
    [ApiController]
    [Route("api/User")]
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

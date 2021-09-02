using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        [HttpPost]
        [Route("")]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BadRequestModel), StatusCodes.Status500InternalServerError)]
        public async Task Add(UserModel.AddIn model)
        {
            await UserService.Add(model);
        }
    }
}

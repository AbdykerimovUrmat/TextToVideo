using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using Uploader.Services;

namespace Uploader.Controllers
{
    [Route("api/Auth")]
    [AllowAnonymous]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private AuthService AuthService { get; set; }

        public AuthController(AuthService authService)
        {
            AuthService = authService;
        }

        [HttpPost]
        public async Task<string> Login(LoginModel.LoginIn model)
        {
            return await AuthService.AccessToken(model);
        }
    }
}

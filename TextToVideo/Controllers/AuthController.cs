using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        /// <summary>
        /// Get accesstoken by login and password (Anonymous)
        /// </summary>
        /// <param name="model">Login data</param>
        [HttpPost]
        [Route("")]
        [ProducesResponseType(typeof(LoginModel.LoginOut), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BadRequestModel), StatusCodes.Status500InternalServerError)]
        public async Task<LoginModel.LoginOut> Login(LoginModel.LoginIn model)
        {
            return await AuthService.AccessToken(model);
        }
    }
}

using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using Uploader.Infrastructure;
using Uploader.Services;

namespace Uploader.Controllers
{
    [ApiController]
    [Route("api/Request")]
    [AuthorizeRoles("Admin, User")]
    public class RequestController : ControllerBase
    {
        private RequestService RequestService { get; set; }

        public RequestController(RequestService requestService)
        {
            RequestService = requestService;
        }

        [HttpPost]
        [Route("")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BadRequestModel), StatusCodes.Status500InternalServerError)]
        public async Task<int> Add(RequestModel.AddIn model)
        {
            return await RequestService.Add(model);
        }
    }
}

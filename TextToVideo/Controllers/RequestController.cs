using Microsoft.AspNetCore.Mvc;
using Models.Models;
using System.Threading.Tasks;
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
        public async Task<int> Add(RequestModel.AddIn model)
        {
            return await RequestService.Add(model);
        }
    }
}

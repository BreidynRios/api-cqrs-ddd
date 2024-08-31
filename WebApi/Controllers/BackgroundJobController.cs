using Application.Commons.Utils;
using Application.Interfaces.ServicesClients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/v1/background-job")]
    [ApiController]
    [Authorize(AuthenticationSchemes =
        $"{GeneralConstants.DEFAULT_SCHEME_BEARER_TOKEN},{GeneralConstants.DEFAULT_SCHEME_API_KEY}")]
    public class BackgroundJobController : ControllerBase
    {
        private readonly IBackgroundJobServiceClient _backgroundJobServiceClient;

        public BackgroundJobController(IBackgroundJobServiceClient backgroundJobServiceClient)
        {
            _backgroundJobServiceClient = backgroundJobServiceClient;
        }

        [HttpDelete("{taskId}")]
        public IActionResult CancelTask(Guid taskId)
        {
            var response = _backgroundJobServiceClient.CancelJob(taskId);
            return response ? Ok() : NotFound();
        }
    }
}

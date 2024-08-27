using Application.Interfaces.ServicesClients;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/v1/background-job")]
    [ApiController]
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

using Application.Features.Securities.Queries.GetBearerToken;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("token")]
        public async Task<IActionResult> TokenAsync(GetBearerTokenQuery query)
        {
            return Ok(await _mediator.Send(query));
        }
    }
}

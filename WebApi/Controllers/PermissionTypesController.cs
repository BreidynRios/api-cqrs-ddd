using Application.Commons.Utils;
using Application.Features.PermissionTypes.Queries.GetAllPermissionTypes;
using Application.Features.PermissionTypes.Queries.GetPermissionTypeById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/v1/permission-types")]
    [ApiController]
    [Authorize(AuthenticationSchemes =
        $"{GeneralConstants.DEFAULT_SCHEME_BEARER_TOKEN},{GeneralConstants.DEFAULT_SCHEME_API_KEY}")]
    public class PermissionTypesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PermissionTypesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetAllPermissionTypesDto>>> GetAsync(
            CancellationToken cancellationToken)
        {
            return Ok(await _mediator.Send(new GetAllPermissionTypesQuery(), cancellationToken));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetPermissionTypeByIdDto>> GetPermissionTypeByIdAsync(
            int id, CancellationToken cancellationToken)
        {
            return Ok(await _mediator.Send(new GetPermissionTypeByIdQuery(id), cancellationToken));
        }
    }
}

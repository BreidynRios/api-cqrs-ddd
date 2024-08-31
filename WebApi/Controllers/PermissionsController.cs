using Application.Commons.Utils;
using Application.DTOs.ServicesClients.Kafka;
using Application.Features.Permissions.Commands.CreatePermission;
using Application.Features.Permissions.Commands.DeletePermission;
using Application.Features.Permissions.Commands.UpdatePermission;
using Application.Features.Permissions.Queries.GetPermissionById;
using Application.Interfaces.ServicesClients;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/v1/permissions")]
    [ApiController]
    [Authorize(AuthenticationSchemes =
        $"{GeneralConstants.DEFAULT_SCHEME_BEARER_TOKEN},{GeneralConstants.DEFAULT_SCHEME_API_KEY}")]
    public class PermissionsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IKafkaServiceClient _kafkaServiceClient;

        public PermissionsController(
            IMediator mediator,
            IKafkaServiceClient kafkaServiceClient)
        {
            _mediator = mediator;
            _kafkaServiceClient = kafkaServiceClient;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetPermissionByIdDto>> GetPermissionByIdQueryAsync(
            int id, CancellationToken cancellationToken)
        {
            await _kafkaServiceClient.ProduceAsync(
                new PermissionTopicParameter<int>(GeneralConstants.GET, id), cancellationToken);

            return Ok(await _mediator.Send(new GetPermissionByIdQuery(id), cancellationToken));
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateAsync(
            CreatePermissionCommand command, CancellationToken cancellationToken)
        {
            await _kafkaServiceClient.ProduceAsync(new PermissionTopicParameter<CreatePermissionCommand>
                (GeneralConstants.REQUEST, command), cancellationToken);

            return Ok(await _mediator.Send(command, cancellationToken));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, UpdatePermissionCommand command,
            CancellationToken cancellationToken)
        {
            command.Id = id;
            await _kafkaServiceClient.ProduceAsync(new PermissionTopicParameter<UpdatePermissionCommand>
                (GeneralConstants.MODIFY, command), cancellationToken);
            await _mediator.Send(command, cancellationToken);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeletePermissionCommand(id), cancellationToken);
            return Ok();
        }
    }
}

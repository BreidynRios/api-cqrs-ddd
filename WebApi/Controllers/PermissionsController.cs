using Application.Commons.Utils;
using Application.DTOs.ServicesClients.Kafka;
using Application.Features.Permissions.Commands.CreatePermission;
using Application.Features.Permissions.Commands.UpdatePermission;
using Application.Features.Permissions.Queries.GetPermissionById;
using Application.Interfaces.ServicesClients;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/v1/permissions")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IKafkaServiceClient _kafkaServiceClient;
        private readonly ILogger<PermissionsController> _logger;

        public PermissionsController(
            IMediator mediator,
            IKafkaServiceClient kafkaServiceClient,
            ILogger<PermissionsController> logger)
        {
            _mediator = mediator;
            _kafkaServiceClient = kafkaServiceClient;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetPermissionByIdDto>> GetPermissionByIdQueryAsync(
            int id, CancellationToken cancellationToken)
        {
            await _kafkaServiceClient.ProduceAsync(
                new PermissionTopicParameter<int>(Constants.Get, id), cancellationToken);
            _logger.LogInformation($"Method: {nameof(GetPermissionByIdQueryAsync)}, Parameter: {new { id }}");

            return Ok(await _mediator.Send(new GetPermissionByIdQuery(id)));
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateAsync(
            CreatePermissionCommand command, CancellationToken cancellationToken)
        {
            await _kafkaServiceClient.ProduceAsync(new PermissionTopicParameter<CreatePermissionCommand>
                (Constants.Request, command), cancellationToken);
            _logger.LogInformation($"Method: {nameof(CreateAsync)}, Parameter: {command}");

            return Ok(await _mediator.Send(command));
        }

        [HttpPut("{id}")]
        public async Task UpdateAsync(int id, UpdatePermissionCommand command,
            CancellationToken cancellationToken)
        {
            command.Id = id;
            await _kafkaServiceClient.ProduceAsync(new PermissionTopicParameter<UpdatePermissionCommand>
                (Constants.Modify, command), cancellationToken);
            _logger.LogInformation($"Method: {nameof(UpdateAsync)}, Parameter: {command}");
            await _mediator.Send(command);
        }
    }
}

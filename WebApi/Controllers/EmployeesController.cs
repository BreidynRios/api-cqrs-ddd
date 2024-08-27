using Application.Features.Employees.Commands.CreateEmployee;
using Application.Features.Employees.Queries.ExportEmployeesBackgroundJob;
using Application.Features.Employees.Queries.GetAllEmployees;
using Application.Features.Employees.Queries.GetEmployeeById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/v1/employees")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EmployeesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetAllEmployeesDto>>> GetAsync(
            CancellationToken cancellationToken)
        {
            var result =  await _mediator.Send(new GetAllEmployeesQuery(), cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetEmployeeByIdDto>> GetEmployeeByIdAsync(
            int id, CancellationToken cancellationToken)
        {
            return Ok(await _mediator.Send(new GetEmployeeByIdQuery(id), cancellationToken));
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateAsync(
            [FromBody] CreateEmployeeCommand command, CancellationToken cancellationToken)
        {
            return Ok(await _mediator.Send(command, cancellationToken));
        }

        [HttpPost]
        [Route("export-file")]
        public async Task<IActionResult> ExportFileAsync(
            [FromBody] ExportEmployeesBackgroundJobQuery query, CancellationToken cancellationToken)
        {
            return Ok(await _mediator.Send(query, cancellationToken));
        }
    }
}

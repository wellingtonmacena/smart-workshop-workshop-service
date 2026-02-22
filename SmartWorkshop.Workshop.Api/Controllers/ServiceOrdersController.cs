using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartWorkshop.Workshop.Application.UseCases.ServiceOrders.Create;
using SmartWorkshop.Workshop.Application.UseCases.ServiceOrders.Get;
using SmartWorkshop.Workshop.Application.UseCases.ServiceOrders.List;
using SmartWorkshop.Workshop.Domain.DTOs.ServiceOrders;
using SmartWorkshop.Workshop.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace SmartWorkshop.Workshop.Api.Controllers;

/// <summary>
/// Controller for managing service orders
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class ServiceOrdersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ServiceOrdersController> _logger;

    public ServiceOrdersController(IMediator mediator, ILogger<ServiceOrdersController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get service order by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ServiceOrderDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetById([FromRoute][Required] Guid id, CancellationToken cancellationToken)
    {
        var query = new GetServiceOrderByIdQuery(id);
        var result = await _mediator.Send(query, cancellationToken);

        if (!result.IsSuccess)
        {
            return StatusCode((int)result.StatusCode, new ProblemDetails
            {
                Title = "Error retrieving service order",
                Detail = string.Join(", ", result.Reasons.Select(r => r.Message)),
                Status = (int)result.StatusCode
            });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// List all service orders (paginated)
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IPaginate<ServiceOrderDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> List([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 15, CancellationToken cancellationToken = default)
    {
        var query = new ListServiceOrdersQuery(pageNumber, pageSize);
        var result = await _mediator.Send(query, cancellationToken);

        if (!result.IsSuccess)
        {
            return StatusCode((int)result.StatusCode, new ProblemDetails
            {
                Title = "Error listing service orders",
                Detail = string.Join(", ", result.Reasons.Select(r => r.Message)),
                Status = (int)result.StatusCode
            });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Create a new service order
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ServiceOrderDto), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Create([FromBody][Required] CreateServiceOrderCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess)
        {
            return StatusCode((int)result.StatusCode, new ProblemDetails
            {
                Title = "Error creating service order",
                Detail = string.Join(", ", result.Reasons.Select(r => r.Message)),
                Status = (int)result.StatusCode
            });
        }

        return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result.Data);
    }
}

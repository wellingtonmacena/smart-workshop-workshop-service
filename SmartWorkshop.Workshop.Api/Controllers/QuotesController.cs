using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartWorkshop.Workshop.Application.UseCases.Quotes.Create;
using SmartWorkshop.Workshop.Application.UseCases.Quotes.Get;
using SmartWorkshop.Workshop.Application.UseCases.Quotes.GetAll;
using SmartWorkshop.Workshop.Application.UseCases.Quotes.Approve;
using SmartWorkshop.Workshop.Application.UseCases.Quotes.Reject;
using SmartWorkshop.Workshop.Application.UseCases.Quotes.AddService;
using SmartWorkshop.Workshop.Application.UseCases.Quotes.AddSupply;
using SmartWorkshop.Workshop.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace SmartWorkshop.Workshop.Api.Controllers;

/// <summary>
/// Controller for managing quotes (orçamentos)
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class QuotesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<QuotesController> _logger;

    public QuotesController(IMediator mediator, ILogger<QuotesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get all quotes
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of all quotes</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Quote>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var query = new GetAllQuotesQuery();
        var result = await _mediator.Send(query, cancellationToken);

        if (!result.IsSuccess)
        {
            return StatusCode((int)result.StatusCode, new ProblemDetails
            {
                Title = "Error retrieving quotes",
                Detail = string.Join(", ", result.Reasons.Select(r => r.Message)),
                Status = (int)result.StatusCode
            });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Get quote by ID
    /// </summary>
    /// <param name="id">Quote ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Quote details</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(Quote), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetById([FromRoute][Required] Guid id, CancellationToken cancellationToken)
    {
        var query = new GetQuoteByIdQuery(id);
        var result = await _mediator.Send(query, cancellationToken);

        if (!result.IsSuccess)
        {
            return StatusCode((int)result.StatusCode, new ProblemDetails
            {
                Title = "Error retrieving quote",
                Detail = string.Join(", ", result.Reasons.Select(r => r.Message)),
                Status = (int)result.StatusCode
            });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Create a new quote for a service order
    /// </summary>
    /// <param name="command">Create quote command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created quote</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Quote), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Create([FromBody][Required] CreateQuoteCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess)
        {
            return StatusCode((int)result.StatusCode, new ProblemDetails
            {
                Title = "Error creating quote",
                Detail = string.Join(", ", result.Reasons.Select(r => r.Message)),
                Status = (int)result.StatusCode
            });
        }

        return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result.Data);
    }

    /// <summary>
    /// Add a service to the quote
    /// </summary>
    /// <param name="id">Quote ID</param>
    /// <param name="command">Add service command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated quote</returns>
    [HttpPost("{id:guid}/services")]
    [ProducesResponseType(typeof(Quote), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> AddService(
        [FromRoute][Required] Guid id,
        [FromBody][Required] AddServiceToQuoteCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.QuoteId)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Invalid request",
                Detail = "Quote ID in route does not match command",
                Status = (int)HttpStatusCode.BadRequest
            });
        }

        var result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess)
        {
            return StatusCode((int)result.StatusCode, new ProblemDetails
            {
                Title = "Error adding service to quote",
                Detail = string.Join(", ", result.Reasons.Select(r => r.Message)),
                Status = (int)result.StatusCode
            });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Add a supply (part/material) to the quote
    /// </summary>
    /// <param name="id">Quote ID</param>
    /// <param name="command">Add supply command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated quote</returns>
    [HttpPost("{id:guid}/supplies")]
    [ProducesResponseType(typeof(Quote), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> AddSupply(
        [FromRoute][Required] Guid id,
        [FromBody][Required] AddSupplyToQuoteCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.QuoteId)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Invalid request",
                Detail = "Quote ID in route does not match command",
                Status = (int)HttpStatusCode.BadRequest
            });
        }

        var result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess)
        {
            return StatusCode((int)result.StatusCode, new ProblemDetails
            {
                Title = "Error adding supply to quote",
                Detail = string.Join(", ", result.Reasons.Select(r => r.Message)),
                Status = (int)result.StatusCode
            });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Approve a quote
    /// </summary>
    /// <param name="id">Quote ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Approved quote</returns>
    [HttpPut("{id:guid}/approve")]
    [ProducesResponseType(typeof(Quote), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Approve([FromRoute][Required] Guid id, CancellationToken cancellationToken)
    {
        var command = new ApproveQuoteCommand(id);
        var result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess)
        {
            return StatusCode((int)result.StatusCode, new ProblemDetails
            {
                Title = "Error approving quote",
                Detail = string.Join(", ", result.Reasons.Select(r => r.Message)),
                Status = (int)result.StatusCode
            });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Reject a quote
    /// </summary>
    /// <param name="id">Quote ID</param>
    /// <param name="request">Rejection request with optional reason</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Rejected quote</returns>
    [HttpPut("{id:guid}/reject")]
    [ProducesResponseType(typeof(Quote), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Reject(
        [FromRoute][Required] Guid id,
        [FromBody] RejectQuoteRequest? request,
        CancellationToken cancellationToken)
    {
        var command = new RejectQuoteCommand(id, request?.Reason);
        var result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess)
        {
            return StatusCode((int)result.StatusCode, new ProblemDetails
            {
                Title = "Error rejecting quote",
                Detail = string.Join(", ", result.Reasons.Select(r => r.Message)),
                Status = (int)result.StatusCode
            });
        }

        return Ok(result.Data);
    }
}

/// <summary>
/// Request model for rejecting a quote
/// </summary>
public record RejectQuoteRequest(string? Reason);

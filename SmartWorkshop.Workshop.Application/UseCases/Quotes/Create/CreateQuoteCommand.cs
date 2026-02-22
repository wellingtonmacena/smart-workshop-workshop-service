using MediatR;
using SmartWorkshop.Workshop.Domain.Common;
using SmartWorkshop.Workshop.Domain.Entities;

namespace SmartWorkshop.Workshop.Application.UseCases.Quotes.Create;

public record CreateQuoteCommand(
    Guid ServiceOrderId,
    string? Notes = null) : IRequest<Response<Quote>>;

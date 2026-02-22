using MediatR;
using SmartWorkshop.Workshop.Domain.Common;
using SmartWorkshop.Workshop.Domain.Entities;

namespace SmartWorkshop.Workshop.Application.UseCases.Quotes.AddService;

public record AddServiceToQuoteCommand(
    Guid QuoteId,
    Guid AvailableServiceId,
    decimal Price,
    string ServiceName) : IRequest<Response<Quote>>;

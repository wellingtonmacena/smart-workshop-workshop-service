using MediatR;
using SmartWorkshop.Workshop.Domain.Common;
using SmartWorkshop.Workshop.Domain.Entities;

namespace SmartWorkshop.Workshop.Application.UseCases.Quotes.AddSupply;

public record AddSupplyToQuoteCommand(
    Guid QuoteId,
    Guid SupplyId,
    decimal Price,
    int Quantity,
    string SupplyName) : IRequest<Response<Quote>>;

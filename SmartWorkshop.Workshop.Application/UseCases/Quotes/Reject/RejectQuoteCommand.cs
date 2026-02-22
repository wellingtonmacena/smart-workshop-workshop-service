using MediatR;
using SmartWorkshop.Workshop.Domain.Common;
using SmartWorkshop.Workshop.Domain.Entities;

namespace SmartWorkshop.Workshop.Application.UseCases.Quotes.Reject;

public record RejectQuoteCommand(Guid QuoteId, string? Reason = null) : IRequest<Response<Quote>>;

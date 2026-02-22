using MediatR;
using SmartWorkshop.Workshop.Domain.Common;
using SmartWorkshop.Workshop.Domain.Entities;

namespace SmartWorkshop.Workshop.Application.UseCases.Quotes.Approve;

public record ApproveQuoteCommand(Guid QuoteId) : IRequest<Response<Quote>>;

using MediatR;
using SmartWorkshop.Workshop.Domain.Common;
using SmartWorkshop.Workshop.Domain.Entities;

namespace SmartWorkshop.Workshop.Application.UseCases.Quotes.Get;

public record GetQuoteByIdQuery(Guid QuoteId) : IRequest<Response<Quote>>;

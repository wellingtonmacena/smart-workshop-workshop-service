using SmartWorkshop.Workshop.Domain.Entities;
using SmartWorkshop.Workshop.Domain.Common;
using MediatR;

namespace SmartWorkshop.Workshop.Application.UseCases.Quotes.GetAll;

public record GetAllQuotesQuery() : IRequest<Response<IEnumerable<Quote>>>;

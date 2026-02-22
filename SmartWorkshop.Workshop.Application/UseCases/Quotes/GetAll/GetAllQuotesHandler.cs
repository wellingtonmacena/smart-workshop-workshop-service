using MediatR;
using Microsoft.Extensions.Logging;
using SmartWorkshop.Workshop.Application.Adapters.Gateways.Repositories;
using SmartWorkshop.Workshop.Domain.Common;
using SmartWorkshop.Workshop.Domain.Entities;

namespace SmartWorkshop.Workshop.Application.UseCases.Quotes.GetAll;

public sealed class GetAllQuotesHandler(
    ILogger<GetAllQuotesHandler> logger,
    IQuoteRepository repository) : IRequestHandler<GetAllQuotesQuery, Response<IEnumerable<Quote>>>
{
    public async Task<Response<IEnumerable<Quote>>> Handle(GetAllQuotesQuery request, CancellationToken cancellationToken)
    {
        var entities = await repository.GetAllAsync(cancellationToken);

        logger.LogInformation("Retrieved {Count} quotes", entities.Count());

        return ResponseFactory.Ok(entities);
    }
}

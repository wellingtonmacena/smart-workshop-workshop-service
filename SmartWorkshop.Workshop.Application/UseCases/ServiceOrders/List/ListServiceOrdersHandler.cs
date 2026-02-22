using MediatR;
using Microsoft.Extensions.Logging;
using SmartWorkshop.Workshop.Application.Adapters.Gateways.Repositories;
using SmartWorkshop.Workshop.Domain.Common;
using SmartWorkshop.Workshop.Domain.Entities;

namespace SmartWorkshop.Workshop.Application.UseCases.ServiceOrders.List;

public sealed class ListServiceOrdersHandler(
    ILogger<ListServiceOrdersHandler> logger,
    IServiceOrderRepository repository) : IRequestHandler<ListServiceOrdersQuery, Response<IPaginate<ServiceOrder>>>
{
    public async Task<Response<IPaginate<ServiceOrder>>> Handle(ListServiceOrdersQuery request, CancellationToken cancellationToken)
    {
        var entities = await repository.GetAllPaginatedAsync(request.PageNumber, request.PageSize, cancellationToken);

        logger.LogInformation("Retrieved {Count} service orders", entities.TotalCount);

        return ResponseFactory.Ok<IPaginate<ServiceOrder>>(entities);
    }
}

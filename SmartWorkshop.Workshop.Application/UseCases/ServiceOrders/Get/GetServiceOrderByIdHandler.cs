using SmartWorkshop.Workshop.Application.Adapters.Gateways.Repositories;
using SmartWorkshop.Workshop.Domain.Entities;
using SmartWorkshop.Workshop.Domain.Common;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;

namespace SmartWorkshop.Workshop.Application.UseCases.ServiceOrders.Get;

public sealed class GetServiceOrderByIdHandler(
    ILogger<GetServiceOrderByIdHandler> logger,
    IServiceOrderRepository repository) : IRequestHandler<GetServiceOrderByIdQuery, Response<ServiceOrder>>
{
    public async Task<Response<ServiceOrder>> Handle(GetServiceOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdWithDetailsAsync(request.Id, cancellationToken);

        if (entity is null)
        {
            logger.LogWarning("ServiceOrder with Id {ServiceOrderId} not found", request.Id);
            return ResponseFactory.Fail<ServiceOrder>("ServiceOrder not found", HttpStatusCode.NotFound);
        }

        return ResponseFactory.Ok(entity);
    }
}

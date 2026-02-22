using AutoMapper;
using SmartWorkshop.Workshop.Application.Adapters.Gateways.Repositories;
using SmartWorkshop.Workshop.Domain.Entities;
using SmartWorkshop.Workshop.Domain.Common;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;

namespace SmartWorkshop.Workshop.Application.UseCases.ServiceOrders.Create;

public sealed class CreateServiceOrderHandler(
    ILogger<CreateServiceOrderHandler> logger,
    IMapper mapper,
    IServiceOrderRepository serviceOrderRepository,
    IPersonRepository personRepository,
    IAvailableServiceRepository availableServiceRepository,
    IVehicleRepository vehicleRepository) : IRequestHandler<CreateServiceOrderCommand, Response<ServiceOrder>>
{
    public async Task<Response<ServiceOrder>> Handle(CreateServiceOrderCommand request, CancellationToken cancellationToken)
    {
        var entity = mapper.Map<ServiceOrder>(request);

        if (!await personRepository.AnyAsync(x => x.Id == entity.ClientId, cancellationToken))
        {
            return ResponseFactory.Fail<ServiceOrder>("Person not found", HttpStatusCode.NotFound);
        }

        if (!await vehicleRepository.AnyAsync(x => x.Id == entity.VehicleId, cancellationToken))
        {
            return ResponseFactory.Fail<ServiceOrder>("Vehicle not found", HttpStatusCode.NotFound);
        }

        foreach (var serviceId in request.ServiceIds)
        {
            var availableService = await availableServiceRepository.GetByIdAsync(serviceId, cancellationToken);
            if (availableService is null)
            {
                return ResponseFactory.Fail<ServiceOrder>(
                    $"Service with Id {serviceId} not found",
                    HttpStatusCode.NotFound);
            }

            entity.AddService(serviceId);
        }

        var createdEntity = await serviceOrderRepository.AddAsync(entity, cancellationToken);

        logger.LogInformation(
            "ServiceOrder created. OrderId: {ServiceOrderId}, Status: {Status}, ClientId: {ClientId}, VehicleId: {VehicleId}, ServicesCount: {ServicesCount}",
            createdEntity.Id,
            createdEntity.Status,
            createdEntity.ClientId,
            createdEntity.VehicleId,
            createdEntity.AvailableServiceIds.Count
        );

        return ResponseFactory.Ok(createdEntity, HttpStatusCode.Created);
    }
}

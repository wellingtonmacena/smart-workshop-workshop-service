using MediatR;
using SmartWorkshop.Workshop.Domain.Common;
using SmartWorkshop.Workshop.Domain.Entities;

namespace SmartWorkshop.Workshop.Application.UseCases.ServiceOrders.Create;

public record CreateServiceOrderCommand(
    Guid ClientId,
    Guid VehicleId,
    IReadOnlyList<Guid> ServiceIds,
    string Title,
    string Description) : IRequest<Response<ServiceOrder>>;

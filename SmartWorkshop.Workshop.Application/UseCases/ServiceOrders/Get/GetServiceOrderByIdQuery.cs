using SmartWorkshop.Workshop.Domain.Entities;
using SmartWorkshop.Workshop.Domain.Common;
using MediatR;

namespace SmartWorkshop.Workshop.Application.UseCases.ServiceOrders.Get;

public record GetServiceOrderByIdQuery(Guid Id) : IRequest<Response<ServiceOrder>>;

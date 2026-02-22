using SmartWorkshop.Workshop.Domain.Entities;
using SmartWorkshop.Workshop.Domain.Common;
using MediatR;

namespace SmartWorkshop.Workshop.Application.UseCases.ServiceOrders.List;

public record ListServiceOrdersQuery(int PageNumber = 1, int PageSize = 15) : IRequest<Response<IPaginate<ServiceOrder>>>;

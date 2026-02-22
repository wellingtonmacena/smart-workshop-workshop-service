using SmartWorkshop.Workshop.Domain.Entities;

namespace SmartWorkshop.Workshop.Application.Adapters.Gateways.Repositories;

public interface IServiceOrderRepository : IRepository<ServiceOrder>
{
    Task<ServiceOrder?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);
}

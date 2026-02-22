using SmartWorkshop.Workshop.Application.Adapters.Gateways.Repositories;
using SmartWorkshop.Workshop.Infrastructure.Repositories;

namespace SmartWorkshop.Workshop.Api.Extensions;

public static class RepositoryExtensions
{
    public static IServiceCollection AddRepositoryExtensions(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IServiceOrderRepository, ServiceOrderRepository>();
        serviceCollection.AddScoped<IPersonRepository, PersonRepository>();
        serviceCollection.AddScoped<IVehicleRepository, VehicleRepository>();
        serviceCollection.AddScoped<IAvailableServiceRepository, AvailableServiceRepository>();
        serviceCollection.AddScoped<ISupplyRepository, SupplyRepository>();
        serviceCollection.AddScoped<IQuoteRepository, QuoteRepository>();
        serviceCollection.AddScoped<IWorkItemRepository, WorkItemRepository>();

        return serviceCollection;
    }
}

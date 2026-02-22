using SmartWorkshop.Workshop.Domain.Entities;
using SmartWorkshop.Workshop.Infrastructure.Data;
using SmartWorkshop.Workshop.Application.Adapters.Gateways.Repositories;
using Microsoft.EntityFrameworkCore;

namespace SmartWorkshop.Workshop.Infrastructure.Repositories;

public sealed class QuoteRepository(WorkshopDbContext dbContext) : Repository<Quote>(dbContext), IQuoteRepository
{
}

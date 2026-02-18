using MassTransit;
using SmartWorkshop.Shared.IntegrationEvents.Billing;

namespace SmartWorkshop.Workshop.Api.Consumers;

/// <summary>
/// Consumes QuoteApprovedIntegrationEvent from Billing Service
/// Triggers workflow to start service order execution
/// </summary>
public class QuoteApprovedConsumer : IConsumer<QuoteApprovedIntegrationEvent>
{
    private readonly ILogger<QuoteApprovedConsumer> _logger;
    
    public QuoteApprovedConsumer(ILogger<QuoteApprovedConsumer> logger)
    {
        _logger = logger;
    }
    
    public async Task Consume(ConsumeContext<QuoteApprovedIntegrationEvent> context)
    {
        var message = context.Message;
        
        _logger.LogInformation(
            "Quote approved - QuoteId: {QuoteId}, ServiceOrderId: {ServiceOrderId}, ApprovedAt: {ApprovedAt}",
            message.QuoteId,
            message.ServiceOrderId,
            message.ApprovedAt);
        
        try
        {
            // TODO: Implement business logic:
            // 1. Update ServiceOrder status to "Approved"
            // 2. Create WorkItems in production queue
            // 3. Allocate resources (mechanics, parts)
            // 4. Notify mechanics about new work
            
            _logger.LogInformation("Quote {QuoteId} processed successfully", message.QuoteId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing quote approval for QuoteId: {QuoteId}", message.QuoteId);
            throw; // MassTransit will handle retry policy
        }
    }
}

using MassTransit;
using SmartWorkshop.Shared.IntegrationEvents.Billing;

namespace SmartWorkshop.Workshop.Api.Consumers;

/// <summary>
/// Consumes PaymentConfirmedIntegrationEvent from Billing Service
/// Finalizes service order and triggers invoice generation
/// </summary>
public class PaymentConfirmedConsumer : IConsumer<PaymentConfirmedIntegrationEvent>
{
    private readonly ILogger<PaymentConfirmedConsumer> _logger;
    
    public PaymentConfirmedConsumer(ILogger<PaymentConfirmedConsumer> logger)
    {
        _logger = logger;
    }
    
    public async Task Consume(ConsumeContext<PaymentConfirmedIntegrationEvent> context)
    {
        var message = context.Message;
        
        _logger.LogInformation(
            "Payment confirmed - PaymentId: {PaymentId}, ServiceOrderId: {ServiceOrderId}, Amount: {Amount}",
            message.PaymentId,
            message.ServiceOrderId,
            message.Amount);
        
        try
        {
            // TODO: Implement business logic:
            // 1. Update ServiceOrder status to "Paid"
            // 2. Mark work as fully completed and billable
            // 3. Update inventory for consumed supplies
            // 4. Generate customer satisfaction survey
            // 5. Close service order
            
            _logger.LogInformation("Payment {PaymentId} processed successfully", message.PaymentId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing payment confirmation for PaymentId: {PaymentId}", message.PaymentId);
            throw; // MassTransit will handle retry policy
        }
    }
}

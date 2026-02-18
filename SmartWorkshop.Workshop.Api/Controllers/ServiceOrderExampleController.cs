using Microsoft.AspNetCore.Mvc;
using SmartWorkshop.Shared.EventBus;
using SmartWorkshop.Shared.IntegrationEvents.OS;
using SmartWorkshop.Shared.IntegrationEvents.Production;

namespace SmartWorkshop.Workshop.Api.Controllers;

/// <summary>
/// EXEMPLO: Controller demonstrando como publicar eventos com MassTransit
/// Este é um controller de exemplo para demonstração da arquitetura
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ServiceOrderExampleController : ControllerBase
{
    private readonly IEventBus _eventBus;
    private readonly ILogger<ServiceOrderExampleController> _logger;

    public ServiceOrderExampleController(
        IEventBus eventBus,
        ILogger<ServiceOrderExampleController> logger)
    {
        _eventBus = eventBus;
        _logger = logger;
    }

    /// <summary>
    /// Exemplo 1: Criar ordem de serviço e publicar evento
    /// POST /api/serviceorderexample
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateServiceOrder([FromBody] CreateServiceOrderRequest request)
    {
        try
        {
            _logger.LogInformation("Creating service order for customer {CustomerId}", request.CustomerPersonId);

            // TODO: Implementar lógica de negócio real
            // 1. Validar dados
            // 2. Criar entidade ServiceOrder
            // 3. Salvar no banco de dados
            // 4. Publicar evento de integração

            var serviceOrderId = Guid.NewGuid(); // Simulação

            // Publicar evento para Billing Service
            var integrationEvent = new ServiceOrderCreatedIntegrationEvent
            {
                ServiceOrderId = serviceOrderId,
                CustomerPersonId = request.CustomerPersonId,
                VehicleId = request.VehicleId,
                Description = request.Description
            };

            await _eventBus.PublishAsync(integrationEvent);

            _logger.LogInformation(
                "Service order {ServiceOrderId} created and event published",
                serviceOrderId);

            return CreatedAtAction(
                nameof(GetServiceOrder),
                new { id = serviceOrderId },
                new { id = serviceOrderId, status = "Created", message = "Event published to Billing Service" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating service order");
            return StatusCode(500, "Error creating service order");
        }
    }

    /// <summary>
    /// Exemplo 2: Completar trabalho e publicar evento
    /// POST /api/serviceorderexample/{id}/complete
    /// </summary>
    [HttpPost("{id}/complete")]
    public async Task<IActionResult> CompleteWork(Guid id, [FromBody] CompleteWorkRequest request)
    {
        try
        {
            _logger.LogInformation("Completing work for service order {ServiceOrderId}", id);

            // TODO: Implementar lógica de negócio real
            // 1. Buscar WorkItem do banco
            // 2. Validar se pode ser concluído
            // 3. Atualizar status para "Completed"
            // 4. Calcular duração total
            // 5. Publicar evento de integração

            var workItemId = Guid.NewGuid(); // Simulação

            // Publicar evento para Billing Service
            var integrationEvent = new WorkCompletedIntegrationEvent
            {
                WorkItemId = workItemId,
                ServiceOrderId = id,
                CompletedAt = DateTime.UtcNow,
                TotalDurationMinutes = request.DurationMinutes
            };

            await _eventBus.PublishAsync(integrationEvent);

            _logger.LogInformation(
                "Work completed for service order {ServiceOrderId}, event published",
                id);

            return Ok(new
            {
                serviceOrderId = id,
                workItemId = workItemId,
                status = "Completed",
                message = "Event published to Billing Service for invoice generation"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error completing work for service order {ServiceOrderId}", id);
            return StatusCode(500, "Error completing work");
        }
    }

    /// <summary>
    /// Exemplo 3: Obter ordem de serviço (stub)
    /// GET /api/serviceorderexample/{id}
    /// </summary>
    [HttpGet("{id}")]
    public IActionResult GetServiceOrder(Guid id)
    {
        // TODO: Implementar consulta real ao banco
        return Ok(new
        {
            id = id,
            status = "InProgress",
            message = "This is a stub endpoint for demonstration"
        });
    }

    /// <summary>
    /// Health check específico do controller
    /// GET /api/serviceorderexample/health
    /// </summary>
    [HttpGet("health")]
    public IActionResult Health()
    {
        return Ok(new
        {
            service = "Workshop Service",
            controller = "ServiceOrderExample",
            status = "Healthy",
            timestamp = DateTime.UtcNow,
            message = "MassTransit integration is working"
        });
    }
}

// ============================================================================
// DTOs
// ============================================================================

public record CreateServiceOrderRequest
{
    public Guid CustomerPersonId { get; init; }
    public Guid VehicleId { get; init; }
    public string Description { get; init; } = string.Empty;
}

public record CompleteWorkRequest
{
    public int DurationMinutes { get; init; }
}

using SmartWorkshop.Workshop.Application.Adapters.Gateways.Repositories;
using SmartWorkshop.Workshop.Domain.Entities;
using SmartWorkshop.Workshop.Domain.Common;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;
using SmartWorkshop.Shared.EventBus;
using SmartWorkshop.Shared.IntegrationEvents.Billing;

namespace SmartWorkshop.Workshop.Application.UseCases.Quotes.Approve;

public sealed class ApproveQuoteHandler(
    ILogger<ApproveQuoteHandler> logger,
    IQuoteRepository quoteRepository,
    IEventBus eventBus) : IRequestHandler<ApproveQuoteCommand, Response<Quote>>
{
    public async Task<Response<Quote>> Handle(ApproveQuoteCommand request, CancellationToken cancellationToken)
    {
        // Buscar o Quote
        var quote = await quoteRepository.GetByIdAsync(request.QuoteId, cancellationToken);
        if (quote is null)
        {
            return ResponseFactory.Fail<Quote>("Quote not found", HttpStatusCode.NotFound);
        }

        // Verificar se o quote está vazio
        if (quote.IsEmpty())
        {
            return ResponseFactory.Fail<Quote>(
                "Cannot approve an empty quote. Add at least one service or supply.",
                HttpStatusCode.BadRequest);
        }

        try
        {
            // Aprovar o quote
            quote.Approve();

            var updatedQuote = await quoteRepository.UpdateAsync(quote, cancellationToken);

            logger.LogInformation(
                "Quote approved. QuoteId: {QuoteId}, ServiceOrderId: {ServiceOrderId}, Total: {Total}",
                updatedQuote.Id,
                updatedQuote.ServiceOrderId,
                updatedQuote.Total
            );

            // Publicar evento de integração para notificar outros microserviços
            var integrationEvent = new QuoteApprovedIntegrationEvent
            {
                QuoteId = updatedQuote.Id,
                ServiceOrderId = updatedQuote.ServiceOrderId,
                ApprovedAt = DateTime.UtcNow
            };

            await eventBus.PublishAsync(integrationEvent, cancellationToken);

            logger.LogInformation(
                "QuoteApprovedIntegrationEvent published. QuoteId: {QuoteId}",
                updatedQuote.Id
            );

            return ResponseFactory.Ok(updatedQuote);
        }
        catch (DomainException ex)
        {
            return ResponseFactory.Fail<Quote>(ex.Message, HttpStatusCode.BadRequest);
        }
    }
}

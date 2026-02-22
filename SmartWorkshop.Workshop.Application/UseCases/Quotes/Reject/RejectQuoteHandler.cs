using SmartWorkshop.Workshop.Application.Adapters.Gateways.Repositories;
using SmartWorkshop.Workshop.Domain.Entities;
using SmartWorkshop.Workshop.Domain.Common;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;
using SmartWorkshop.Shared.EventBus;
using SmartWorkshop.Shared.IntegrationEvents.Billing;

namespace SmartWorkshop.Workshop.Application.UseCases.Quotes.Reject;

public sealed class RejectQuoteHandler(
    ILogger<RejectQuoteHandler> logger,
    IQuoteRepository quoteRepository,
    IEventBus eventBus) : IRequestHandler<RejectQuoteCommand, Response<Quote>>
{
    public async Task<Response<Quote>> Handle(RejectQuoteCommand request, CancellationToken cancellationToken)
    {
        // Buscar o Quote
        var quote = await quoteRepository.GetByIdAsync(request.QuoteId, cancellationToken);
        if (quote is null)
        {
            return ResponseFactory.Fail<Quote>("Quote not found", HttpStatusCode.NotFound);
        }

        try
        {
            // Rejeitar o quote
            quote.Reject();

            // Adicionar motivo da rejeição às notas se fornecido
            if (!string.IsNullOrEmpty(request.Reason))
            {
                var notes = string.IsNullOrEmpty(quote.Notes)
                    ? $"Rejected: {request.Reason}"
                    : $"{quote.Notes}\nRejected: {request.Reason}";
                quote.SetNotes(notes);
            }

            var updatedQuote = await quoteRepository.UpdateAsync(quote, cancellationToken);

            logger.LogInformation(
                "Quote rejected. QuoteId: {QuoteId}, ServiceOrderId: {ServiceOrderId}, Reason: {Reason}",
                updatedQuote.Id,
                updatedQuote.ServiceOrderId,
                request.Reason ?? "Not specified"
            );

            // Publicar evento de integração para notificar outros microserviços
            var integrationEvent = new QuoteRejectedIntegrationEvent
            {
                QuoteId = updatedQuote.Id,
                ServiceOrderId = updatedQuote.ServiceOrderId,
                RejectionReason = request.Reason ?? "Not specified"
            };

            await eventBus.PublishAsync(integrationEvent, cancellationToken);

            logger.LogInformation(
                "QuoteRejectedIntegrationEvent published. QuoteId: {QuoteId}",
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

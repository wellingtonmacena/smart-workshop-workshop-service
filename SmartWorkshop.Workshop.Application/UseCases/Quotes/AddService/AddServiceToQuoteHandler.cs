using SmartWorkshop.Workshop.Application.Adapters.Gateways.Repositories;
using SmartWorkshop.Workshop.Domain.Entities;
using SmartWorkshop.Workshop.Domain.Common;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;

namespace SmartWorkshop.Workshop.Application.UseCases.Quotes.AddService;

public sealed class AddServiceToQuoteHandler(
    ILogger<AddServiceToQuoteHandler> logger,
    IQuoteRepository quoteRepository,
    IAvailableServiceRepository availableServiceRepository) : IRequestHandler<AddServiceToQuoteCommand, Response<Quote>>
{
    public async Task<Response<Quote>> Handle(AddServiceToQuoteCommand request, CancellationToken cancellationToken)
    {
        // Verificar se o Quote existe
        var quote = await quoteRepository.GetByIdAsync(request.QuoteId, cancellationToken);
        if (quote is null)
        {
            return ResponseFactory.Fail<Quote>("Quote not found", HttpStatusCode.NotFound);
        }

        // Verificar se o AvailableService existe
        var availableService = await availableServiceRepository.GetByIdAsync(request.AvailableServiceId, cancellationToken);
        if (availableService is null)
        {
            return ResponseFactory.Fail<Quote>("Available Service not found", HttpStatusCode.NotFound);
        }

        // Verificar se o quote já foi aprovado ou rejeitado
        if (quote.Status == Domain.ValueObjects.QuoteStatus.Approved)
        {
            return ResponseFactory.Fail<Quote>("Cannot add service to an approved quote", HttpStatusCode.BadRequest);
        }

        if (quote.Status == Domain.ValueObjects.QuoteStatus.Rejected)
        {
            return ResponseFactory.Fail<Quote>("Cannot add service to a rejected quote", HttpStatusCode.BadRequest);
        }

        // Adicionar serviço ao quote
        quote.AddService(request.AvailableServiceId, request.Price, request.ServiceName);

        var updatedQuote = await quoteRepository.UpdateAsync(quote, cancellationToken);

        logger.LogInformation(
            "Service added to Quote. QuoteId: {QuoteId}, ServiceId: {ServiceId}, Price: {Price}, NewTotal: {Total}",
            updatedQuote.Id,
            request.AvailableServiceId,
            request.Price,
            updatedQuote.Total
        );

        return ResponseFactory.Ok(updatedQuote);
    }
}

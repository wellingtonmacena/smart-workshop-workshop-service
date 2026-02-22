using SmartWorkshop.Workshop.Application.Adapters.Gateways.Repositories;
using SmartWorkshop.Workshop.Domain.Entities;
using SmartWorkshop.Workshop.Domain.Common;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;

namespace SmartWorkshop.Workshop.Application.UseCases.Quotes.AddSupply;

public sealed class AddSupplyToQuoteHandler(
    ILogger<AddSupplyToQuoteHandler> logger,
    IQuoteRepository quoteRepository,
    ISupplyRepository supplyRepository) : IRequestHandler<AddSupplyToQuoteCommand, Response<Quote>>
{
    public async Task<Response<Quote>> Handle(AddSupplyToQuoteCommand request, CancellationToken cancellationToken)
    {
        // Verificar se o Quote existe
        var quote = await quoteRepository.GetByIdAsync(request.QuoteId, cancellationToken);
        if (quote is null)
        {
            return ResponseFactory.Fail<Quote>("Quote not found", HttpStatusCode.NotFound);
        }

        // Verificar se o Supply existe
        var supply = await supplyRepository.GetByIdAsync(request.SupplyId, cancellationToken);
        if (supply is null)
        {
            return ResponseFactory.Fail<Quote>("Supply not found", HttpStatusCode.NotFound);
        }

        // Verificar se há estoque suficiente
        if (!supply.HasSufficientStock(request.Quantity))
        {
            return ResponseFactory.Fail<Quote>(
                $"Insufficient stock. Available: {supply.Quantity}, Requested: {request.Quantity}",
                HttpStatusCode.BadRequest);
        }

        // Verificar se o quote já foi aprovado ou rejeitado
        if (quote.Status == Domain.ValueObjects.QuoteStatus.Approved)
        {
            return ResponseFactory.Fail<Quote>("Cannot add supply to an approved quote", HttpStatusCode.BadRequest);
        }

        if (quote.Status == Domain.ValueObjects.QuoteStatus.Rejected)
        {
            return ResponseFactory.Fail<Quote>("Cannot add supply to a rejected quote", HttpStatusCode.BadRequest);
        }

        // Adicionar supply ao quote
        quote.AddSupply(request.SupplyId, request.Price, request.Quantity, request.SupplyName);

        var updatedQuote = await quoteRepository.UpdateAsync(quote, cancellationToken);

        logger.LogInformation(
            "Supply added to Quote. QuoteId: {QuoteId}, SupplyId: {SupplyId}, Quantity: {Quantity}, Price: {Price}, NewTotal: {Total}",
            updatedQuote.Id,
            request.SupplyId,
            request.Quantity,
            request.Price,
            updatedQuote.Total
        );

        return ResponseFactory.Ok(updatedQuote);
    }
}

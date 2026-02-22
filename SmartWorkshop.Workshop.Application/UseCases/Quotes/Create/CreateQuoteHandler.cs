using AutoMapper;
using SmartWorkshop.Workshop.Application.Adapters.Gateways.Repositories;
using SmartWorkshop.Workshop.Domain.Entities;
using SmartWorkshop.Workshop.Domain.Common;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;

namespace SmartWorkshop.Workshop.Application.UseCases.Quotes.Create;

public sealed class CreateQuoteHandler(
    ILogger<CreateQuoteHandler> logger,
    IQuoteRepository quoteRepository,
    IServiceOrderRepository serviceOrderRepository) : IRequestHandler<CreateQuoteCommand, Response<Quote>>
{
    public async Task<Response<Quote>> Handle(CreateQuoteCommand request, CancellationToken cancellationToken)
    {
        // Verificar se a ServiceOrder existe
        var serviceOrder = await serviceOrderRepository.GetByIdAsync(request.ServiceOrderId, cancellationToken);
        if (serviceOrder is null)
        {
            return ResponseFactory.Fail<Quote>("Service Order not found", HttpStatusCode.NotFound);
        }

        // Criar o Quote
        var quote = new Quote(request.ServiceOrderId);
        
        if (!string.IsNullOrEmpty(request.Notes))
        {
            quote.SetNotes(request.Notes);
        }

        var createdQuote = await quoteRepository.AddAsync(quote, cancellationToken);

        logger.LogInformation(
            "Quote created. QuoteId: {QuoteId}, ServiceOrderId: {ServiceOrderId}, Status: {Status}",
            createdQuote.Id,
            createdQuote.ServiceOrderId,
            createdQuote.Status
        );

        return ResponseFactory.Ok(createdQuote, HttpStatusCode.Created);
    }
}

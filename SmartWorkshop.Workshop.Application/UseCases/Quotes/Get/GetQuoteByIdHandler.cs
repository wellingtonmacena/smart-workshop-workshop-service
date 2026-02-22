using SmartWorkshop.Workshop.Application.Adapters.Gateways.Repositories;
using SmartWorkshop.Workshop.Domain.Entities;
using SmartWorkshop.Workshop.Domain.Common;
using MediatR;
using System.Net;

namespace SmartWorkshop.Workshop.Application.UseCases.Quotes.Get;

public sealed class GetQuoteByIdHandler(
    IQuoteRepository quoteRepository) : IRequestHandler<GetQuoteByIdQuery, Response<Quote>>
{
    public async Task<Response<Quote>> Handle(GetQuoteByIdQuery request, CancellationToken cancellationToken)
    {
        var quote = await quoteRepository.GetByIdAsync(request.QuoteId, cancellationToken);

        if (quote is null)
        {
            return ResponseFactory.Fail<Quote>("Quote not found", HttpStatusCode.NotFound);
        }

        return ResponseFactory.Ok(quote);
    }
}

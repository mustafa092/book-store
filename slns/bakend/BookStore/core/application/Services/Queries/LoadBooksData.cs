using application.DTOs.infrastructure;
using application.infrastructure.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace application.Services.Queries;

public class LoadBooksQuery : IRequest<IEnumerable<LoadingBooksResponse>>
{
    public string? SearchKey { get; set; }
    public int PageNumber { get; set; }
    // set the maximum number of items per page to 30
    public int PageSize { get; set; } = 20;
}

public class LoadBooksQueryHandler : IRequestHandler<LoadBooksQuery, IEnumerable<LoadingBooksResponse>>
{
    private readonly IBookRepository _bookRepository;
    private readonly ILogger<LoadBooksQueryHandler> _logger;

    public LoadBooksQueryHandler(IBookRepository bookRepository , ILogger<LoadBooksQueryHandler> logger)
    {
        _bookRepository = bookRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<LoadingBooksResponse>> Handle(LoadBooksQuery request,
        CancellationToken cancellationToken)
    {
        // loggin the request 
        _logger.LogInformation("Loading books with request {@request}", request);
        
        // adding the libarary for the validation name it : FluentValidation
        // todo:// in the validation we should check if the page number is less than 1 then we should return the first page

        // todo :// and also page size should be greater than 0 and less than 30


        var loadingBooksRequest = new LoadingBooksRequest
        {
            SearchKey = request.SearchKey,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };

        return await _bookRepository.SearchAsync(loadingBooksRequest);
    }
}
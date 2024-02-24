using System.Runtime.CompilerServices;
using application.DTOs.infrastructure;
using application.infrastructure.Repositories;
using cachingManager.services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace persistence.Data.Repositories;

public class BookRepository : IBookRepository
{
    private const string BooksCacheKey = nameof(BookRepository);
    private readonly ICacheService _cacheService;
    private readonly ILogger<BookRepository> _logger;
    private readonly ElmDbContext _elmDbContext;

    public BookRepository(ElmDbContext elmDbContext, ICacheService cacheService , ILogger<BookRepository> logger)
    {
        _elmDbContext = elmDbContext;
        _cacheService = cacheService;
        _logger = logger;
    }


    public async Task<List<LoadingBooksResponse>> LoadingBooksAsyncV2(LoadingBooksRequest request)
    {
        // Define the raw SQL query
        var rawSqlQuery = @"
        SELECT *
        FROM Book
        WHERE JSON_VALUE(BookInfo, '$.BookTitle') LIKE '%' + {0} + '%'
           OR JSON_VALUE(BookInfo, '$.BookDescription') LIKE '%' + {0} + '%'
           OR JSON_VALUE(BookInfo, '$.Author') LIKE '%' + {0} + '%'";

        // Execute the raw SQL query
        var books = await _elmDbContext.Books
            .FromSqlInterpolated(FormattableStringFactory.Create(rawSqlQuery, request.SearchKey))
            .OrderBy(b => b.BookId)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(b => new LoadingBooksResponse
            {
                BookId = b.BookId,
                BookTitle = b.BookInfo.BookTitle,
                BookDescription = b.BookInfo.BookDescription,
                Author = b.BookInfo.Author,
                PublishDate = b.BookInfo.PublishDate,
                CoverBase64 = b.BookInfo.CoverBase64
            }).ToListAsync();

        return books;
    }


    public async Task<List<LoadingBooksResponse>> SearchAsync(LoadingBooksRequest request)
    {
        // Construct a unique cache key based on the request parameters
        var cacheKey = $"{BooksCacheKey}_Page{request.PageNumber}_Size{request.PageSize}";
        if (!_cacheService.TryGetValue(cacheKey, out List<LoadingBooksResponse> cachedBooks))
        {
            _logger.LogInformation("Cache miss for key {cacheKey} and retrieve from the database ", cacheKey);
            // configure the cache options
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(60))
                .SetAbsoluteExpiration(TimeSpan.FromHours(24))
                .SetPriority(CacheItemPriority.High);
            // Cache miss; fetch data from database
            cachedBooks = await RetrieveRecordFromDataStore(request);
            // Cache the result
            _cacheService.Set(cacheKey, cachedBooks, cacheEntryOptions);
        }
        else
        {
            _logger.LogInformation("Cache hit for key {cacheKey} and retrieve from the cache ", cacheKey);
        }

        return cachedBooks;
    }

    private async Task<List<LoadingBooksResponse>> RetrieveRecordFromDataStore(LoadingBooksRequest request)
    {
        IQueryable<Book> query = _elmDbContext.Books;

        // Apply search filters only if SearchKey is not empty
        if (!string.IsNullOrWhiteSpace(request.SearchKey))
        {
            var rawSqlQuery = @"
        SELECT *
        FROM Book
        WHERE JSON_VALUE(BookInfo, '$.BookTitle') LIKE '%' + {0} + '%'
           OR JSON_VALUE(BookInfo, '$.BookDescription') LIKE '%' + {0} + '%'
           OR JSON_VALUE(BookInfo, '$.Author') LIKE '%' + {0} + '%'";

            query = _elmDbContext.Books.FromSqlInterpolated(
                FormattableStringFactory.Create(rawSqlQuery, request.SearchKey));
        }

        var books = await query.OrderBy(b => b.BookId) // Adjust the ordering as per your requirement
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(b => new LoadingBooksResponse
            {
                BookId = b.BookId,
                BookTitle = b.BookInfo.BookTitle,
                BookDescription = b.BookInfo.BookDescription,
                Author = b.BookInfo.Author,
                PublishDate = b.BookInfo.PublishDate,
                CoverBase64 = b.BookInfo.CoverBase64
            }).ToListAsync();
        return books;
    }
}
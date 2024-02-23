using System.Runtime.CompilerServices;
using application.DTOs.infrastructure;
using application.infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace persistence.Data.Repositories;

public class BookRepository : IBookRepository
{
    private readonly ElmDbContext _elmDbContext;

    public BookRepository(ElmDbContext elmDbContext)
    {
        _elmDbContext = elmDbContext;
    }

    public async Task<List<LoadingBooksResponse>> LoadingBooksAsync(LoadingBooksRequest request)
    {
        var books = await _elmDbContext.Books
            .Where(b => b.BookInfo.BookTitle.Contains(request.SearchKey) ||
                        b.BookInfo.BookDescription.Contains(request.SearchKey) ||
                        b.BookInfo.Author.Contains(request.SearchKey)
            )
            .OrderBy(b => b.BookInfo.BookTitle)
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
}
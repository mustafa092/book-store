using application.DTOs.infrastructure;

namespace application.infrastructure.Repositories;

public interface IBookRepository
{
    Task<List<LoadingBooksResponse>> LoadingBooksAsync(LoadingBooksRequest request);
    Task<List<LoadingBooksResponse>> LoadingBooksAsyncV2(LoadingBooksRequest request);
}
namespace application.DTOs.infrastructure;

public class LoadingBooksRequest
{
    private const int MaxPageSize = 30;

    private int _pageSize = 10; // Default page size, you can adjust this

    // dto for loading books request with pagination
    public string? SearchKey { get; set; }

    public int PageNumber { get; set; } = 1;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }
}

public class LoadingBooksResponse
{
    public long BookId { get; set; }
    public string BookTitle { get; set; } = null!;
    public string BookDescription { get; set; } = null!;
    public string Author { get; set; } = null!;
    public DateTime PublishDate { get; set; }
    public string CoverBase64 { get; set; }
}
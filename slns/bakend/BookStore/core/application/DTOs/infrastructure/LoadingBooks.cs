namespace application.DTOs.infrastructure;

public class LoadingBooksRequest
{
    // dto for loading books request with pagination
    public string SearchKey { get; set; }

    public int PageNumber { get; set; }
    public int PageSize { get; set; } = 20;
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
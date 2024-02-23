namespace persistence;

public class Book
{
    public long BookId { get; set; }
    public BookInfo BookInfo { get; set; } = null!;
    public DateTime LastModified { get; set; }
}
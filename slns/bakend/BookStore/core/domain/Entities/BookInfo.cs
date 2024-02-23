namespace persistence;

public class BookInfo
{
    public string BookTitle { get; set; } = null!;
    public string BookDescription { get; set; } = null!;
    public string Author { get; set; } = null!;
    public DateTime PublishDate { get; set; }
    public string CoverBase64 { get; set; }
}
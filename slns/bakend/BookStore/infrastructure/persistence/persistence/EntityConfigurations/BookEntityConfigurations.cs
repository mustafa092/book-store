using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using persistence.CustomValueConvertors;

namespace persistence.EntityConfigurations;

public class BookEntityConfigurations : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("Book");
        var bookInfoConverter = new JsonValueConverter<BookInfo>();
        builder.Property(m => m.BookInfo).HasConversion(bookInfoConverter);

        builder.HasIndex(e => e.LastModified, "IX_Book_LastModified");
    }
}
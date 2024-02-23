using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace persistence.EntityConfigurations
{
    public class BookEntityConfigurations : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            
            builder.ToTable("Book");

            builder.HasIndex(e => e.LastModified, "IX_Book_LastModified");
        }
    }
}

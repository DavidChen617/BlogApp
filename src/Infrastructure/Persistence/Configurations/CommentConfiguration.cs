using Domain.Comments;
using Domain.Posts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public sealed class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable("comments");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasConversion(id => id.Value, v => CommentId.From(v));

        builder.Property(c => c.PostId)
            .HasConversion(id => id.Value, v => PostId.From(v))
            .IsRequired();

        builder.Property(c => c.AuthorId)
            .IsRequired();

        builder.Property(c => c.Body)
            .HasMaxLength(2000)
            .IsRequired();

        builder.Property(c => c.CreatedAtUtc)
            .IsRequired();

        builder.Ignore(c => c.DomainEvents);
    }
}

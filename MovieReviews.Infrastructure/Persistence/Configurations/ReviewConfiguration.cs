using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieReviews.Domain.Entities;

namespace MovieReviews.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework Core configuration for the Review entity.
/// </summary>
public sealed class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
	/// <summary>
	/// Configures the Review entity mapping to the database.
	/// </summary>
	/// <param name="builder">The entity type builder for Review.</param>
	public void Configure(EntityTypeBuilder<Review> builder)
	{
		builder.ToTable("Reviews");

		builder.HasKey(r => r.Id);

		builder.Property(r => r.ReviewerName)
			.IsRequired()
			.HasMaxLength(100);

		builder.Property(r => r.Rating)
			.IsRequired();

		builder.Property(r => r.Comment)
			.HasMaxLength(1000);
	}
}
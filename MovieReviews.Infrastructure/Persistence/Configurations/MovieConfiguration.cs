using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieReviews.Domain.Entities;

namespace MovieReviews.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework Core configuration for the Movie entity.
/// </summary>
public sealed class MovieConfiguration : IEntityTypeConfiguration<Movie>
{
	/// <summary>
	/// Configures the Movie entity mapping to the database.
	/// </summary>
	/// <param name="builder">The entity type builder for Movie.</param>
	public void Configure(EntityTypeBuilder<Movie> builder)
	{
		builder.ToTable("Movies");

		builder.HasKey(m => m.Id);

		builder.Property(m => m.Title)
			.IsRequired()
			.HasMaxLength(200);

		builder.Property(m => m.Director)
			.IsRequired()
			.HasMaxLength(100);

		builder.Property(m => m.Genre)
			.IsRequired()
			.HasMaxLength(50);

		builder.Property(m => m.ReleaseYear)
			.IsRequired();

		builder.HasMany(m => m.Reviews)
			.WithOne(r => r.Movie)
			.HasForeignKey(r => r.MovieId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}
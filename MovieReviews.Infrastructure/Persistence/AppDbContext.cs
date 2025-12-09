using Microsoft.EntityFrameworkCore;
using MovieReviews.Domain.Entities;

namespace MovieReviews.Infrastructure.Persistence;

/// <summary>
/// Represents the Entity Framework Core database context for the Movie Reviews application.
/// Manages database connections and entity mappings.
/// </summary>
/// <param name="options">Configuration options for the database context.</param>
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
	/// <summary>
	/// Gets or sets the DbSet for movie entities.
	/// </summary>
	public DbSet<Movie> Movies { get; set; } = null!;

	/// <summary>
	/// Gets or sets the DbSet for review entities.
	/// </summary>
	public DbSet<Review> Reviews { get; set; } = null!;

	/// <summary>
	/// Configures the entity models and relationships using Fluent API.
	/// </summary>
	/// <param name="modelBuilder">The builder used to construct the model for this context.</param>
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
	}
}

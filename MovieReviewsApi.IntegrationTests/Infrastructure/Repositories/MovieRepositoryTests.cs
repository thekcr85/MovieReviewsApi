using Microsoft.EntityFrameworkCore;
using MovieReviews.Domain.Entities;
using MovieReviews.Infrastructure.Persistence;
using MovieReviews.Infrastructure.Repositories;
using System.Collections.Generic;
using Xunit;

namespace MovieReviewsApi.IntegrationTests.Infrastructure.Repositories
{
	public class MovieRepositoryTests
	{
		private static AppDbContext CreateInMemoryContext()
		{
			var options = new DbContextOptionsBuilder<AppDbContext>()
				.UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
				.Options;

			return new AppDbContext(options);
		}

		[Fact]
		public async Task GetAllAsync_ReturnsAllMovies()
		{
			// Arrange
			await using var context = CreateInMemoryContext();
			context.Movies.AddRange(
				new Movie { Id = 1, Title = "A", Director = "D1", ReleaseYear = 2000, Genre = "Drama" },
				new Movie { Id = 2, Title = "B", Director = "D2", ReleaseYear = 2001, Genre = "Comedy" }
			);
			await context.SaveChangesAsync();

			var repository = new MovieRepository(context);

			// Act
			var result = await repository.GetAllAsync();

			// Assert
			Assert.NotNull(result); // Check that result is not null
			Assert.Equal(2, result.Count); // Check that two movies are returned
			Assert.Contains(result, m => m.Title == "A"); // Check that movie 'A' is in the result
			Assert.Contains(result, m => m.Title == "B"); // Check that movie 'B' is in the result
		}
	}
}

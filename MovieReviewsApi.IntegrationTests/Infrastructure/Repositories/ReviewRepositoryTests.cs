using Microsoft.EntityFrameworkCore;
using MovieReviews.Domain.Entities;
using MovieReviews.Infrastructure.Persistence;
using MovieReviews.Infrastructure.Repositories;

namespace MovieReviewsApi.IntegrationTests.Infrastructure.Repositories
{
	public class ReviewRepositoryTests
	{
		private static AppDbContext CreateInMemoryContext()
		{
			var options = new DbContextOptionsBuilder<AppDbContext>()
				.UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
				.Options;

			return new AppDbContext(options);
		}

		[Fact]
		public async Task GetAllAsync_ReturnsAllReviews()
		{
			await using var context = CreateInMemoryContext();
			var movie = new Movie { Id = 1, Title = "M1", Director = "D", ReleaseYear = 2000, Genre = "Drama" };
			context.Movies.Add(movie);
			context.Reviews.AddRange(
				new Review { Id = 10, MovieId = 1, ReviewerName = "Alice", Rating = 8, Comment = "Good" },
				new Review { Id = 11, MovieId = 1, ReviewerName = "Bob", Rating = 7, Comment = "Okay" }
			);
			await context.SaveChangesAsync();

			var repository = new ReviewRepository(context);
			var result = await repository.GetAllAsync();

			Assert.NotNull(result);
			Assert.Equal(2, result.Count);
			Assert.Contains(result, r => r.ReviewerName == "Alice");
			Assert.Contains(result, r => r.ReviewerName == "Bob");
		}
	}
}

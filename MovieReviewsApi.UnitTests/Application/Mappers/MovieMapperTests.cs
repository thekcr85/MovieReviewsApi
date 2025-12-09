using MovieReviews.Application.Mappers;
using MovieReviews.Domain.Entities;

namespace MovieReviewsApi.UnitTests.Application.Mappers
{
	public class MovieMapperTests
	{
		[Fact]
		public void ToDto_CalculatesAverageRating()
		{
			var movie = new Movie { Id = 1, Title = "T", Director = "D", ReleaseYear = 2000, Genre = "G", Reviews = new List<Review> { new Review { Rating = 8 }, new Review { Rating = 6 } } };
			var dto = movie.ToDto();
			Assert.Equal(7, dto.AverageRating);  // (8 + 6) / 2 = 7
		}

		[Fact]
		public void ToWithReviewsDto_IncludesReviews()
		{
			var movie = new Movie { Id = 2, Title = "T2", Director = "D2", ReleaseYear = 2001, Genre = "G2", Reviews = new List<Review> { new Review { Id = 1, MovieId = 2, ReviewerName = "A", Rating = 5, Comment = "ok" } } };
			var dto = movie.ToWithReviewsDto();
			Assert.Single(dto.Reviews);
			Assert.Equal("A", dto.Reviews.First().ReviewerName);
		}
	}
}

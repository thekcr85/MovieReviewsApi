using MovieReviews.Application.Mappers;
using MovieReviews.Domain.Entities;
using Xunit;

namespace MovieReviewsApi.UnitTests.Application.Mappers
{
	public class ReviewMapperTests
	{
		[Fact]
		public void ToDto_MapsFields()
		{
			var review = new Review { Id = 1, MovieId = 2, ReviewerName = "X", Rating = 9, Comment = "Great" };
			var dto = review.ToDto();
			Assert.Equal(1, dto.Id);
			Assert.Equal(2, dto.MovieId);
			Assert.Equal("X", dto.ReviewerName);
			Assert.Equal(9, dto.Rating);
		}
	}
}

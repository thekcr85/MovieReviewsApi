using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieReviews.Application.Services;
using MovieReviews.Domain.Entities;
using MovieReviews.Domain.Interfaces;
using Xunit;

namespace MovieReviewsApi.UnitTests.Application.Services
{
	public class ReviewServiceTests
	{
		private class FakeReviewRepository : IReviewRepository
		{
			private readonly List<Review> reviews = new();

			public FakeReviewRepository(IEnumerable<Review> seed)
			{
				reviews.AddRange(seed);
			}

			public Task AddAsync(Review review, CancellationToken ct = default)
			{
				reviews.Add(review);
				return Task.CompletedTask;
			}

			public Task<int> CountByMovieIdAsync(int movieId, CancellationToken ct = default) => Task.FromResult(reviews.Count(r=>r.MovieId==movieId));
			public Task DeleteAsync(Review review, CancellationToken ct = default) => throw new NotImplementedException();
			public Task<IReadOnlyList<Review>> GetAllAsync(CancellationToken ct = default) => Task.FromResult((IReadOnlyList<Review>)reviews.ToArray());
			public Task<IReadOnlyList<Review>> GetByMovieIdAsync(int movieId, CancellationToken ct = default) => Task.FromResult((IReadOnlyList<Review>)reviews.Where(r=>r.MovieId==movieId).ToArray());
			public Task<IReadOnlyList<Review>> GetByRatingRangeAsync(int minRating, int maxRating, CancellationToken ct = default) => Task.FromResult((IReadOnlyList<Review>)reviews.Where(r=>r.Rating>=minRating && r.Rating<=maxRating).ToArray());
			public Task<IReadOnlyList<Review>> GetByReviewerNameAsync(string reviewerName, CancellationToken ct = default) => Task.FromResult((IReadOnlyList<Review>)reviews.Where(r=>r.ReviewerName==reviewerName).ToArray());
			public Task<Review?> GetByIdAsync(int id, CancellationToken ct = default) => Task.FromResult(reviews.FirstOrDefault(r=>r.Id==id));
			public Task<Review> UpdateAsync(Review review, CancellationToken ct = default)
			{
				var idx = reviews.FindIndex(r=>r.Id==review.Id);
				if(idx==-1) throw new InvalidOperationException();
				reviews[idx]=review;
				return Task.FromResult(review);
			}
		}

		[Fact]
		public async Task GetAverageRatingByMovieId_ReturnsZeroWhenNoReviews()
		{
			var repo = new FakeReviewRepository(new Review[0]);
			var service = new ReviewService(repo);
			var avg = await service.GetAverageRatingByMovieIdAsync(1);
			Assert.Equal(0.0, avg);
		}

		[Fact]
		public async Task GetByReviewerAsync_TrimsAndReturnsEmptyWhenBlank()
		{
			var repo = new FakeReviewRepository(new Review[0]);
			var service = new ReviewService(repo);
			var res = await service.GetByReviewerAsync("   ");
			Assert.Empty(res);
		}

		[Fact]
		public async Task GetByMovieIdAsync_ThrowsOnInvalidMovieId()
		{
			var repo = new FakeReviewRepository(new Review[0]);
			var service = new ReviewService(repo);
			await Assert.ThrowsAsync<ArgumentException>(async ()=> await service.GetByMovieIdAsync(0));
		}
	}
}

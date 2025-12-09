using MovieReviews.Application.DTOs.Movies;
using MovieReviews.Application.Services;
using MovieReviews.Domain.Entities;
using MovieReviews.Domain.Interfaces;

namespace MovieReviewsApi.UnitTests.Application.Services
{
	public class MovieServiceTests
	{
		private class FakeMovieRepository : IMovieRepository
		{
			private readonly List<Movie> movies = new();

			public FakeMovieRepository(IEnumerable<Movie> seed)
			{
				movies.AddRange(seed);
			}

			public Task AddAsync(Movie movie, CancellationToken ct = default)
			{
				movies.Add(movie);
				return Task.CompletedTask;
			}

			public Task<int> CountByGenreAsync(string genre, CancellationToken ct = default) => Task.FromResult(movies.Count(m => m.Genre == genre));

			public Task DeleteAsync(Movie movie, CancellationToken ct = default) => throw new NotImplementedException();
			public Task<IReadOnlyList<Movie>> GetAllAsync(CancellationToken ct = default) => Task.FromResult((IReadOnlyList<Movie>)movies.ToArray());
			public Task<IReadOnlyList<string>> GetAllGenresAsync(CancellationToken ct = default) => Task.FromResult((IReadOnlyList<string>)movies.Select(m=>m.Genre).Distinct().ToArray());
			public Task<IReadOnlyList<Movie>> GetByDirectorAsync(string director, CancellationToken ct = default) => Task.FromResult((IReadOnlyList<Movie>)movies.Where(m=>m.Director==director).ToArray());
			public Task<IReadOnlyList<Movie>> GetByGenreAsync(string genre, CancellationToken ct = default) => Task.FromResult((IReadOnlyList<Movie>)movies.Where(m=>m.Genre==genre).ToArray());
			public Task<IReadOnlyList<Movie>> GetByReleaseYearAsync(int year, CancellationToken ct = default) => Task.FromResult((IReadOnlyList<Movie>)movies.Where(m=>m.ReleaseYear==year).ToArray());
			public Task<Movie?> GetByIdAsync(int id, CancellationToken ct = default) => Task.FromResult(movies.FirstOrDefault(m=>m.Id==id));
			public Task<IReadOnlyList<Movie>> GetByYearRangeAsync(int startYear, int endYear, CancellationToken ct = default) => Task.FromResult((IReadOnlyList<Movie>)movies.Where(m=>m.ReleaseYear>=startYear && m.ReleaseYear<=endYear).ToArray());
			public Task<IReadOnlyList<Movie>> GetMoviesByMinRatingAsync(double minRating, CancellationToken ct = default) => Task.FromResult((IReadOnlyList<Movie>)movies.Where(m=>m.Reviews!=null && m.Reviews.Any() && m.Reviews.Average(r=>r.Rating)>=minRating).ToArray());
			public Task<IReadOnlyList<Movie>> GetMoviesWithReviewsAsync(CancellationToken ct = default) => Task.FromResult((IReadOnlyList<Movie>)movies.Where(m=>m.Reviews!=null && m.Reviews.Any()).ToArray());
			public Task<IReadOnlyList<Movie>> GetTopRatedAsync(int count, CancellationToken ct = default) => Task.FromResult((IReadOnlyList<Movie>)movies.Where(m=>m.Reviews!=null && m.Reviews.Any()).OrderByDescending(m=>m.Reviews.Average(r=>r.Rating)).Take(count).ToArray());
			public Task<IReadOnlyList<Movie>> SearchByTitleAsync(string searchTerm, CancellationToken ct = default) => Task.FromResult((IReadOnlyList<Movie>)movies.Where(m=>m.Title!=null && m.Title.Contains(searchTerm)).ToArray());
			public Task<Movie> UpdateAsync(Movie movie, CancellationToken ct = default)
			{
				var idx = movies.FindIndex(m=>m.Id==movie.Id);
				if (idx==-1) throw new InvalidOperationException();
				movies[idx]=movie;
				return Task.FromResult(movie);
			}
		}

		[Fact]
		public async Task GetAllAsync_MapsToMovieDto()
		{
			// Arrange
			var m = new Movie { Id = 1, Title = "T", Director = "D", ReleaseYear = 2000, Genre = "G", Reviews = new List<Review> { new Review { Rating = 8 }, new Review { Rating = 6 } } };
			var repo = new FakeMovieRepository(new[] { m });
			var service = new MovieService(repo);

			// Act
			var result = await service.GetAllAsync();

			// Assert
			Assert.Single(result);
			var dto = result[0];
			Assert.Equal(7, dto.AverageRating);
		}

		[Fact]
		public async Task CreateAsync_AddsMovieAndReturnsDto()
		{
			// Arrange
			var repo = new FakeMovieRepository(new Movie[0]);
			var service = new MovieService(repo);
			var create = new CreateMovieDto("New", "Dir", 2023, "Sci-Fi");

			// Act
			var dto = await service.CreateAsync(create);

			// Assert
			Assert.Equal("New", dto.Title);
			// repository should contain the new movie
			var all = await repo.GetAllAsync();
			Assert.Contains(all, m=>m.Title=="New");
		}

		[Fact]
		public async Task GetByDirectorAsync_TrimsAndReturnsEmptyForWhitespace()
		{
			// Arrange
			var repo = new FakeMovieRepository(new Movie[0]);
			var service = new MovieService(repo);
			// Act
			var result = await service.GetByDirectorAsync("   ");
			// Assert
			Assert.Empty(result);
		}
	}
}

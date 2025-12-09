using Microsoft.EntityFrameworkCore;
using MovieReviews.Domain.Interfaces;
using MovieReviews.Domain.Entities;
using MovieReviews.Infrastructure.Persistence;

namespace MovieReviews.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for movie data access operations.
/// Handles all persistence layer interactions for Movie entities.
/// </summary>
/// <param name="appDbContext">The Entity Framework Core database context instance.</param>
public class MovieRepository(AppDbContext appDbContext) : IMovieRepository
{
	/// <inheritdoc/>
	public async Task<IReadOnlyList<Movie>> GetAllAsync(CancellationToken ct = default)
	{
		return await appDbContext.Movies
			.Include(m => m.Reviews)
			.AsNoTracking()
			.ToArrayAsync(ct);
	}

	/// <inheritdoc/>
	public async Task<Movie?> GetByIdAsync(int id, CancellationToken ct = default)
	{
		return await appDbContext.Movies
			.Include(m => m.Reviews)
			.AsNoTracking()
			.FirstOrDefaultAsync(m => m.Id == id, ct);
	}

	/// <inheritdoc/>
	public async Task AddAsync(Movie movie, CancellationToken ct = default)
	{
		ArgumentNullException.ThrowIfNull(movie, nameof(movie));

		await appDbContext.Movies.AddAsync(movie, ct);
		await appDbContext.SaveChangesAsync(ct);
	}

	/// <inheritdoc/>
	public async Task<Movie> UpdateAsync(Movie movie, CancellationToken ct = default)
	{
		ArgumentNullException.ThrowIfNull(movie);

		var existingMovie = await appDbContext.Movies
			.FirstOrDefaultAsync(m => m.Id == movie.Id, ct);

		if (existingMovie is null)
		{
			throw new InvalidOperationException($"Movie with ID {movie.Id} not found.");
		}

		appDbContext.Entry(existingMovie).CurrentValues.SetValues(movie);
		await appDbContext.SaveChangesAsync(ct);

		return existingMovie; // ← zwraca śledzoną encję z aktualnymi wartościami
	}

	/// <inheritdoc/>
	public async Task DeleteAsync(Movie movie, CancellationToken ct = default)
	{
		ArgumentNullException.ThrowIfNull(movie, nameof(movie));

		var existingMovie = await appDbContext.Movies
			.FirstOrDefaultAsync(m => m.Id == movie.Id, ct);

		if (existingMovie is null)
		{
			throw new InvalidOperationException($"Movie with ID {movie.Id} not found.");
		}

		appDbContext.Movies.Remove(existingMovie);
		await appDbContext.SaveChangesAsync(ct);
	}



	/// <inheritdoc/>
	public async Task<IReadOnlyList<Movie>> GetByDirectorAsync(string director, CancellationToken ct = default)
	{
		if (string.IsNullOrWhiteSpace(director))
		{
			return Array.Empty<Movie>();
		}

		var normalized = director.Trim();

		return await appDbContext.Movies
			.Where(m => !string.IsNullOrEmpty(m.Director) && m.Director == normalized)
			.Include(m => m.Reviews)
			.AsNoTracking()
			.ToArrayAsync(ct);
	}

	/// <inheritdoc/>
	public async Task<IReadOnlyList<Movie>> GetByGenreAsync(string genre, CancellationToken ct = default)
	{ 
		if (string.IsNullOrWhiteSpace(genre))
		{
			return Array.Empty<Movie>();
		}

		return await appDbContext.Movies
			.Where(m => m.Genre == genre)
			.Include(m => m.Reviews)
			.AsNoTracking()
			.ToArrayAsync(ct);
	}

	/// <inheritdoc/>
	public async Task<IReadOnlyList<Movie>> GetByReleaseYearAsync(int year, CancellationToken ct = default)
	{
		return await appDbContext.Movies
			.Where(m => m.ReleaseYear == year)
			.Include(m => m.Reviews)
			.AsNoTracking()
			.ToArrayAsync(ct);
	}

	/// <inheritdoc/>
	public async Task<IReadOnlyList<Movie>> SearchByTitleAsync(string searchTerm, CancellationToken ct = default)
	{
		if (string.IsNullOrWhiteSpace(searchTerm))
			return Array.Empty<Movie>();

		var pattern = $"%{searchTerm}%";
		return await appDbContext.Movies
			.Where(m => EF.Functions.Like(m.Title, pattern))
			.Include(m => m.Reviews)
			.AsNoTracking()
			.ToArrayAsync(ct);
	}

	/// <inheritdoc/>
	public async Task<IReadOnlyList<Movie>> GetByYearRangeAsync(int startYear, int endYear, CancellationToken ct = default)
	{
		if (startYear > endYear)
		{
			throw new ArgumentException("startYear must be less than or equal to endYear.", nameof(startYear));
		}

		return await appDbContext.Movies
			.Where(m => m.ReleaseYear >= startYear && m.ReleaseYear <= endYear)
			.Include(m => m.Reviews)
			.AsNoTracking()
			.ToArrayAsync(ct);
	}

	/// <inheritdoc/>
	public async Task<IReadOnlyList<Movie>> GetMoviesByMinRatingAsync(double minRating, CancellationToken ct = default)
	{
		return await appDbContext.Movies
			.Where(m => m.Reviews != null && m.Reviews.Any())
			.Where(m => m.Reviews!.Average(r => r.Rating) >= minRating)
			.Include(m => m.Reviews)
			.AsNoTracking()
			.ToArrayAsync(ct);
	}

	/// <inheritdoc/>
	public async Task<IReadOnlyList<Movie>> GetMoviesWithReviewsAsync(CancellationToken ct = default)
	{
		return await appDbContext.Movies
			.Where(m => m.Reviews != null && m.Reviews.Any())
			.Include(m => m.Reviews)
			.AsNoTracking()
			.ToArrayAsync(ct);
	}

	/// <inheritdoc/>
	public async Task<IReadOnlyList<Movie>> GetTopRatedAsync(int count, CancellationToken ct = default)
	{
		if (count <= 0)
		{
			return Array.Empty<Movie>();
		}

		return await appDbContext.Movies
			.Where(m => m.Reviews != null && m.Reviews.Any())
			.OrderByDescending(m => m.Reviews!.Average(r => r.Rating))
			.Take(count)
			.Include(m => m.Reviews)
			.AsNoTracking()
			.ToArrayAsync(ct);
	}

	// --- Other Operations ---

	/// <inheritdoc/>
	public async Task<IReadOnlyList<string>> GetAllGenresAsync(CancellationToken ct = default)
	{
		return await appDbContext.Movies
			.Select(m => m.Genre)
			.Distinct()
			.Where(g => !string.IsNullOrEmpty(g))
			.ToArrayAsync(ct);
	}

	/// <inheritdoc/>
	public async Task<int> CountByGenreAsync(string genre, CancellationToken ct = default)
	{
		if (string.IsNullOrWhiteSpace(genre))
		{
			return 0;
		}
		return await appDbContext.Movies
			.CountAsync(m => m.Genre == genre, ct);
	}
}

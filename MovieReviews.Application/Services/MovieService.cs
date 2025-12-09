using MovieReviews.Application.DTOs.Movies;
using MovieReviews.Application.Interfaces;
using MovieReviews.Application.Mappers;
using MovieReviews.Domain.Interfaces;

namespace MovieReviews.Application.Services;

/// <summary>
/// Application service for movie-related business operations.
/// Orchestrates repository calls, mapping, and business logic.
/// </summary>
/// <param name="movieRepository">Movie data repository.</param>
public class MovieService(IMovieRepository movieRepository) : IMovieService
{
	// --- Movie Management ---

	/// <inheritdoc/>
	public async Task<IReadOnlyList<MovieDto>> GetAllAsync(CancellationToken ct = default)
	{
		var movies = await movieRepository.GetAllAsync(ct);
		return movies.Select(m => m.ToDto()).ToArray();
	}

	/// <inheritdoc/>
	public async Task<MovieDto?> GetByIdAsync(int id, CancellationToken ct = default)
	{
		var movie = await movieRepository.GetByIdAsync(id, ct);
		return movie?.ToDto();
	}

	/// <inheritdoc/>
	public async Task<MovieDto> CreateAsync(CreateMovieDto createMovieDto, CancellationToken ct = default)
	{
		var movie = createMovieDto.ToEntity();
		await movieRepository.AddAsync(movie, ct);
		return movie.ToDto();
	}

	/// <inheritdoc/>
	public async Task<MovieDto?> UpdateAsync(int id, UpdateMovieDto updateMovieDto, CancellationToken ct = default)
	{
		var existingMovie = await movieRepository.GetByIdAsync(id, ct);
		if (existingMovie is null)
		{
			return null;
		}
		var updatedMovie = existingMovie.UpdateEntity(updateMovieDto);
		await movieRepository.UpdateAsync(updatedMovie, ct);
		return updatedMovie.ToDto();
	}

	/// <inheritdoc/>
	public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
	{
		var existingMovie = await movieRepository.GetByIdAsync(id, ct);
		if (existingMovie is null)
		{
			return false;
		}
		await movieRepository.DeleteAsync(existingMovie, ct);
		return true;
	}

	// --- Additional Movie Operations ---

	/// <inheritdoc/>
	public async Task<IReadOnlyList<MovieWithReviewsDto>> GetMoviesWithReviewsAsync(CancellationToken ct = default)
	{
		var movies = await movieRepository.GetMoviesWithReviewsAsync(ct);
		return movies.Select(m => m.ToWithReviewsDto()).ToArray();
	}

	/// <inheritdoc/>
	public async Task<MovieWithReviewsDto?> GetMovieWithReviewsByIdAsync(int id, CancellationToken ct = default)
	{
		var movie = await movieRepository.GetByIdAsync(id, ct);
		return movie?.ToWithReviewsDto();
	}

	// --- Business Logic Specific to Movies ---

	/// <inheritdoc/>
	public async Task<IReadOnlyList<MovieDto>> GetByDirectorAsync(string director, CancellationToken ct = default)
	{
		// Normalize input at the application boundary: trim and validate here
		var normalized = director?.Trim();
		if (string.IsNullOrWhiteSpace(normalized))
		{
			return Array.Empty<MovieDto>();
		}

		var movies = await movieRepository.GetByDirectorAsync(normalized, ct);
		return movies.Select(m => m.ToDto()).ToArray();
	}

	/// <inheritdoc/>
	public async Task<IReadOnlyList<MovieDto>> GetByGenreAsync(string genre, CancellationToken ct = default)
	{
		if (string.IsNullOrWhiteSpace(genre))
		{
			throw new ArgumentException("Genre cannot be null or empty.", nameof(genre));
		}
		var normalized = genre.Trim();
		var movies = await movieRepository.GetByGenreAsync(normalized, ct);
		return movies.Select(m => m.ToDto()).ToArray();
	}

	/// <inheritdoc/>
	public async Task<IReadOnlyList<MovieDto>> GetByReleaseYearAsync(int year, CancellationToken ct = default)
	{	
		if (year < 1888 || year > DateTime.UtcNow.Year + 1) // Movies started around 1888
		{
			throw new ArgumentOutOfRangeException(nameof(year), "Release year is out of valid range.");
		}
		var movies = await movieRepository.GetByReleaseYearAsync(year, ct);
		return movies.Select(m => m.ToDto()).ToArray();
	}

	/// <inheritdoc/>
	public async Task<IReadOnlyList<MovieDto>> SearchByTitleAsync(string searchTerm, CancellationToken ct = default)
	{
		var normalized = searchTerm?.Trim();
		if (string.IsNullOrWhiteSpace(normalized))
		{
			return Array.Empty<MovieDto>();
		}
		var movies = await movieRepository.SearchByTitleAsync(normalized, ct);
		return movies.Select(m => m.ToDto()).ToArray();
	}

	// --- Other Operations ---

	/// <inheritdoc/>
	public async Task<IReadOnlyList<string>> GetAllGenresAsync(CancellationToken ct = default)
	{
		return await movieRepository.GetAllGenresAsync(ct);
	}

	/// <inheritdoc/>
	public async Task<int> CountByGenreAsync(string genre, CancellationToken ct = default)
	{
		var normalized = genre?.Trim();
		if (string.IsNullOrWhiteSpace(normalized))
		{
			return 0;
		}
		return await movieRepository.CountByGenreAsync(normalized, ct);
	}
}

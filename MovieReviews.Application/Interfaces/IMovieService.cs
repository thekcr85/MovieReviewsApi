using MovieReviews.Application.DTOs.Movies;

namespace MovieReviews.Application.Interfaces;

/// <summary>
/// Application service interface for movie-related business operations.
/// Defines contracts for managing movies and their associated data.
/// </summary>
public interface IMovieService
{
	// --- Movie Management ---
	
	/// <summary>
	/// Retrieves all movies asynchronously.
	/// </summary>
	/// <param name="ct">Cancellation token.</param>
	/// <returns>A read-only collection of all movies as DTOs.</returns>
	Task<IReadOnlyList<MovieDto>> GetAllAsync(CancellationToken ct = default);
	
	/// <summary>
	/// Retrieves a single movie by ID asynchronously.
	/// </summary>
	/// <param name="id">The movie identifier.</param>
	/// <param name="ct">Cancellation token.</param>
	/// <returns>A movie DTO or null if not found.</returns>
	Task<MovieDto?> GetByIdAsync(int id, CancellationToken ct = default);
	
	/// <summary>
	/// Creates a new movie asynchronously.
	/// </summary>
	/// <param name="createMovieDto">DTO containing movie details.</param>
	/// <param name="ct">Cancellation token.</param>
	/// <returns>The created movie DTO with assigned ID.</returns>
	Task<MovieDto> CreateAsync(CreateMovieDto createMovieDto, CancellationToken ct = default);
	
	/// <summary>
	/// Updates an existing movie asynchronously.
	/// </summary>
	/// <param name="id">The movie ID to update.</param>
	/// <param name="updateMovieDto">DTO containing updated movie details.</param>
	/// <param name="ct">Cancellation token.</param>
	/// <returns>The updated movie DTO or null if not found.</returns>
	Task<MovieDto?> UpdateAsync(int id, UpdateMovieDto updateMovieDto, CancellationToken ct = default);
	
	/// <summary>
	/// Deletes a movie by ID asynchronously.
	/// </summary>
	/// <param name="id">The movie identifier.</param>
	/// <param name="ct">Cancellation token.</param>
	/// <returns>True if deleted successfully; false if not found.</returns>
	Task<bool> DeleteAsync(int id, CancellationToken ct = default);

	// --- Additional Movie Operations ---
	
	/// <summary>
	/// Retrieves all movies that have associated reviews asynchronously.
	/// </summary>
	/// <param name="ct">Cancellation token.</param>
	/// <returns>A read-only collection of movies with their reviews.</returns>
	Task<IReadOnlyList<MovieWithReviewsDto>> GetMoviesWithReviewsAsync(CancellationToken ct = default);
	
	/// <summary>
	/// Retrieves a single movie with its reviews by ID asynchronously.
	/// </summary>
	/// <param name="id">The movie identifier.</param>
	/// <param name="ct">Cancellation token.</param>
	/// <returns>A movie with reviews DTO or null if not found.</returns>
	Task<MovieWithReviewsDto?> GetMovieWithReviewsByIdAsync(int id, CancellationToken ct = default);

	// --- Business Logic Specific to Movies ---
	
	/// <summary>
	/// Retrieves all movies by a specific director asynchronously.
	/// </summary>
	/// <param name="director">The director's name.</param>
	/// <param name="ct">Cancellation token.</param>
	/// <returns>A read-only collection of movies directed by the specified director.</returns>
	Task<IReadOnlyList<MovieDto>> GetByDirectorAsync(string director, CancellationToken ct = default);
	
	/// <summary>
	/// Retrieves all movies in a specific genre asynchronously.
	/// </summary>
	/// <param name="genre">The genre name (e.g., Action, Drama, Comedy).</param>
	/// <param name="ct">Cancellation token.</param>
	/// <returns>A read-only collection of movies in the specified genre.</returns>
	Task<IReadOnlyList<MovieDto>> GetByGenreAsync(string genre, CancellationToken ct = default);
	
	/// <summary>
	/// Retrieves all movies released in a specific year asynchronously.
	/// </summary>
	/// <param name="year">The release year.</param>
	/// <param name="ct">Cancellation token.</param>
	/// <returns>A read-only collection of movies released in the specified year.</returns>
	Task<IReadOnlyList<MovieDto>> GetByReleaseYearAsync(int year, CancellationToken ct = default);
	
	/// <summary>
	/// Searches for movies by title containing the search term asynchronously.
	/// </summary>
	/// <param name="searchTerm">The text to search for in movie titles.</param>
	/// <param name="ct">Cancellation token.</param>
	/// <returns>A read-only collection of movies matching the search criteria.</returns>
	Task<IReadOnlyList<MovieDto>> SearchByTitleAsync(string searchTerm, CancellationToken ct = default);

	// --- Other Operations ---
	
	/// <summary>
	/// Retrieves a list of all distinct genres in the catalog asynchronously.
	/// </summary>
	/// <param name="ct">Cancellation token.</param>
	/// <returns>A read-only collection of unique genre names.</returns>
	Task<IReadOnlyList<string>> GetAllGenresAsync(CancellationToken ct = default);
	
	/// <summary>
	/// Counts the number of movies in a specific genre asynchronously.
	/// </summary>
	/// <param name="genre">The genre name.</param>
	/// <param name="ct">Cancellation token.</param>
	/// <returns>The count of movies in the specified genre.</returns>
	Task<int> CountByGenreAsync(string genre, CancellationToken ct = default);
}

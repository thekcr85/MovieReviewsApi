using MovieReviews.Domain.Entities;

namespace MovieReviews.Domain.Interfaces;

/// <summary>
/// Defines contract for movie repository operations.
/// Provides methods for CRUD operations and domain-specific queries on movies.
/// </summary>
public interface IMovieRepository
{
	/// <summary>
	/// Retrieves all movies from the data store.
	/// </summary>
	/// <param name="ct">Token to monitor for cancellation requests.</param>
	/// <returns>
	/// A task representing the asynchronous operation.
	/// The task result contains a read-only collection of all movies.
	/// </returns>
	Task<IReadOnlyList<Movie>> GetAllAsync(CancellationToken ct = default);

	/// <summary>
	/// Retrieves a single movie by its unique identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the movie.</param>
	/// <param name="ct">Token to monitor for cancellation requests.</param>
	/// <returns>
	/// A task representing the asynchronous operation.
	/// The task result contains the movie if found; otherwise, <see langword="null"/>.
	/// </returns>
	Task<Movie?> GetByIdAsync(int id, CancellationToken ct = default);

	/// <summary>
	/// Adds a new movie to the data store.
	/// </summary>
	/// <param name="movie">The movie entity to add.</param>
	/// <param name="ct">Token to monitor for cancellation requests.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="movie"/> is <see langword="null"/>.</exception>
	Task AddAsync(Movie movie, CancellationToken ct = default);

	/// <summary>
	/// Updates an existing movie in the data store.
	/// </summary>
	/// <param name="movie">The movie entity with updated values.</param>
	/// <param name="ct">Token to monitor for cancellation requests.</param>
	/// <returns>
	/// A task representing the asynchronous operation.
	/// The task result contains the updated movie entity.
	/// </returns>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="movie"/> is <see langword="null"/>.</exception>
	/// <exception cref="InvalidOperationException">Thrown when the movie with the specified ID is not found.</exception>
	Task<Movie> UpdateAsync(Movie movie, CancellationToken ct = default);

	/// <summary>
	/// Deletes a movie from the data store.
	/// </summary>
	/// <param name="movie">The movie entity to delete.</param>
	/// <param name="ct">Token to monitor for cancellation requests.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="movie"/> is <see langword="null"/>.</exception>
	/// <exception cref="InvalidOperationException">Thrown when the movie with the specified ID is not found.</exception>
	Task DeleteAsync(Movie movie, CancellationToken ct = default);

	/// <summary>
	/// Retrieves all movies that have at least one review.
	/// </summary>
	/// <param name="ct">Token to monitor for cancellation requests.</param>
	/// <returns>
	/// A task representing the asynchronous operation.
	/// The task result contains a read-only collection of movies with associated reviews.
	/// </returns>
	Task<IReadOnlyList<Movie>> GetMoviesWithReviewsAsync(CancellationToken ct = default);

	/// <summary>
	/// Retrieves all movies directed by a specific director.
	/// </summary>
	/// <param name="director">The name of the director.</param>
	/// <param name="ct">Token to monitor for cancellation requests.</param>
	/// <returns>
	/// A task representing the asynchronous operation.
	/// The task result contains a read-only collection of movies directed by the specified director.
	/// </returns>
	Task<IReadOnlyList<Movie>> GetByDirectorAsync(string director, CancellationToken ct = default);

	/// <summary>
	/// Retrieves all movies released in a specific year.
	/// </summary>
	/// <param name="year">The release year.</param>
	/// <param name="ct">Token to monitor for cancellation requests.</param>
	/// <returns>
	/// A task representing the asynchronous operation.
	/// The task result contains a read-only collection of movies released in the specified year.
	/// </returns>
	Task<IReadOnlyList<Movie>> GetByReleaseYearAsync(int year, CancellationToken ct = default);

	/// <summary>
	/// Retrieves all movies released within a specific year range.
	/// </summary>
	/// <param name="startYear">The start year (inclusive).</param>
	/// <param name="endYear">The end year (inclusive).</param>
	/// <param name="ct">Token to monitor for cancellation requests.</param>
	/// <returns>
	/// A task representing the asynchronous operation.
	/// The task result contains a read-only collection of movies released within the specified range.
	/// </returns>
	/// <exception cref="ArgumentException">Thrown when <paramref name="startYear"/> is greater than <paramref name="endYear"/>.</exception>
	Task<IReadOnlyList<Movie>> GetByYearRangeAsync(int startYear, int endYear, CancellationToken ct = default);

	/// <summary>
	/// Retrieves all movies belonging to a specific genre.
	/// </summary>
	/// <param name="genre">The genre name (e.g., Action, Drama, Comedy).</param>
	/// <param name="ct">Token to monitor for cancellation requests.</param>
	/// <returns>
	/// A task representing the asynchronous operation.
	/// The task result contains a read-only collection of movies in the specified genre.
	/// </returns>
	Task<IReadOnlyList<Movie>> GetByGenreAsync(string genre, CancellationToken ct = default);

	/// <summary>
	/// Searches for movies whose title contains the specified search term (case-insensitive).
	/// </summary>
	/// <param name="searchTerm">The text to search for within movie titles.</param>
	/// <param name="ct">Token to monitor for cancellation requests.</param>
	/// <returns>
	/// A task representing the asynchronous operation.
	/// The task result contains a read-only collection of movies matching the search term.
	/// </returns>
	Task<IReadOnlyList<Movie>> SearchByTitleAsync(string searchTerm, CancellationToken ct = default);

	/// <summary>
	/// Retrieves the top-rated movies, ordered by average rating in descending order.
	/// </summary>
	/// <param name="count">The maximum number of movies to return.</param>
	/// <param name="ct">Token to monitor for cancellation requests.</param>
	/// <returns>
	/// A task representing the asynchronous operation.
	/// The task result contains a read-only collection of the top-rated movies.
	/// </returns>
	Task<IReadOnlyList<Movie>> GetTopRatedAsync(int count, CancellationToken ct = default);

	/// <summary>
	/// Retrieves movies with an average rating equal to or above the specified minimum.
	/// </summary>
	/// <param name="minRating">The minimum average rating threshold.</param>
	/// <param name="ct">Token to monitor for cancellation requests.</param>
	/// <returns>
	/// A task representing the asynchronous operation.
	/// The task result contains a read-only collection of movies meeting the rating criteria.
	/// </returns>
	Task<IReadOnlyList<Movie>> GetMoviesByMinRatingAsync(double minRating, CancellationToken ct = default);


	/// <summary>
	/// Retrieves a distinct list of all genres available in the movie catalog.
	/// </summary>
	/// <param name="ct">Token to monitor for cancellation requests.</param>
	/// <returns>
	/// A task representing the asynchronous operation.
	/// The task result contains a read-only collection of unique genre names.
	/// </returns>
	Task<IReadOnlyList<string>> GetAllGenresAsync(CancellationToken ct = default);

	/// <summary>
	/// Counts the total number of movies in a specific genre.
	/// </summary>
	/// <param name="genre">The genre name.</param>
	/// <param name="ct">Token to monitor for cancellation requests.</param>
	/// <returns>
	/// A task representing the asynchronous operation.
	/// The task result contains the count of movies in the specified genre.
	/// </returns>
	Task<int> CountByGenreAsync(string genre, CancellationToken ct = default);
}
using MovieReviews.Domain.Entities;

namespace MovieReviews.Domain.Interfaces;

/// <summary>
/// Defines contract for review repository operations.
/// Provides methods for CRUD operations and domain-specific queries on reviews.
/// </summary>
public interface IReviewRepository
{
	// --- Standard CRUD operations ---

	/// <summary>
	/// Retrieves all reviews from the data store.
	/// </summary>
	/// <param name="ct">Token to monitor for cancellation requests.</param>
	/// <returns>
	/// A task representing the asynchronous operation.
	/// The task result contains a read-only collection of all reviews.
	/// </returns>
	Task<IReadOnlyList<Review>> GetAllAsync(CancellationToken ct = default);

	/// <summary>
	/// Retrieves a single review by its unique identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the review.</param>
	/// <param name="ct">Token to monitor for cancellation requests.</param>
	/// <returns>
	/// A task representing the asynchronous operation.
	/// The task result contains the review if found; otherwise, <see langword="null"/>.
	/// </returns>
	Task<Review?> GetByIdAsync(int id, CancellationToken ct = default);
	
	/// <summary>
	/// Adds a new review to the data store.
	/// </summary>
	/// <param name="review">The review entity to add.</param>
	/// <param name="ct">Token to monitor for cancellation requests.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="review"/> is <see langword="null"/>.</exception>
	Task AddAsync(Review review, CancellationToken ct = default);

	/// <summary>
	/// Updates an existing review in the data store.
	/// </summary>
	/// <param name="review">The review entity with updated values.</param>
	/// <param name="ct">Token to monitor for cancellation requests.</param>
	/// <returns>
	/// A task representing the asynchronous operation.
	/// The task result contains the updated review entity.
	/// </returns>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="review"/> is <see langword="null"/>.</exception>
	/// <exception cref="InvalidOperationException">Thrown when the review with the specified ID is not found.</exception>
	Task<Review> UpdateAsync(Review review, CancellationToken ct = default);

	/// <summary>
	/// Deletes a review from the data store.
	/// </summary>
	/// <param name="review">The review entity to delete.</param>
	/// <param name="ct">Token to monitor for cancellation requests.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="review"/> is <see langword="null"/>.</exception>
	/// <exception cref="InvalidOperationException">Thrown when the review with the specified ID is not found.</exception>
	Task DeleteAsync(Review review, CancellationToken ct = default);

	// --- Domain-specific queries ---

	/// <summary>
	/// Retrieves all reviews associated with a specific movie.
	/// </summary>
	/// <param name="movieId">The unique identifier of the movie.</param>
	/// <param name="ct">Token to monitor for cancellation requests.</param>
	/// <returns>
	/// A task representing the asynchronous operation.
	/// The task result contains a read-only collection of reviews for the specified movie.
	/// </returns>
	Task<IReadOnlyList<Review>> GetByMovieIdAsync(int movieId, CancellationToken ct = default);

	/// <summary>
	/// Retrieves reviews within a specific rating range.
	/// </summary>
	/// <param name="minRating">The minimum rating value (inclusive).</param>
	/// <param name="maxRating">The maximum rating value (inclusive).</param>
	/// <param name="ct">Token to monitor for cancellation requests.</param>
	/// <returns>
	/// A task representing the asynchronous operation.
	/// The task result contains a read-only collection of reviews within the specified rating range.
	/// </returns>
	Task<IReadOnlyList<Review>> GetByRatingRangeAsync(int minRating, int maxRating, CancellationToken ct = default);

	/// <summary>
	/// Retrieves all reviews written by a specific reviewer.
	/// </summary>
	/// <param name="reviewerName">The name of the reviewer.</param>
	/// <param name="ct">Token to monitor for cancellation requests.</param>
	/// <returns>
	/// A task representing the asynchronous operation.
	/// The task result contains a read-only collection of reviews by the specified reviewer.
	/// </returns>
	Task<IReadOnlyList<Review>> GetByReviewerNameAsync(string reviewerName, CancellationToken ct = default);

	/// <summary>
	/// Counts the total number of reviews for a specific movie.
	/// </summary>
	/// <param name="movieId">The unique identifier of the movie.</param>
	/// <param name="ct">Token to monitor for cancellation requests.</param>
	/// <returns>
	/// A task representing the asynchronous operation.
	/// The task result contains the total count of reviews for the specified movie.
	/// </returns>
	Task<int> CountByMovieIdAsync(int movieId, CancellationToken ct = default);
}

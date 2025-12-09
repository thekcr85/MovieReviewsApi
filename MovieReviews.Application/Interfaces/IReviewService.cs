using MovieReviews.Application.DTOs.Reviews;

namespace MovieReviews.Application.Interfaces;

/// <summary>
/// Application service interface for review-related business operations.
/// Defines contracts for managing movie reviews.
/// </summary>
public interface IReviewService
{
	// --- Review Management ---
	
	/// <summary>
	/// Retrieves all reviews asynchronously.
	/// </summary>
	/// <param name="ct">Cancellation token.</param>
	/// <returns>A read-only collection of all reviews as DTOs.</returns>
	Task<IReadOnlyList<ReviewDto>> GetAllAsync(CancellationToken ct = default);

	/// <summary>
	/// Retrieves a single review by ID asynchronously.
	/// </summary>
	/// <param name="id">The review identifier.</param>
	/// <param name="ct">Cancellation token.</param>
	/// <returns>A review DTO or null if not found.</returns>
	Task<ReviewDto?> GetByIdAsync(int id, CancellationToken ct = default);

	/// <summary>
	/// Creates a new review asynchronously.
	/// </summary>
	/// <param name="createReviewDto">DTO containing review details.</param>
	/// <param name="ct">Cancellation token.</param>
	/// <returns>The created review DTO with assigned ID.</returns>
	Task<ReviewDto> CreateAsync(CreateReviewDto createReviewDto, CancellationToken ct = default);

	/// <summary>
	/// Updates an existing review asynchronously.
	/// </summary>
	/// <param name="id">The review ID to update.</param>
	/// <param name="updateReviewDto">DTO containing updated review details.</param>
	/// <param name="ct">Cancellation token.</param>
	/// <returns>The updated review DTO or null if not found.</returns>
	Task<ReviewDto?> UpdateAsync(int id, UpdateReviewDto updateReviewDto, CancellationToken ct = default);

	/// <summary>
	/// Deletes a review by ID asynchronously.
	/// </summary>
	/// <param name="id">The review identifier.</param>
	/// <param name="ct">Cancellation token.</param>
	/// <returns>True if deleted successfully; false if not found.</returns>
	Task<bool> DeleteAsync(int id, CancellationToken ct = default);

	// --- Review Business Logic ---

	/// <summary>
	/// Retrieves all reviews for a specific movie asynchronously.
	/// </summary>
	/// <param name="movieId">The movie identifier.</param>
	/// <param name="ct">Cancellation token.</param>
	/// <returns>A read-only collection of reviews for the specified movie.</returns>
	Task<IReadOnlyList<ReviewDto>> GetByMovieIdAsync(int movieId, CancellationToken ct = default);

	/// <summary>
	/// Retrieves all reviews by a specific reviewer asynchronously.
	/// </summary>
	/// <param name="reviewerName">The reviewer's name.</param>
	/// <param name="ct">Cancellation token.</param>
	/// <returns>A read-only collection of reviews by the specified reviewer.</returns>
	Task<IReadOnlyList<ReviewDto>> GetByReviewerAsync(string reviewerName, CancellationToken ct = default);

	/// <summary>
	/// Retrieves reviews with a minimum rating asynchronously.
	/// </summary>
	/// <param name="minRating">The minimum rating threshold (1-10).</param>
	/// <param name="ct">Cancellation token.</param>
	/// <returns>A read-only collection of reviews meeting the minimum rating criteria.</returns>
	Task<IReadOnlyList<ReviewDto>> GetByMinimumRatingAsync(int minRating, CancellationToken ct = default);

	/// <summary>
	/// Calculates the average rating for a specific movie asynchronously.
	/// </summary>
	/// <param name="movieId">The movie identifier.</param>
	/// <param name="ct">Cancellation token.</param>
	/// <returns>The average rating for the movie (0 if no reviews exist).</returns>
	Task<double> GetAverageRatingByMovieIdAsync(int movieId, CancellationToken ct = default);

	/// <summary>
	/// Counts the number of reviews for a specific movie asynchronously.
	/// </summary>
	/// <param name="movieId">The movie identifier.</param>
	/// <param name="ct">Cancellation token.</param>
	/// <returns>The count of reviews for the specified movie.</returns>
	Task<int> CountByMovieIdAsync(int movieId, CancellationToken ct = default);
}

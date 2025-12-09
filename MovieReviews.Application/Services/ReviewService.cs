using MovieReviews.Application.DTOs.Reviews;
using MovieReviews.Application.Interfaces;
using MovieReviews.Application.Mappers;
using MovieReviews.Domain.Interfaces;

namespace MovieReviews.Application.Services;

/// <summary>
/// Application service for review-related business operations.
/// Orchestrates repository calls, mapping, and business logic.
/// </summary>
/// <param name="reviewRepository">Review data repository.</param>
public class ReviewService(IReviewRepository reviewRepository) : IReviewService
{
	// --- Review Management ---

	/// <inheritdoc/>
	public async Task<IReadOnlyList<ReviewDto>> GetAllAsync(CancellationToken ct = default)
	{
		var reviews = await reviewRepository.GetAllAsync(ct);
		return reviews.Select(r => r.ToDto()).ToArray();
	}

	/// <inheritdoc/>
	public async Task<ReviewDto?> GetByIdAsync(int id, CancellationToken ct = default)
	{
		var review = await reviewRepository.GetByIdAsync(id, ct);
		return review?.ToDto();
	}

	/// <inheritdoc/>
	public async Task<ReviewDto> CreateAsync(CreateReviewDto createReviewDto, CancellationToken ct = default)
	{
		ArgumentNullException.ThrowIfNull(createReviewDto, nameof(createReviewDto));

		var review = createReviewDto.ToEntity();
		await reviewRepository.AddAsync(review, ct);
		return review.ToDto();
	}

	/// <inheritdoc/>
	public async Task<ReviewDto?> UpdateAsync(int id, UpdateReviewDto updateReviewDto, CancellationToken ct = default)
	{
		ArgumentNullException.ThrowIfNull(updateReviewDto, nameof(updateReviewDto));

		if (id != updateReviewDto.Id)
		{
			throw new ArgumentException("Review ID in route does not match DTO ID.", nameof(id));
		}

		var existingReview = await reviewRepository.GetByIdAsync(id, ct);
		if (existingReview is null)
		{
			return null;
		}

		var updatedReview = existingReview.UpdateEntity(updateReviewDto);
		await reviewRepository.UpdateAsync(updatedReview, ct);
		return updatedReview.ToDto();
	}

	/// <inheritdoc/>
	public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
	{
		var existingReview = await reviewRepository.GetByIdAsync(id, ct);
		if (existingReview is null)
		{
			return false;
		}

		await reviewRepository.DeleteAsync(existingReview, ct);
		return true;
	}

	// --- Review Business Logic ---

	/// <inheritdoc/>
	public async Task<IReadOnlyList<ReviewDto>> GetByMovieIdAsync(int movieId, CancellationToken ct = default)
	{
		if (movieId <= 0)
		{
			throw new ArgumentException("Movie ID must be a positive integer.", nameof(movieId));
		}

		var reviews = await reviewRepository.GetByMovieIdAsync(movieId, ct);
		return reviews.Select(r => r.ToDto()).ToArray();
	}

	/// <inheritdoc/>
	public async Task<IReadOnlyList<ReviewDto>> GetByReviewerAsync(string reviewerName, CancellationToken ct = default)
	{
		var normalized = reviewerName?.Trim();
		if (string.IsNullOrWhiteSpace(normalized))
		{
			return Array.Empty<ReviewDto>();
		}

		var reviews = await reviewRepository.GetByReviewerNameAsync(normalized, ct);
		return reviews.Select(r => r.ToDto()).ToArray();
	}

	/// <inheritdoc/>
	public async Task<IReadOnlyList<ReviewDto>> GetByMinimumRatingAsync(int minRating, CancellationToken ct = default)
	{
		if (minRating < 1 || minRating > 10)
		{
			throw new ArgumentOutOfRangeException(nameof(minRating), "Rating must be between 1 and 10.");
		}

		var reviews = await reviewRepository.GetByRatingRangeAsync(minRating, 10, ct);
		return reviews.Select(r => r.ToDto()).ToArray();
	}

	/// <inheritdoc/>
	public async Task<double> GetAverageRatingByMovieIdAsync(int movieId, CancellationToken ct = default)
	{
		if (movieId <= 0)
		{
			throw new ArgumentException("Movie ID must be a positive integer.", nameof(movieId));
		}

		var reviews = await reviewRepository.GetByMovieIdAsync(movieId, ct);
		if (reviews.Count == 0)
		{
			return 0.0;
		}

		return reviews.Average(r => r.Rating);
	}

	/// <inheritdoc/>
	public async Task<int> CountByMovieIdAsync(int movieId, CancellationToken ct = default)
	{
		if (movieId <= 0)
		{
			throw new ArgumentException("Movie ID must be a positive integer.", nameof(movieId));
		}

		return await reviewRepository.CountByMovieIdAsync(movieId, ct);
	}
}

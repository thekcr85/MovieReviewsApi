using Microsoft.EntityFrameworkCore;
using MovieReviews.Domain.Entities;
using MovieReviews.Domain.Interfaces;
using MovieReviews.Infrastructure.Persistence;

namespace MovieReviews.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for review data access operations.
/// </summary>
/// <param name="appDbContext">The database context instance.</param>
public class ReviewRepository(AppDbContext appDbContext) : IReviewRepository
{
	/// <inheritdoc/>
	public async Task<IReadOnlyList<Review>> GetAllAsync(CancellationToken cancellationToken = default)
	{
		return await appDbContext.Reviews
			.Include(r => r.Movie)
			.AsNoTracking()
			.ToListAsync(cancellationToken);
	}

	/// <inheritdoc/>
	public async Task<Review?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
	{
		return await appDbContext.Reviews
			.Include(r => r.Movie)
			.AsNoTracking()
			.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
	}

	/// <inheritdoc/>
	public async Task AddAsync(Review review, CancellationToken cancellationToken = default)
	{
		await appDbContext.Reviews.AddAsync(review, cancellationToken);
		await appDbContext.SaveChangesAsync(cancellationToken);
	}

	/// <inheritdoc/>
	public async Task<Review> UpdateAsync(Review review, CancellationToken cancellationToken = default)
	{
		var existingReview = await appDbContext.Reviews.FirstOrDefaultAsync(r => r.Id == review.Id, cancellationToken);

		if (existingReview is null)
		{
			throw new InvalidOperationException($"Review with ID {review.Id} not found.");
		}

		appDbContext.Entry(existingReview).CurrentValues.SetValues(review);
		await appDbContext.SaveChangesAsync(cancellationToken);
		return existingReview;
	}

	/// <inheritdoc/>
	public async Task DeleteAsync(Review review, CancellationToken cancellationToken = default)
	{
		var existingReview = await appDbContext.Reviews.FirstOrDefaultAsync(r => r.Id == review.Id, cancellationToken);
		if (existingReview is null)
		{
			throw new InvalidOperationException($"Review with ID {review.Id} not found.");
		}
		appDbContext.Reviews.Remove(existingReview);
		await appDbContext.SaveChangesAsync(cancellationToken);
	}

	// --- Domain-specific queries ---

	/// <inheritdoc/>
	public async Task<IReadOnlyList<Review>> GetByMovieIdAsync(int movieId, CancellationToken cancellationToken = default)
	{
		return await appDbContext.Reviews
			.Include(r => r.Movie)
			.AsNoTracking()
			.Where(r => r.MovieId == movieId)
			.ToListAsync(cancellationToken);
	}

	/// <inheritdoc/>
	public async Task<IReadOnlyList<Review>> GetByRatingRangeAsync(int minRating, int maxRating, CancellationToken cancellationToken = default)
	{
		return await appDbContext.Reviews
			.Include(r => r.Movie)
			.AsNoTracking()
			.Where(r => r.Rating >= minRating && r.Rating <= maxRating)
			.ToListAsync(cancellationToken);
	}

	/// <inheritdoc/>
	public async Task<IReadOnlyList<Review>> GetByReviewerNameAsync(string reviewerName, CancellationToken cancellationToken = default)
	{
		return await appDbContext.Reviews
			.Include(r => r.Movie)
			.AsNoTracking()
			.Where(r => r.ReviewerName == reviewerName)
			.ToListAsync(cancellationToken);
	}

	/// <inheritdoc/>
	public async Task<int> CountByMovieIdAsync(int movieId, CancellationToken cancellationToken = default)
	{
		return await appDbContext.Reviews
			.AsNoTracking()
			.CountAsync(r => r.MovieId == movieId, cancellationToken);
	}
}

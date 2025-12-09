using System.ComponentModel.DataAnnotations;

namespace MovieReviews.Application.DTOs.Reviews;

/// <summary>
/// DTO for creating a new movie review.
/// </summary>
/// <param name="MovieId">The ID of the movie being reviewed (must be a valid positive integer).</param>
/// <param name="ReviewerName">Name of the reviewer (1-100 characters).</param>
/// <param name="Rating">Rating given to the movie (1-10).</param>
/// <param name="Comment">Optional text comment of the review (max 1000 characters).</param>
public sealed record CreateReviewDto(
	[Required, Range(1, int.MaxValue, ErrorMessage = "MovieId must be a valid positive integer.")]
	int MovieId,

	[Required, StringLength(100, MinimumLength = 1, ErrorMessage = "Reviewer name must be between 1 and 100 characters.")]
	string ReviewerName,

	[Required, Range(1, 10, ErrorMessage = "Rating must be between 1 and 10.")]
	int Rating,

	[StringLength(1000, ErrorMessage = "Comment cannot exceed 1000 characters.")]
	string Comment = ""
);
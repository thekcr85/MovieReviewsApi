using System.ComponentModel.DataAnnotations;

namespace MovieReviews.Application.DTOs.Reviews;

/// <summary>
/// DTO for updating an existing movie review.
/// </summary>
/// <param name="Id">The unique identifier of the review to update.</param>
/// <param name="ReviewerName">Updated name of the reviewer (1-100 characters).</param>
/// <param name="Rating">Updated rating for the movie (1-10).</param>
/// <param name="Comment">Updated text comment of the review (max 1000 characters).</param>
public sealed record UpdateReviewDto(
	[Required]
	int Id,

	[Required, StringLength(100, MinimumLength = 1, ErrorMessage = "Reviewer name must be between 1 and 100 characters.")]
	string ReviewerName,

	[Required, Range(1, 10, ErrorMessage = "Rating must be between 1 and 10.")]
	int Rating,

	[StringLength(1000, ErrorMessage = "Comment cannot exceed 1000 characters.")]
	string Comment = ""
);

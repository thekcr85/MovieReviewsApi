using MovieReviews.Application.DTOs.Reviews;

namespace MovieReviews.Application.DTOs.Movies;

/// <summary>
/// DTO for detailed movie information with reviews and average rating.
/// </summary>
/// <param name="Id">Movie identifier.</param>
/// <param name="Title">Movie title.</param>
/// <param name="Director">Director name.</param>
/// <param name="ReleaseYear">Release year.</param>
/// <param name="Genre">Movie genre.</param>
/// <param name="AverageRating">Average rating from reviews.</param>
/// <param name="Reviews">Associated reviews.</param>

public sealed record MovieWithReviewsDto(
	int Id,
	string Title,
	string Director,
	int ReleaseYear,
	string Genre,
	double AverageRating,
	IReadOnlyCollection<ReviewDto> Reviews
);
namespace MovieReviews.Application.DTOs.Reviews;

/// <summary>
/// DTO representing a movie review for read operations.
/// </summary>
/// <param name="Id">Unique identifier of the review.</param>
/// <param name="MovieId">Foreign key referencing the associated movie.</param>
/// <param name="ReviewerName">Name of the reviewer.</param>
/// <param name="Rating">Rating given to the movie (1-10).</param>
/// <param name="Comment">Text comment of the review.</param>
public sealed record ReviewDto(
	int Id,
	int MovieId,
	string ReviewerName,
	int Rating,
	string Comment
);

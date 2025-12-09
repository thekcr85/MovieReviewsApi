namespace MovieReviews.Application.DTOs.Reviews;

/// <summary>
/// DTO representing a movie review including the associated movie title.
/// </summary>
/// <param name="Id">Unique identifier of the review.</param>
/// <param name="MovieId">Foreign key referencing the associated movie.</param>
/// <param name="MovieTitle">Title of the associated movie.</param>
/// <param name="ReviewerName">Name of the reviewer.</param>
/// <param name="Rating">Rating given to the movie (1-10).</param>
/// <param name="Comment">Text comment of the review.</param>
public sealed record ReviewWithMovieDto(
	int Id,
	int MovieId,
	string MovieTitle,
	string ReviewerName,
	int Rating,
	string Comment
);
namespace MovieReviews.Application.DTOs.Movies;

/// <summary>
/// DTO representing a movie for read operations with calculated average rating.
/// </summary>
/// <param name="Id">Unique identifier of the movie.</param>
/// <param name="Title">Title of the movie.</param>
/// <param name="Director">Director of the movie.</param>
/// <param name="ReleaseYear">Year the movie was released.</param>
/// <param name="Genre">Genre of the movie (e.g., Drama, Comedy).</param>
/// <param name="AverageRating">Calculated average rating from all reviews.</param>
public sealed record MovieDto(
	int Id,
	string Title,
	string Director,
	int ReleaseYear,
	string Genre,
	double AverageRating
);

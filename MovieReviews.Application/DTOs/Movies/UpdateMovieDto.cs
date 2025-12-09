using System.ComponentModel.DataAnnotations;

namespace MovieReviews.Application.DTOs.Movies;

/// <summary>
/// DTO for updating an existing movie.
/// </summary>
/// <param name="Id">The unique identifier of the movie to update.</param>
/// <param name="Title">Updated title of the movie (1-200 characters).</param>
/// <param name="Director">Updated director of the movie (1-100 characters).</param>
/// <param name="ReleaseYear">Updated release year (1888-2100).</param>
/// <param name="Genre">Updated genre of the movie (3-50 characters).</param>
public sealed record UpdateMovieDto(
	[Required]
	int Id,

	[Required, StringLength(200, MinimumLength = 1)]
	string Title,

	[Required, StringLength(100, MinimumLength = 1)]
	string Director,

	[Range(1888, 2100)]
	int ReleaseYear,

	[Required, StringLength(50, MinimumLength = 3)]
	string Genre
);

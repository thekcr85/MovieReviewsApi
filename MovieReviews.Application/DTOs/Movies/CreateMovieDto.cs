using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MovieReviews.Application.DTOs.Movies;

/// <summary>
/// DTO for creating a new movie.
/// </summary>
/// <param name="Title">Title of the movie (1-200 characters).</param>
/// <param name="Director">Director of the movie (1-100 characters).</param>
/// <param name="ReleaseYear">Year the movie was released (1888-2100).</param>
/// <param name="Genre">Genre of the movie (3-50 characters).</param>
public sealed record CreateMovieDto(
	[Required, StringLength(200, MinimumLength = 1)]
	string Title,

	[Required, StringLength(100, MinimumLength = 1)]
	string Director,

	[Range(1888, 2100)]
	int ReleaseYear,

	[Required, StringLength(50, MinimumLength = 3)]
	string Genre
);


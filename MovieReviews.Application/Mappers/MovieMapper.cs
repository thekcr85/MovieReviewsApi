using MovieReviews.Domain.Entities;
using MovieReviews.Application.DTOs.Movies;

namespace MovieReviews.Application.Mappers;

/// <summary>
/// Provides mapping methods between Movie entity and Movie DTOs.
/// </summary>
public static class MovieMapper
{
	/// <summary>
	/// Maps a Movie entity to a MovieDto with calculated average rating.
	/// </summary>
	/// <param name="movie">The movie entity to map.</param>
	/// <returns>A MovieDto representation of the movie including average rating.</returns>
	public static MovieDto ToDto(this Movie movie) =>
		new(
			movie.Id,
			movie.Title,
			movie.Director,
			movie.ReleaseYear,
			movie.Genre,
			movie.Reviews.Count != 0 ? movie.Reviews.Average(r => r.Rating) : 0
		);

	/// <summary>
	/// Creates a <see cref="MovieWithReviewsDto"/> instance that represents the specified movie and its associated
	/// reviews.
	/// </summary>
	/// <param name="movie">The <see cref="Movie"/> object to convert. Cannot be null.</param>
	/// <returns>A <see cref="MovieWithReviewsDto"/> containing the movie's details and a list of its reviews. The average rating is
	/// set to 0 if the movie has no reviews.</returns>
	public static MovieWithReviewsDto ToWithReviewsDto(this Movie movie) =>
		new(
			movie.Id,
			movie.Title,
			movie.Director,
			movie.ReleaseYear,
			movie.Genre,
			movie.Reviews.Count != 0 ? movie.Reviews.Average(r => r.Rating) : 0,
			movie.Reviews.Select(r => r.ToDto()).ToList()
		);

	/// <summary>
	/// Maps a CreateMovieDto to a new Movie entity.
	/// </summary>
	/// <param name="dto">The DTO containing movie creation data.</param>
	/// <returns>A new Movie entity.</returns>
	public static Movie ToEntity(this CreateMovieDto dto) =>
		new()
		{
			Title = dto.Title,
			Director = dto.Director,
			ReleaseYear = dto.ReleaseYear,
			Genre = dto.Genre
		};

	/// <summary>
	/// Updates an existing Movie entity with values from UpdateMovieDto.
	/// </summary>
	/// <param name="movie">The existing movie entity.</param>
	/// <param name="dto">The DTO containing updated movie data.</param>
	/// <returns>A new Movie entity with updated values.</returns>
	/// <exception cref="ArgumentNullException">Thrown when movie is null.</exception>
	public static Movie UpdateEntity(this Movie movie, UpdateMovieDto dto)
	{
		ArgumentNullException.ThrowIfNull(movie);

		return new Movie
		{
			Id = movie.Id,
			Title = dto.Title,
			Director = dto.Director,
			ReleaseYear = dto.ReleaseYear,
			Genre = dto.Genre,
			Reviews = movie.Reviews
		};
	}
}
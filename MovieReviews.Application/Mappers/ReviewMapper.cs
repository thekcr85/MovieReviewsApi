using MovieReviews.Application.DTOs.Reviews;
using MovieReviews.Domain.Entities;

namespace MovieReviews.Application.Mappers;

/// <summary>
/// Provides mapping methods between Review entity and Review DTOs.
/// </summary>
public static class ReviewMapper
{
	/// <summary>
	/// Maps a Review entity to a ReviewDto.
	/// </summary>
	/// <param name="review">The review entity to map.</param>
	/// <returns>A ReviewDto representation of the review.</returns>
	public static ReviewDto ToDto(this Review review) => new(
		review.Id,
		review.MovieId,
		review.ReviewerName,
		review.Rating,
		review.Comment
	);

	/// <summary>
	/// Maps a Review entity to a ReviewWithMovieDto, including the associated movie title.
	/// </summary>
	/// <param name="review">The review entity to map.</param>
	/// <returns>A ReviewWithMovieDto representation of the review with movie information.</returns>
	/// <remarks>
	/// Requires the Movie navigation property to be loaded via Include in the query.
	/// If Movie is null, MovieTitle will be an empty string.
	/// </remarks>
	public static ReviewWithMovieDto ToReviewWithMovieDto(this Review review) => new(
		review.Id,
		review.MovieId,
		review.Movie?.Title ?? string.Empty,
		review.ReviewerName,
		review.Rating,
		review.Comment
	);

	/// <summary>
	/// Maps a CreateReviewDto to a new Review entity.
	/// </summary>
	/// <param name="dto">The DTO containing review creation data.</param>
	/// <returns>A new Review entity.</returns>
	public static Review ToEntity(this CreateReviewDto dto) => new()
	{
		MovieId = dto.MovieId,
		ReviewerName = dto.ReviewerName,
		Rating = dto.Rating,
		Comment = dto.Comment
	};

	/// <summary>
	/// Updates an existing Review entity with values from UpdateReviewDto.
	/// </summary>
	/// <param name="review">The existing review entity.</param>
	/// <param name="dto">The DTO containing updated review data.</param>
	/// <returns>A new Review entity with updated values.</returns>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="review"/> is <see langword="null"/>.</exception>
	public static Review UpdateEntity(this Review review, UpdateReviewDto dto)
	{
		ArgumentNullException.ThrowIfNull(review);

		return new Review
		{
			Id = review.Id,
			MovieId = review.MovieId,
			ReviewerName = dto.ReviewerName,
			Rating = dto.Rating,
			Comment = dto.Comment,
			Movie = review.Movie
		};
	}
}
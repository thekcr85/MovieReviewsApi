namespace MovieReviews.Domain.Entities;

/// <summary>
/// Represents a movie review entity in the domain model.
/// Contains reviewer feedback and rating for a specific movie.
/// </summary>
public sealed class Review
{
	/// <summary>
	/// Gets the unique identifier for the review.
	/// </summary>
	public int Id { get; init; }

	/// <summary>
	/// Gets the foreign key referencing the associated movie.
	/// </summary>
	public int MovieId { get; init; }

	/// <summary>
	/// Gets the name of the person who wrote the review.
	/// </summary>
	public string ReviewerName { get; init; } = string.Empty;

	/// <summary>
	/// Gets the rating given to the movie.
	/// </summary>
	/// <remarks>
	/// Rating scale is typically 1-10, where 1 is the lowest and 10 is the highest.
	/// </remarks>
	public int Rating { get; init; }

	/// <summary>
	/// Gets the textual comment provided by the reviewer.
	/// </summary>
	public string Comment { get; init; } = string.Empty;

	/// <summary>
	/// Gets the navigation property to the associated movie.
	/// </summary>
	/// <remarks>
	/// This is a navigation property for Entity Framework Core.
	/// Use <c>Include</c> in queries to eagerly load the movie data.
	/// </remarks>
	public Movie Movie { get; init; } = default!;
}
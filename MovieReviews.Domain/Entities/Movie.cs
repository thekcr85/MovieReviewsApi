namespace MovieReviews.Domain.Entities;

/// <summary>
/// Represents a movie entity in the domain model.
/// Contains movie information and related reviews.
/// </summary>
public sealed class Movie
{
	/// <summary>
	/// Gets the unique identifier for the movie.
	/// </summary>
	public int Id { get; init; }

	/// <summary>
	/// Gets the title of the movie.
	/// </summary>
	public string Title { get; init; } = string.Empty;

	/// <summary>
	/// Gets the name of the movie's director.
	/// </summary>
	public string Director { get; init; } = string.Empty;

	/// <summary>
	/// Gets the year the movie was released.
	/// </summary>
	public int ReleaseYear { get; init; }

	/// <summary>
	/// Gets the genre of the movie (e.g., Drama, Comedy, Action).
	/// </summary>
	public string Genre { get; init; } = string.Empty;

	/// <summary>
	/// Gets the collection of reviews associated with this movie.
	/// </summary>
	/// <remarks>
	/// This is a navigation property for Entity Framework Core.
	/// Use eager loading or explicit loading to populate this collection.
	/// </remarks>
	public ICollection<Review> Reviews { get; init; } = [];
}
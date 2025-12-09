using Microsoft.AspNetCore.Http.HttpResults;
using MovieReviews.Application.DTOs.Movies;
using MovieReviews.Domain.Interfaces;
using MovieReviews.Application.Mappers;
using MovieReviews.Application.Interfaces;

namespace MovieReviews.Api.Endpoints;

/// <summary>
/// Provides endpoint mappings for movie-related operations.
/// </summary>
public static class MovieEndpoints
{
	/// <summary>
	/// Maps movie endpoints (CRUD) under /movies.
	/// </summary>
	/// <remarks>Registers CRUD endpoints tagged "Movies".</remarks>
	/// <param name="routes">Endpoint route builder.</param>
	/// <returns>A route group with mapped movie endpoints.</returns>
	public static RouteGroupBuilder MapMovieEndpoints(this IEndpointRouteBuilder routes)
	{
		var group = routes.MapGroup("/movies")
			.WithTags("Movies");

		// CRUD Endpoints

		group.MapGet("/", GetAllMovies)
			.WithName("GetAllMovies")
			.WithSummary("Get all movies")
			.WithDescription("Retrieves a list of all movies with their average ratings from reviews")
			.Produces<IEnumerable<MovieDto>>(200)
			.ProducesProblem(500);

		group.MapGet("/{id:int}", GetMovieById)
			.WithName("GetMovieById")
			.WithSummary("Get a movie by ID")
			.WithDescription("Retrieves a single movie by its unique identifier with average rating")
			.Produces<MovieDto>(200)
			.Produces(404)
			.ProducesProblem(500);

		group.MapPost("/", CreateMovie)
			.WithName("CreateMovie")
			.WithSummary("Create a new movie")
			.WithDescription("Creates a new movie with the provided details")
			.Produces<MovieDto>(201)
			.ProducesValidationProblem(400)
			.ProducesProblem(500);

		group.MapPut("/{id:int}", UpdateMovie)
			.WithName("UpdateMovie")
			.WithSummary("Update an existing movie")
			.WithDescription("Updates all fields of an existing movie by its ID")
			.Produces<MovieDto>(200)
			.Produces(400)
			.Produces(404)
			.ProducesValidationProblem(400)
			.ProducesProblem(500);

		group.MapDelete("/{id:int}", DeleteMovie)
			.WithName("DeleteMovie")
			.WithSummary("Delete a movie")
			.WithDescription("Deletes a movie by its ID along with all associated reviews")
			.Produces(204)
			.Produces(404)
			.ProducesProblem(500);

		// Additional Movie Endpoints

		group.MapGet("/with-reviews", GetMoviesWithReviews)
			.WithName("GetMoviesWithReviews")
			.WithSummary("Get all movies with reviews")
			.WithDescription("Retrieves a list of all movies along with their associated reviews")
			.Produces<IEnumerable<MovieWithReviewsDto>>(200)
			.ProducesProblem(500);

		group.MapGet("/with-reviews/{id:int}", GetMovieWithReviewsById)
			.WithName("GetMovieWithReviewsById")
			.WithSummary("Get a movie with reviews by ID")
			.WithDescription("Retrieves a single movie along with its associated reviews by its unique identifier")
			.Produces<MovieWithReviewsDto>(200)
			.Produces(404)
			.ProducesProblem(500);

		// Business Logic Specific to Movies

		group.MapGet("/count-by-genre/{genre}", CountMoviesWithSpecificGenre)
			.WithName("CountMoviesWithSpecificGenre")
			.WithSummary("Count movies with specific genre")
			.WithDescription("Counts the number of movies that belong to the specified genre")
			.Produces<int>(200)
			.ProducesProblem(500);

		group.MapGet("/genres", GetAllGenres)
			.WithName("GetAllGenres")
			.WithSummary("Get all movie genres")
			.WithDescription("Retrieves a list of all distinct movie genres")
			.Produces<IEnumerable<string>>(200)
			.ProducesProblem(500);

		group.MapGet("/by-director/{director}", GetMoviesByDirector)
			.WithName("GetMoviesByDirector")
			.WithSummary("Get movies by director")
			.WithDescription("Retrieves a list of movies directed by the specified director")
			.Produces<IEnumerable<MovieDto>>(200)
			.ProducesProblem(500);

		group.MapGet("/by-release-year/{year:int}", GetMoviesByReleaseYear)
			.WithName("GetMoviesByReleaseYear")
			.WithSummary("Get movies by release year")
			.WithDescription("Retrieves a list of movies released in the specified year")
			.Produces<IEnumerable<MovieDto>>(200)
			.ProducesProblem(500);

		group.MapGet("/by-year-range/{startYear:int}/{endYear:int}", GetMoviesByYearRange)
			.WithName("GetMoviesByYearRange")
			.WithSummary("Get movies by year range")
			.WithDescription("Retrieves a list of movies released within the specified year range")
			.Produces<IEnumerable<MovieDto>>(200)
			.ProducesProblem(500);

		group.MapGet("/by-genre/{genre}", GetMoviesByGenre)
			.WithName("GetMoviesByGenre")
			.WithSummary("Get movies by genre")
			.WithDescription("Retrieves a list of movies with specified genre")
			.Produces<IEnumerable<MovieDto>>(200)
			.ProducesProblem(500);

		group.MapGet("/by-title/{keyword}", GetMoviesByTitleKeyword)
			.WithName("GetMoviesByTitleKeyword")
			.WithSummary("Search movies by title keyword")
			.WithDescription("Retrieves movies whose titles contain the specified keyword")
			.Produces<IEnumerable<MovieDto>>(200)
			.ProducesProblem(500);

		group.MapGet("/top-rated/{count:int}", GetTopRatedMovies)
			.WithName("GetTopRatedMovies")
			.WithSummary("Get top-rated movies")
			.WithDescription("Retrieves a list of top-rated movies limited by the specified count")
			.Produces<IEnumerable<MovieDto>>(200)
			.ProducesProblem(500);

		group.MapGet("/by-min-rating/{minRating:double}", GetMoviesByMinimumAverageRating)
			.WithName("GetMoviesByMinimumAverageRating")
			.WithSummary("Get movies by minimum average rating")
			.WithDescription("Retrieves a list of movies with an average rating greater than or equal to the specified minimum rating")
			.Produces<IEnumerable<MovieDto>>(200)
			.ProducesProblem(500);

		return group;
	}

	/// <summary>
	/// Gets all movies as DTOs.
	/// </summary>
	/// <param name="movieService">Movie service.</param>
	/// <param name="ct">Cancellation token.</param>
	/// <returns>200 OK with an enumerable of <see cref="MovieDto"/>.</returns>
	private static async Task<Ok<IEnumerable<MovieDto>>> GetAllMovies(
		IMovieService movieService,
		CancellationToken ct = default)
	{
		var moviesDto = await movieService.GetAllAsync(ct);
		return TypedResults.Ok<IEnumerable<MovieDto>>(moviesDto);
	}

	/// <summary>
	/// Gets a movie by id.
	/// </summary>
	/// <param name="id">Movie identifier.</param>
	/// <param name="movieService">Movie service.</param>
	/// <param name="ct">Cancellation token.</param>
	/// <returns>200 OK with <see cref="MovieDto"/> or 404 NotFound.</returns>
	private static async Task<Results<Ok<MovieDto>, ProblemHttpResult>> GetMovieById(
		int id,
		IMovieService movieService,
		CancellationToken ct = default)
	{
		ArgumentNullException.ThrowIfNull(movieService);

		var movieDto = await movieService.GetByIdAsync(id, ct);

		if (movieDto is null)
		{
			return TypedResults.Problem(
				statusCode: StatusCodes.Status404NotFound,
				title: "Movie not found",
				detail: $"Movie with id {id} does not exist.",
				instance: $"/movies/{id}"
				);
		}

		return TypedResults.Ok(movieDto);
	}

	/// <summary>
	/// Creates a new movie.
	/// </summary>
	/// <param name="dto">Create movie DTO.</param>
	/// <param name="movieService">Movie service.</param>
	/// <param name="ct">Cancellation token.</param>
	/// <returns>201 CreatedAtRoute with <see cref="MovieDto"/>.</returns>
	private static async Task<CreatedAtRoute<MovieDto>> CreateMovie(
		CreateMovieDto dto,
		IMovieService movieService,
		CancellationToken ct = default)
	{
		var movieDto = await movieService.CreateAsync(dto, ct);

		return TypedResults.CreatedAtRoute(
			routeName: "GetMovieById",          // nazwa endpointu GET
			routeValues: new { id = movieDto.Id }, // parametry routingu
			value: movieDto                        // zwracany DTO
		);
	}

	/// <summary>
	/// Updates an existing movie.
	/// </summary>
	/// <param name="id">Movie identifier.</param>
	/// <param name="updateMovieDto">Update movie DTO.</param>
	/// <param name="movieService">Movie service.</param>
	/// <param name="ct">Cancellation token.</param>
	/// <returns>200 OK with updated <see cref="MovieDto"/>, 400 BadRequest or 404 NotFound.</returns>
	private static async Task<Results<Ok<MovieDto>, BadRequest, NotFound>> UpdateMovie(
		int id,
		UpdateMovieDto updateMovieDto,
		IMovieService movieService,
		CancellationToken ct = default)
	{
		if (id != updateMovieDto.Id)
		{
			return TypedResults.BadRequest();
		}

		return await movieService.UpdateAsync(id, updateMovieDto, ct) is MovieDto updatedMovieDto
			? TypedResults.Ok(updatedMovieDto)
			: TypedResults.NotFound();
	}

	/// <summary>
	/// Deletes a movie by id.
	/// </summary>
	/// <param name="id">Movie identifier.</param>
	/// <param name="movieService">Movie service.</param>
	/// <param name="ct">Cancellation token.</param>
	/// <returns>204 NoContent or 404 NotFound.</returns>
	private static async Task<Results<NoContent, NotFound>> DeleteMovie(
		int id,
		IMovieService movieService,
		CancellationToken ct = default)
	{
		return await movieService.DeleteAsync(id, ct) ? TypedResults.NoContent() : TypedResults.NotFound();
	}

	/// <summary>
	/// Retrieves all movies with their associated reviews
	/// </summary>
	/// <param name="movieService">Movie service</param>
	/// <param name="ct">Cancellation token</param>
	/// <returns>200 OK with an enumerable of <see cref="MovieWithReviewsDto"/></returns>
	public static async Task<Ok<IEnumerable<MovieWithReviewsDto>>> GetMoviesWithReviews(
		IMovieService movieService,
		CancellationToken ct = default)
	{
		ArgumentNullException.ThrowIfNull(movieService);
		var moviesWithReviewsDto = await movieService.GetMoviesWithReviewsAsync(ct);
		return TypedResults.Ok<IEnumerable<MovieWithReviewsDto>>(moviesWithReviewsDto);
	}

	/// <summary>
	/// Gets a movie by ID and its associated reviews.
	/// </summary>
	/// <param name="id">Movie identifier.</param>
	/// <param name="movieService">Movie service.</param>
	/// <param name="ct">Cancellation token.</param>
	/// <returns>200 OK with <see cref="MovieWithReviewsDto"/> or 404 NotFound.</returns>
	public static async Task<Results<Ok<MovieWithReviewsDto>, NotFound>> GetMovieWithReviewsById(
		int id,
		IMovieService movieService,
		CancellationToken ct = default)
	{
		var movieWithReviewsDto = await movieService.GetMovieWithReviewsByIdAsync(id, ct);
		if (movieWithReviewsDto is null)
		{
			return TypedResults.NotFound();
		}
		return TypedResults.Ok(movieWithReviewsDto);
	}

	/// <summary>
	/// Asynchronously counts the number of movies that belong to the specified genre.
	/// </summary>
	/// <param name="genre">The genre to filter movies by. If null, empty, or consists only of whitespace, the count will be zero.</param>
	/// <param name="movieRepository">The repository used to access and count movies by genre. Cannot be null.</param>
	/// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
	/// <returns>An Ok result containing the number of movies in the specified genre. Returns zero if the genre is null, empty, or
	/// whitespace.</returns>
	private static async Task<Ok<int>> CountMoviesWithSpecificGenre(
		string genre,
		IMovieRepository movieRepository,
		CancellationToken ct = default)
	{
		ArgumentNullException.ThrowIfNull(movieRepository);

		if (string.IsNullOrWhiteSpace(genre))
		{
			return TypedResults.Ok(0);
		}

		var count = await movieRepository.CountByGenreAsync(genre, ct);
		return TypedResults.Ok(count);
	}



	/// <summary>
	/// Retrieves all movies directed by the specified director.
	/// </summary>
	/// <param name="director">The name of the director whose movies are to be retrieved. Cannot be null or empty.</param>
	/// <param name="movieRepository">The repository used to access movie data.</param>
	/// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains an Ok response with a collection of
	/// movie data transfer objects for the specified director. If no movies are found, the collection will be empty.</returns>
	public static async Task<Ok<IEnumerable<MovieDto>>> GetMoviesByDirector(
		string director,
		IMovieRepository movieRepository,
		CancellationToken ct = default)
	{
		ArgumentNullException.ThrowIfNull(movieRepository);
		var movies = await movieRepository.GetByDirectorAsync(director, ct);
		var movieDtos = movies.Select(m => m.ToDto()).ToList();
		return TypedResults.Ok<IEnumerable<MovieDto>>(movieDtos);
	}

	/// <summary>
	/// Retrieves a collection of movies that match the specified genre.
	/// </summary>
	/// <param name="genre">The genre to filter movies by. If null or empty, all movies may be returned depending on repository implementation.</param>
	/// <param name="movieRepository">The repository used to access movie data. Cannot be null.</param>
	/// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains an HTTP 200 OK response with a
	/// collection of movie data transfer objects that match the specified genre.</returns>
	public static async Task<Ok<IEnumerable<MovieDto>>> GetMoviesByGenre(
		string genre,
		IMovieRepository movieRepository,
		CancellationToken ct = default)
	{
		ArgumentNullException.ThrowIfNull(movieRepository);
		var movies = await movieRepository.GetByGenreAsync(genre, ct);
		var movieDtos = movies.Select(m => m.ToDto()).ToList();
		return TypedResults.Ok<IEnumerable<MovieDto>>(movieDtos);
	}

	/// <summary>
	/// Gets movies released in the specified year.
	/// </summary>
	/// <param name="year">Release year.</param>
	/// <param name="movieService">Movie repository.</param>
	/// <param name="ct">Cancellation token.</param>
	/// <returns>200 OK with an enumerable of <see cref="MovieDto"/>.</returns>
	public static async Task<Ok<IEnumerable<MovieDto>>> GetMoviesByReleaseYear(
		int year,
		IMovieService movieService,
		CancellationToken ct = default)
	{
		ArgumentNullException.ThrowIfNull(movieService);
		var movies = await movieService.GetByReleaseYearAsync(year, ct);
		return TypedResults.Ok<IEnumerable<MovieDto>>(movies);
	}

	/// <summary>
	/// Gets movies released within the specified year range.
	/// </summary>
	/// <param name="startYear">Start year (inclusive).</param>
	/// <param name="endYear">End year (inclusive).</param>
	/// <param name="movieRepository">Movie repository.</param>
	/// <param name="ct">Cancellation token.</param>
	/// <returns>200 OK with movies or 400 BadRequest.</returns>
	public static async Task<Results<Ok<IEnumerable<MovieDto>>, BadRequest>> GetMoviesByYearRange(
		int startYear,
		int endYear,
		IMovieRepository movieRepository,
		CancellationToken ct = default)
	{
		if (startYear > endYear)
		{
			return TypedResults.BadRequest();
		}
		var movies = await movieRepository.GetByYearRangeAsync(startYear, endYear, ct);
		var movieDtos = movies.Select(m => m.ToDto()).ToList();
		return TypedResults.Ok<IEnumerable<MovieDto>>(movieDtos);
	}


	/// <summary>
	/// Searches for movies whose titles contain the specified keyword and returns the matching results.
	/// </summary>
	/// <param name="keyword">The keyword to search for within movie titles. The search is typically case-insensitive and matches any part of the
	/// title.</param>
	/// <param name="movieService">The service used to perform the movie search operation. Cannot be null.</param>
	/// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
	/// <returns>An HTTP 200 OK result containing a collection of movies whose titles match the specified keyword. The collection
	/// will be empty if no matches are found.</returns>
	public static async Task<Ok<IEnumerable<MovieDto>>> GetMoviesByTitleKeyword(
		string keyword,
		IMovieService movieService,
		CancellationToken ct = default)
	{
		ArgumentNullException.ThrowIfNull(movieService);
		var movies = await movieService.SearchByTitleAsync(keyword, ct);
		return TypedResults.Ok<IEnumerable<MovieDto>>(movies);
	}

	/// <summary>
	/// Retrieves the top-rated movies, limited to the specified count, and returns them as a collection of data transfer
	/// objects.
	/// </summary>
	/// <param name="count">The maximum number of top-rated movies to retrieve. Must be greater than zero.</param>
	/// <param name="movieRepository">The repository used to access movie data. Cannot be null.</param>
	/// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
	/// <returns>A result containing a collection of <see cref="MovieDto"/> objects representing the top-rated movies. The
	/// collection will contain at most <paramref name="count"/> items.</returns>
	public static async Task<Ok<IEnumerable<MovieDto>>> GetTopRatedMovies(
		int count,
		IMovieRepository movieRepository,
		CancellationToken ct = default)
	{
		ArgumentNullException.ThrowIfNull(movieRepository);
		var movies = await movieRepository.GetTopRatedAsync(count, ct);
		var movieDtos = movies.Select(m => m.ToDto()).ToList();
		return TypedResults.Ok<IEnumerable<MovieDto>>(movieDtos);
	}

	/// <summary>
	/// Retrieves a collection of movies whose average rating is greater than or equal to the specified minimum value.
	/// </summary>
	/// <param name="minRating">The minimum average rating a movie must have to be included in the results. Must be between 0.0 and 10.0.</param>
	/// <param name="movieRepository">The repository used to access movie data. Cannot be null.</param>
	/// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
	/// <returns>A result containing an enumerable collection of movie data transfer objects that meet the minimum average rating
	/// criteria.</returns>
	public static async Task<Ok<IEnumerable<MovieDto>>> GetMoviesByMinimumAverageRating(
		double minRating,
		IMovieRepository movieRepository,
		CancellationToken ct = default)
	{
		ArgumentNullException.ThrowIfNull(movieRepository);
		var movies = await movieRepository.GetMoviesByMinRatingAsync(minRating, ct);
		var moviesDto = movies.Select(m => m.ToDto()).ToList();
		return TypedResults.Ok<IEnumerable<MovieDto>>(moviesDto);
	}

	/// <summary>
	/// Retrieves all distinct movie genres from the repository asynchronously.
	/// </summary>
	/// <param name="movieService">The movie repository used to access genre data. Cannot be null.</param>
	/// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
	/// <returns>An HTTP 200 OK result containing an enumerable collection of genre names. The collection will be empty if no genres
	/// are found.</returns>
	public static async Task<Ok<IEnumerable<string>>> GetAllGenres(
		IMovieService movieService,
		CancellationToken ct = default)
	{
		ArgumentNullException.ThrowIfNull(movieService);
		var genres = await movieService.GetAllGenresAsync(ct);
		return TypedResults.Ok<IEnumerable<string>>(genres);
	}

}

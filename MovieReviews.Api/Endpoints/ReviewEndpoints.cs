using Microsoft.AspNetCore.Http.HttpResults;
using MovieReviews.Application.DTOs.Reviews;
using MovieReviews.Application.Interfaces;
using MovieReviews.Application.Mappers;
using MovieReviews.Application.Services;
using MovieReviews.Domain.Interfaces;

namespace MovieReviews.Api.Endpoints;

/// <summary>
/// Provides extension methods for mapping review-related API endpoints.
/// </summary>
/// <remarks>Registers review endpoints under "/reviews" and tags them "Reviews".</remarks>
public static class ReviewEndpoints
{
	public static RouteGroupBuilder MapReviewEndpoints(this IEndpointRouteBuilder routes)
	{
		var group = routes.MapGroup("/reviews")
			.WithTags("Reviews");

		group.MapGet("/", GetAllReviews)
			.WithName("GetAllReviews")
			.WithSummary("Get all reviews")
			.WithDescription("Retrieves a list of all reviews")
			.Produces<IEnumerable<ReviewDto>>(200)
			.ProducesProblem(500);

		group.MapGet("/{id:int}", GetReviewById)
			.WithName("GetReviewById")
			.WithSummary("Get a review by ID")
			.WithDescription("Retrieves a single review by its unique identifier")
			.Produces<ReviewDto>(200)
			.Produces(404)
			.ProducesProblem(500);

		group.MapPost("/", CreateReview)
			.WithName("CreateReview")
			.WithSummary("Create a new review")
			.WithDescription("Creates a new review with the provided details")
			.Produces<ReviewDto>(201)
			.ProducesValidationProblem(400)
			.ProducesProblem(500);

		group.MapPut("/{id:int}", UpdateReview)
			.WithName("UpdateReview")
			.WithSummary("Update an existing review")
			.WithDescription("Updates all fields of an existing review by its ID")
			.Produces<ReviewDto>(200)
			.Produces(400)
			.Produces(404)
			.ProducesValidationProblem(400)
			.ProducesProblem(500);

		group.MapDelete("/{id:int}", DeleteReview)
			.WithName("DeleteReview")
			.WithSummary("Delete a review")
			.WithDescription("Deletes a review by its ID")
			.Produces(204)
			.Produces(404)
			.ProducesProblem(500);

		group.MapGet("/bymovie/{movieId:int}", GetReviewsByMovieId)
			.WithName("GetReviewsByMovieId")
			.WithSummary("Get reviews by movie ID")
			.WithDescription("Retrieves all reviews for a specific movie by its ID")
			.Produces<IEnumerable<ReviewDto>>(200)
			.ProducesProblem(500);

		group.MapGet("/bymovie/{movieId:int}/count", GetReviewCountByMovieId)
			.WithName("GetReviewCountByMovieId")
			.WithSummary("Get review count by movie ID")
			.WithDescription("Returns the total number of reviews for a specific movie")
			.Produces<int>(200)
			.ProducesProblem(500);

		return group;
	}

	/// <summary>
	/// Gets all reviews.
	/// </summary>
	/// <param name="reviewService">Review service.</param>
	/// <param name="ct">Cancellation token.</param>
	/// <returns>200 OK with an enumerable of <see cref="ReviewDto"/>.</returns>
	public static async Task<Ok<IEnumerable<ReviewDto>>> GetAllReviews(
		IReviewService reviewService,
		CancellationToken ct = default)
	{
		var reviews = await reviewService.GetAllAsync(ct);
		return TypedResults.Ok<IEnumerable<ReviewDto>>(reviews);
	}

	/// <summary>
	/// Gets a review by id.
	/// </summary>
	/// <param name="id">Review identifier.</param>
	/// <param name="reviewService">Review service.</param>
	/// <param name="ct">Cancellation token.</param>
	/// <returns>200 OK with <see cref="ReviewDto"/> or 404 NotFound.</returns>
	public static async Task<Results<Ok<ReviewDto>, NotFound>> GetReviewById(
		int id,
		IReviewService reviewService,
		CancellationToken ct = default)
	{
		var review = await reviewService.GetByIdAsync(id, ct);
		if (review is null)
		{
			return TypedResults.NotFound();
		}
		return TypedResults.Ok(review);
	}

	/// <summary>
	/// Creates a new review.
	/// </summary>
	/// <param name="createReviewDto">Create review DTO.</param>
	/// <param name="reviewService">Review service.</param>
	/// <param name="ct">Cancellation token.</param>
	/// <returns>201 Created with the created <see cref="ReviewDto"/>.</returns>
	public static async Task<CreatedAtRoute<ReviewDto>> CreateReview(
		CreateReviewDto createReviewDto,
		IReviewService reviewService,
		CancellationToken ct = default)
	{
		var reviewDto = await reviewService.CreateAsync(createReviewDto, ct);

		return TypedResults.CreatedAtRoute(
			reviewDto,
			"GetReviewById",
			new { id = reviewDto.Id }
			);
	}

	/// <summary>
	/// Updates an existing review.
	/// </summary>
	/// <param name="id">Review identifier.</param>
	/// <param name="updateReviewDto">Update review DTO.</param>
	/// <param name="reviewService">Review service.</param>
	/// <param name="ct">Cancellation token.</param>
	/// <returns>200 OK with updated <see cref="ReviewDto"/>, 400 BadRequest or 404 NotFound.</returns>
	public static async Task<Results<Ok<ReviewDto>, BadRequest, NotFound>> UpdateReview(
		int id,
		UpdateReviewDto updateReviewDto,
		IReviewService reviewService,
		CancellationToken ct = default)
	{
		if (id != updateReviewDto.Id)
		{
			return TypedResults.BadRequest();
		}
	
		return await reviewService.UpdateAsync(id, updateReviewDto, ct) is ReviewDto updatedReview
			? TypedResults.Ok(updatedReview)
			: TypedResults.NotFound();
	}

	/// <summary>
	/// Deletes a review by id.
	/// </summary>
	/// <param name="id">Review identifier.</param>
	/// <param name="reviewService">Review repository.</param>
	/// <param name="ct">Cancellation token.</param>
	/// <returns>204 NoContent or 404 NotFound.</returns>
	public static async Task<Results<NoContent, NotFound>> DeleteReview(
		int id,
		IReviewService reviewService,
		CancellationToken ct = default)
	{
		return await reviewService.DeleteAsync(id, ct)
			? TypedResults.NoContent()
			: TypedResults.NotFound();
	}

	/// <summary>
	/// Retrieves all reviews associated with the specified movie identifier.
	/// </summary>
	/// <param name="movieId">The unique identifier of the movie for which to retrieve reviews.</param>
	/// <param name="reviewService">Review service.</param>
	/// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains an HTTP 200 OK response with a
	/// collection of review data transfer objects for the specified movie. The collection will be empty if no reviews are
	/// found.</returns>
	public static async Task<Ok<IEnumerable<ReviewDto>>> GetReviewsByMovieId(
		int movieId,
		IReviewService reviewService,
		CancellationToken ct = default)
	{
		var reviews = await reviewService.GetAllAsync(ct);
		var filteredReviews = reviews.Where(r => r.MovieId == movieId);
		return TypedResults.Ok(filteredReviews);
	}

	/// <summary>
	/// Gets the total count of reviews for a specific movie.
	/// </summary>
	/// <param name="movieId">Movie identifier.</param>
	/// <param name="reviewService">Review repository.</param>
	/// <param name="ct">Cancellation token.</param>
	/// <returns>200 OK with the count of reviews.</returns>
	public static async Task<Ok<int>> GetReviewCountByMovieId(
		int movieId,
		IReviewService reviewService,
		CancellationToken ct = default)
	{
		var reviews = await reviewService.GetAllAsync(ct);
		var count = reviews.Count(r => r.MovieId == movieId);
		return TypedResults.Ok(count);
	}
}

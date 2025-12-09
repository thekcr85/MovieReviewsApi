using Microsoft.EntityFrameworkCore;
using MovieReviews.Application.Interfaces;
using MovieReviews.Application.Services;
using MovieReviews.Domain.Interfaces;
using MovieReviews.Infrastructure.Persistence;
using MovieReviews.Infrastructure.Repositories;
using Scalar.AspNetCore;
using System.Globalization;

namespace MovieReviews.Api.Extensions;

/// <summary>
/// Extension methods for configuring dependency injection services.
/// </summary>
public static class ServiceCollectionExtensions
{
	/// <summary>
	/// Registers infrastructure layer services including database context and repositories.
	/// </summary>
	/// <param name="services">The service collection.</param>
	/// <param name="configuration">Application configuration containing connection strings.</param>
	/// <returns>The configured service collection.</returns>
	public static IServiceCollection AddMovieReviewsInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContext<AppDbContext>(options =>
			options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

		services.AddScoped<IMovieRepository, MovieRepository>();
		services.AddScoped<IReviewRepository, ReviewRepository>();

		return services;
	}

	/// <summary>
	/// Registers application layer services including business logic services.
	/// </summary>
	/// <param name="services">The service collection.</param>
	/// <returns>The configured service collection.</returns>
	public static IServiceCollection AddMovieReviewsApplication(this IServiceCollection services)
	{
		services.AddScoped<IMovieService, MovieService>();
		services.AddScoped<IReviewService, ReviewService>();
		return services;
	}

	/// <summary>
	/// Registers API layer services including validation, problem details, and OpenAPI.
	/// </summary>
	/// <param name="services">The service collection.</param>
	/// <returns>The configured service collection.</returns>
	public static IServiceCollection AddMovieReviewsApi(this IServiceCollection services)
	{
		services.AddValidation();

		services.AddProblemDetails(options =>
		{
			options.CustomizeProblemDetails = (context) =>
			{
				if (context.ProblemDetails is HttpValidationProblemDetails validationProblem)
				{
					context.ProblemDetails.Detail =
						$"Error(s) occurred: {validationProblem.Errors.Values.Sum(x => x.Length)}";
				}

				context.ProblemDetails.Extensions.TryAdd("timestamp",
					DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture));
			};
		});

		services.AddOpenApi(options =>
		{
			options.AddDocumentTransformer((document, context, cancellationToken) =>
			{
				document.Info = new()
				{
					Title = "Movie Reviews API",
					Version = "v1",
					Description = "API for managing movies and their reviews"
				};
				return Task.CompletedTask;
			});
		});

		return services;
	}
}

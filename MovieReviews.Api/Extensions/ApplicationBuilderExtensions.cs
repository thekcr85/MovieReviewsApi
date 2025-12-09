using Microsoft.AspNetCore.Builder;
using Scalar.AspNetCore;
using MovieReviews.Api.Endpoints;

namespace MovieReviews.Api.Extensions;

/// <summary>
/// Extension methods for configuring the ASP.NET Core application pipeline.
/// </summary>
public static class ApplicationBuilderExtensions
{
	/// <summary>
	/// Configures OpenAPI and Scalar UI for API documentation in development environment.
	/// </summary>
	/// <param name="app">The web application instance.</param>
	/// <returns>The configured web application.</returns>
	public static WebApplication UseMovieReviewsOpenApi(this WebApplication app)
	{
		if (app.Environment.IsDevelopment())
		{
			app.MapOpenApi();
			app.MapScalarApiReference(options =>
			{
				options.WithTitle("Movie Reviews API");
				options.WithTheme(ScalarTheme.Purple);
			});
		}

		return app;
	}

	/// <summary>
	/// Maps all Movie Reviews API endpoints to the application.
	/// </summary>
	/// <param name="app">The web application instance.</param>
	/// <returns>The configured web application.</returns>
	public static WebApplication MapMovieReviewsEndpoints(this WebApplication app)
	{
		app.MapMovieEndpoints();
		app.MapReviewEndpoints();
		return app;
	}
}

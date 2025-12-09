using Microsoft.EntityFrameworkCore;
using MovieReviews.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddMovieReviewsInfrastructure(builder.Configuration);
builder.Services.AddMovieReviewsApplication();
builder.Services.AddMovieReviewsApi();

var app = builder.Build();

// Configure pipeline
app.UseMovieReviewsOpenApi();

// Map endpoints
app.MapMovieReviewsEndpoints();

app.Run();

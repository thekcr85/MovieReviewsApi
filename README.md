# ?? MovieReviewsApi

A simple movie reviews API built with .NET 10, showing how to organize code using **Clean Architecture**.

**Perfect for:** Portfolio projects, learning architecture, or as a template for similar APIs.

---

## ?? Quick Start

### What You Need
- .NET 10 SDK ([download](https://dotnet.microsoft.com/download/dotnet/10.0))

### Run It
```bash
git clone https://github.com/yourusername/MovieReviewsApi.git
cd MovieReviewsApi
dotnet build
dotnet run --project MovieReviews.Api
```

Open: `https://localhost:5001`

---

## ?? Project Layout

This project is split into **4 layers** so each layer has one job:

```
?? MovieReviewsApi
??? ?? MovieReviews.Domain/           ? Business stuff (no frameworks!)
?   ??? Entities/                     (Movie, Review classes)
?   ??? Interfaces/                   (IMovieRepository, IReviewRepository)
?
??? ?? MovieReviews.Application/      ? What to do with the business stuff
?   ??? Services/                     (MovieService, ReviewService)
?   ??? DTOs/                         (Data Transfer Objects for API)
?   ??? Mappers/                      (Convert between entities and DTOs)
?
??? ???  MovieReviews.Infrastructure/  ? Save/get data from database
?   ??? Persistence/                  (AppDbContext, configurations)
?   ??? Repositories/                 (MovieRepository, ReviewRepository)
?
??? ?? MovieReviews.Api/              ? HTTP endpoints
    ??? Program.cs                    (Setup and start the API)
    ??? Endpoints/                    (API routes)
    ??? Extensions/                   (Setup helpers)
```

---

## ?? Why This Layout?

Each layer has **one responsibility**:

| Layer | Responsibility | Example |
|-------|---|---|
| **Domain** | What are movies and reviews? | `Movie.cs`, `Review.cs` |
| **Application** | What business logic exists? | `MovieService.GetAllAsync()` |
| **Infrastructure** | How do we save data? | `MovieRepository.cs` with EF Core |
| **API** | How do users interact? | HTTP endpoints like `GET /movies` |

**Benefits:**
- ? Easy to test (swap out database, no framework worries)
- ? Easy to change (database change = only Infrastructure changes)
- ? Easy to understand (clear what each folder does)

---

## ?? Running Tests

```bash
# Run all tests
dotnet test

# Run only unit tests (fast)
dotnet test ./MovieReviewsApi.UnitTests/

# Run only repository tests (integration tests)
dotnet test ./MovieReviewsApi.IntegrationTests/
```

### What Gets Tested

- **Unit Tests** ? Services work correctly with fake data
- **Integration Tests** ? Repositories work with the database

---

## ?? Tech Stack

- **.NET 10** (latest runtime)
- **C# 14** (latest language)
- **Entity Framework Core** (database access)
- **xUnit** (tests)
- **Moq** (mocking for tests)

---

## ?? Code Examples

### Add a Movie
```csharp
var movieService = new MovieService(movieRepository);
var dto = new CreateMovieDto { Title = "Avatar", Director = "Cameron", ReleaseYear = 2009, Genre = "Sci-Fi" };
var result = await movieService.CreateAsync(dto);
```

### Get a Movie
```csharp
var movie = await movieService.GetByIdAsync(1);
```

### Search by Title
```csharp
var movies = await movieService.SearchByTitleAsync("Avatar");
```

---

## ?? Learning: How Layers Work Together

1. **User** calls `GET /movies/1` (API Layer)
2. **Endpoint** calls `movieService.GetByIdAsync(1)` (Application Layer)
3. **Service** calls `movieRepository.GetByIdAsync(1)` (Infrastructure Layer)
4. **Repository** queries the database for the Movie (Domain Layer)
5. **Response** is sent back as JSON

Each layer only knows about the layer below it. The database doesn't know about the API. The API doesn't have business logic mixed in. Clean!

---

## ? .NET 10 Features Used

- **Primary Constructors** ? `public class Service(IRepository repo) { }`
- **Nullable Reference Types** ? Know which values can be null
- **Implicit Usings** ? No `using System;` needed
- **Async/Await** ? Non-blocking operations

---

## ?? What to Look At First

**For Learning:**
1. `MovieReviews.Domain/Entities/Movie.cs` — Simple model
2. `MovieReviews.Application/Services/MovieService.cs` — Business logic
3. `MovieReviews.Infrastructure/Repositories/MovieRepository.cs` — Data access
4. `MovieReviews.Api/Program.cs` — How it all fits together

**For Testing:**
1. `MovieReviewsApi.UnitTests/Application/Services/` — Service tests
2. `MovieReviewsApi.IntegrationTests/Infrastructure/Repositories/` — Repository tests

---

## ?? How to Extend This

**Add Caching?** ? Modify `Infrastructure` layer only  
**Switch to PostgreSQL?** ? Change `Infrastructure` only  
**Add Authentication?** ? Add to `API` layer only  
**Add business rules?** ? Add to `Application` layer only  

This is the power of Clean Architecture!

---

## ?? References

- [What is Clean Architecture?](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- [xUnit Testing](https://xunit.net/)
- [.NET 10](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-10)

---

## ?? License

MIT — You can use this for anything.

---

**Built as a learning project for .NET developers** ??

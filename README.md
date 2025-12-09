# 🎬 MovieReviewsApi

A clean, modern REST API for managing movies and reviews, built with .NET 10 using Minimal APIs and Entity Framework Core following Clean Architecture principles.

## 🚀 Quick Start

### Prerequisites
- .NET 10 SDK
- SQL Server (local or remote)

### Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/thekcr85/MovieReviewsApi.git
   cd MovieReviewsApi
   ```

2. **Configure database connection**
   
   Update `MovieReviews.Api/appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=MovieReviewsDb;Trusted_Connection=true;Encrypt=false"
     }
   }
   ```

3. **Apply migrations**
   ```bash
   dotnet ef database update --project MovieReviews.Infrastructure --startup-project MovieReviews.Api
   ```

4. **Run the API**
   ```bash
   dotnet run --project MovieReviews.Api
   ```

5. **Access the API**
   - API: `https://localhost:5001/api`
   - OpenAPI Docs: `https://localhost:5001/openapi/v1.json`
   - Scalar UI: `https://localhost:5001/scalar/v1`

---

## 📚 API Endpoints

### Movies
- `GET /api/movies` - Get all movies
- `GET /api/movies/{id}` - Get movie by ID
- `GET /api/movies/with-reviews` - Get all movies with reviews
- `GET /api/movies/with-reviews/{id}` - Get movie with reviews by ID
- `GET /api/movies/by-director/{director}` - Get movies by director
- `GET /api/movies/by-genre/{genre}` - Get movies by genre
- `GET /api/movies/by-release-year/{year}` - Get movies by release year
- `GET /api/movies/by-year-range/{startYear}/{endYear}` - Get movies by year range
- `GET /api/movies/by-title/{keyword}` - Search movies by title
- `GET /api/movies/top-rated/{count}` - Get top-rated movies
- `GET /api/movies/by-min-rating/{minRating}` - Get movies by minimum rating
- `GET /api/movies/genres` - Get all genres
- `GET /api/movies/count-by-genre/{genre}` - Count movies by genre
- `POST /api/movies` - Create movie
- `PUT /api/movies/{id}` - Update movie
- `DELETE /api/movies/{id}` - Delete movie

### Reviews
- `GET /api/reviews` - Get all reviews
- `GET /api/reviews/{id}` - Get review by ID
- `GET /api/reviews/movie/{movieId}` - Get reviews for a movie
- `GET /api/reviews/movie/{movieId}/average` - Get average rating for a movie
- `GET /api/reviews/reviewer/{reviewerName}` - Get reviews by reviewer name
- `POST /api/reviews` - Create review
- `PUT /api/reviews/{id}` - Update review
- `DELETE /api/reviews/{id}` - Delete review

---

## 🏗️ Architecture

```
MovieReviewsApi/
├── MovieReviews.Domain/              # Entities & interfaces (no dependencies)
├── MovieReviews.Application/         # Services, DTOs, mappers
├── MovieReviews.Infrastructure/      # Database, repositories, EF Core
├── MovieReviews.Api/                 # Minimal APIs, composition root
├── MovieReviewsApi.UnitTests/        # Unit tests
└── MovieReviewsApi.IntegrationTests/ # Integration tests
```

**Clean Architecture Layers:**
- **Domain** — Business models, no external dependencies
- **Application** — Business logic, services, interfaces
- **Infrastructure** — Data access, repository implementations
- **API** — Minimal APIs endpoints, DI configuration

---

## 💡 Technology Stack

| Component | Technology |
|-----------|-----------|
| **Runtime** | .NET 10 |
| **Language** | C# 14 |
| **Web Framework** | ASP.NET Core Minimal APIs |
| **Database** | SQL Server + Entity Framework Core 10 |
| **Testing** | xUnit, Moq |
| **Documentation** | OpenAPI/Swagger, Scalar |

---

## 🛡️ Error Handling

The API implements global exception handling with **RFC 7807 ProblemDetails** standard responses.

### Error Response Format

```json
{
  "type": "https://httpstatuses.com/404",
  "title": "Not found",
  "status": 404,
  "detail": "Movie with id 999 does not exist.",
  "instance": "/api/movies/999"
}
```

### Status Codes

| Status | Exception | Scenario |
|--------|-----------|----------|
| 400 | `ArgumentException` | Invalid input parameters |
| 404 | `KeyNotFoundException` | Resource not found |
| 409 | `InvalidOperationException` | Invalid state |
| 500 | Other exceptions | Server error |

---

## 📝 Example Requests

### Create Movie
```bash
curl -X POST https://localhost:5001/api/movies \
  -H "Content-Type: application/json" \
  -d '{
    "title": "The Shawshank Redemption",
    "director": "Frank Darabont",
    "releaseYear": 1994,
    "genre": "Drama"
  }'
```

### Create Review
```bash
curl -X POST https://localhost:5001/api/reviews \
  -H "Content-Type: application/json" \
  -d '{
    "movieId": 1,
    "reviewerName": "John Doe",
    "rating": 9,
    "comment": "A masterpiece."
  }'
```

### Get Top-Rated Movies
```bash
curl https://localhost:5001/api/movies/top-rated/10
```

---

## 🧪 Testing

```bash
# Run all tests
dotnet test

# Run unit tests only
dotnet test ./MovieReviewsApi.UnitTests/

# Run integration tests only
dotnet test ./MovieReviewsApi.IntegrationTests/

# Run with code coverage
dotnet test /p:CollectCoverage=true
```

---

## 🔑 Key Features

✅ **Minimal APIs** — Lightweight, type-safe endpoint definitions  
✅ **Clean Architecture** — Clear separation of concerns  
✅ **SOLID Principles** — Single Responsibility, Open/Closed, etc.  
✅ **Async/Await** — Non-blocking operations with CancellationToken support  
✅ **Repository Pattern** — Data access abstraction  
✅ **Dependency Injection** — Built-in ASP.NET Core DI  
✅ **Comprehensive Testing** — Unit and integration tests  
✅ **Global Error Handling** — RFC 7807 ProblemDetails  
✅ **Auto Documentation** — OpenAPI/Swagger integration  

---

## 📦 Project Dependencies

- **Entity Framework Core 10.0** — ORM for SQL Server
- **xUnit 2.9.3** — Unit testing framework
- **Moq 4.20.72** — Mocking library
- **Coverlet 6.0.4** — Code coverage
- **Scalar.AspNetCore** — OpenAPI UI

---

## 📄 License
MIT License — See [LICENSE](LICENSE) file for details.

---

## 👨‍💻 Contributing
See [CONTRIBUTING.md](CONTRIBUTING.md) for guidelines.

---

## 🎓 About
Portfolio project demonstrating modern .NET development with Clean Architecture, Minimal APIs, and SOLID principles.

# 🎬 MovieReviewsApi

A clean, modern REST API for managing movies and reviews, built with .NET 10 and Entity Framework Core following Clean Architecture principles. This project was developed as a portfolio piece to showcase how I apply Clean Architecture and best practices in .NET.

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

2. **Configure the database connection**
   
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
   - OpenAPI: `https://localhost:5001/openapi/v1.json`

---

## 📚 API Endpoints

### Movies
- `GET /api/movies` - Get all movies
- `GET /api/movies/{id}` - Get movie by ID
- `GET /api/movies/search?title={title}` - Search movies by title
- `GET /api/movies/director/{director}` - Get movies by director
- `GET /api/movies/genre/{genre}` - Get movies by genre
- `GET /api/movies/year/{year}` - Get movies by release year
- `POST /api/movies` - Create movie
- `PUT /api/movies/{id}` - Update movie
- `DELETE /api/movies/{id}` - Delete movie

### Reviews
- `GET /api/reviews` - Get all reviews
- `GET /api/reviews/{id}` - Get review by ID
- `GET /api/reviews/movie/{movieId}` - Get reviews for a movie
- `GET /api/reviews/movie/{movieId}/average` - Get average rating for a movie
- `GET /api/reviews/reviewer/{reviewerName}` - Get reviews by reviewer
- `POST /api/reviews` - Create review
- `PUT /api/reviews/{id}` - Update review
- `DELETE /api/reviews/{id}` - Delete review

---

## 🏗️ Project Structure

```
MovieReviewsApi/
├── MovieReviews.Domain/              # Entities & business logic
├── MovieReviews.Application/         # DTOs, Services, Interfaces
├── MovieReviews.Infrastructure/      # Database, Repositories
├── MovieReviews.Api/                 # Endpoints, Configuration
├── MovieReviewsApi.UnitTests/        # Unit tests
└── MovieReviewsApi.IntegrationTests/ # Integration tests
```

### Clean Architecture
- **Domain** → No external dependencies, pure business logic
- **Application** → Depends on Domain only, services & mappers
- **Infrastructure** → Implements Application interfaces, database access
- **API** → Composition root (depends on all layers)

---

## 💡 Key Features

✅ **Clean Architecture** - Clear separation of concerns across 4 layers  
✅ **.NET 10 & C# 14** - Latest language features (primary constructors, nullable reference types)  
✅ **CancellationToken Support** - Proper async/await patterns  
✅ **Entity Framework Core 10** - SQL Server integration  
✅ **OpenAPI/Scalar** - Auto-generated API documentation  
✅ **Dependency Injection** - Built-in DI container  
✅ **Repository Pattern** - Data access abstraction  
✅ **SOLID Principles** - Single Responsibility, Open/Closed, Liskov Substitution, Interface Segregation, Dependency Inversion  
✅ **Comprehensive Testing** - Unit tests with mocks, integration tests with in-memory database

---

## 🛡️ Error Handling

The API implements global exception handling following the **RFC 7807 ProblemDetails** standard for consistent error responses.

### Error Response Format

All errors return a standardized `ProblemDetails` response:

```json
{
  "type": "https://httpstatuses.com/404",
  "title": "Not found",
  "status": 404,
  "detail": "Movie with ID 999 does not exist.",
  "instance": "/api/movies/999"
}
```

### HTTP Status Codes

| Status | Exception | Meaning |
|--------|-----------|---------|
| 400 | `ArgumentException` | Invalid request parameters |
| 404 | `KeyNotFoundException` | Resource not found |
| 409 | `InvalidOperationException` | Invalid operation state |
| 500 | Other exceptions | Internal server error |

### Development vs. Production

- **Development**: Error `Detail` includes the full exception message for debugging
- **Production**: Error `Detail` contains a generic message for security

---

## 📝 Example: Create a Movie

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

## 📝 Example: Create a Review

```bash
curl -X POST https://localhost:5001/api/reviews \
  -H "Content-Type: application/json" \
  -d '{
    "movieId": 1,
    "reviewerName": "John Doe",
    "rating": 9,
    "comment": "A masterpiece of cinema."
  }'
```

---

## 🧪 Running Tests

```bash
# Run all tests
dotnet test

# Run unit tests only
dotnet test ./MovieReviewsApi.UnitTests

# Run integration tests only
dotnet test ./MovieReviewsApi.IntegrationTests

# Run with code coverage
dotnet test /p:CollectCoverage=true
```

---

## 📦 Technologies

- **Runtime**: .NET 10
- **Language**: C# 14
- **Database**: SQL Server + Entity Framework Core 10
- **API**: ASP.NET Core Web API
- **Testing**: xUnit, Moq
- **Documentation**: OpenAPI/Scalar

---

## 🎓 Learning & Architecture

### Why Clean Architecture?
- **Testability** — Core business logic is framework-agnostic
- **Maintainability** — Clear separation makes changes localized
- **Scalability** — Easy to add features or swap implementations
- **Flexibility** — Switch databases or frameworks without affecting business logic

### How Layers Work Together

1. **User** calls `GET /api/movies/1` (API Layer)
2. **Endpoint** calls `movieService.GetByIdAsync(1)` (Application Layer)
3. **Service** calls `movieRepository.GetByIdAsync(1)` (Infrastructure Layer)
4. **Repository** queries the database for the Movie (Domain Layer)
5. **Response** is sent back as JSON

Each layer only knows about the layer below it. The database doesn't know about the API. The API doesn't have business logic mixed in.

### What to Look At First

**For Learning:**
1. `MovieReviews.Domain/Entities/Movie.cs` — Simple domain model
2. `MovieReviews.Application/Services/MovieService.cs` — Business logic
3. `MovieReviews.Infrastructure/Repositories/MovieRepository.cs` — Data access
4. `MovieReviews.Api/Program.cs` — How it all fits together

**For Testing:**
1. `MovieReviewsApi.UnitTests/Application/Services/` — Service unit tests
2. `MovieReviewsApi.IntegrationTests/Infrastructure/Repositories/` — Repository tests

---

## 🔗 Resources

- [.NET 10 Documentation](https://learn.microsoft.com/en-us/dotnet/)
- [Clean Architecture by Robert C. Martin](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- [ASP.NET Core Web API](https://learn.microsoft.com/en-us/aspnet/core/web-api/)
- [xUnit Testing Framework](https://xunit.net/)

---

## 📄 License
This project is open source and available under the MIT License.

---

## 👨‍💻 Contributing
Feel free to fork and submit pull requests! For guidelines, see [CONTRIBUTING.md](CONTRIBUTING.md)

---

## 🎓 About
This is a portfolio project created to practice modern .NET development patterns and demonstrate skills in building maintainable, testable APIs using Clean Architecture. The goal is to showcase understanding of architectural patterns, SOLID principles, and best practices in .NET development.

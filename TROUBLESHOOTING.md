# ?? Troubleshooting Guide

Common issues and how to fix them.

## Build Issues

### "Project dependency not found"
```bash
dotnet clean
dotnet restore
dotnet build
```

### "'DbContext' could not be found"
Make sure `MovieReviews.Infrastructure` is referenced in your project.

## Runtime Issues

### "No database provider configured"
Check `appsettings.json` has a valid `DefaultConnection` connection string.

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=MovieReviewsDb;Trusted_Connection=true;"
  }
}
```

### "Port 5001 already in use"
Use a different port:
```bash
dotnet run --project MovieReviews.Api -- --urls "https://localhost:5002"
```

## Test Issues

### "Tests can't find repositories"
Make sure test projects reference:
- `MovieReviews.Domain`
- `MovieReviews.Application`
- `MovieReviews.Infrastructure`

### "In-memory database isn't working"
Add NuGet package: `Microsoft.EntityFrameworkCore.InMemory`

## Understanding the Code

### "Where do I add a new feature?"
1. **Add a method to Repository** (`Infrastructure/Repositories/`)
2. **Call it from Service** (`Application/Services/`)
3. **Create an Endpoint** (`Api/Endpoints/`)

### "Why do we have DTOs?"
DTOs (Data Transfer Objects) separate what the API returns from how the database stores data. This gives you flexibility to change one without breaking the other.

### "What's the difference between Entity and DTO?"
- **Entity** — How data is stored (Database model)
- **DTO** — What API returns (API contract)

Example: `Movie` entity might have 100 fields, but API only returns 5 important ones in `MovieDto`.

### "Why do we need mappers?"
Mappers convert between entities and DTOs. This keeps your domain clean and your API contract stable.

## Common Mistakes

### ? Don't put business logic in endpoints
```csharp
// Wrong
app.MapGet("/movies/{id}", async (int id, AppDbContext db) =>
{
    var movie = await db.Movies.FirstOrDefaultAsync(m => m.Id == id);
    // Business logic here... NO!
    return movie;
});
```

```csharp
// Right
app.MapGet("/movies/{id}", async (int id, IMovieService service) =>
{
    return await service.GetByIdAsync(id);
});
```

### ? Don't reference Infrastructure in Domain
```csharp
// Wrong - Domain shouldn't know about EF Core
public class Movie
{
    [Key]  // ? This is EF Core! Don't use in Domain
    public int Id { get; set; }
}
```

```csharp
// Right - Domain is pure
public class Movie
{
    public int Id { get; set; }
}
```

### ? Don't forget to register services in Program.cs
All services must be added:
```csharp
builder.Services.AddMovieReviewsInfrastructure(builder.Configuration);
builder.Services.AddMovieReviewsApplication();
```

## Getting Help

1. **Check the README** — Most questions are answered there
2. **Read SOLUTION_STRUCTURE.md** — Explains the layout
3. **Look at tests** — Tests show how to use the code
4. **Open an issue** — Ask questions on GitHub

## Tips for Success

? Keep domain logic pure (no frameworks)  
? Use dependency injection for everything  
? Test your services with fake repositories  
? Keep your endpoints thin (call services)  
? Use meaningful names  
? Comment complex logic  
? Follow the existing code style  

Happy coding! ??

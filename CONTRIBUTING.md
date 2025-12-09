# Contributing to Movie Reviews API

Thank you for your interest in contributing to the Movie Reviews API project! This document provides guidelines and instructions for contributing.

## ?? How to Contribute

### Reporting Issues

If you find a bug or have a suggestion for improvement:

1. Check if the issue already exists in the [Issues](https://github.com/yourusername/MovieReviewsApi/issues) section
2. If not, create a new issue with a clear title and description
3. Include steps to reproduce (for bugs) or detailed explanation (for features)
4. Add relevant labels (bug, enhancement, documentation, etc.)

### Submitting Changes

1. **Fork the Repository**
   ```bash
   git clone https://github.com/yourusername/MovieReviewsApi.git
   cd MovieReviewsApi
   ```

2. **Create a Feature Branch**
   ```bash
   git checkout -b feature/your-feature-name
   ```
   
   Branch naming conventions:
   - `feature/` - New features
   - `bugfix/` - Bug fixes
   - `docs/` - Documentation updates
   - `refactor/` - Code refactoring
   - `test/` - Adding or updating tests

3. **Make Your Changes**
   - Follow the coding standards outlined below
   - Write clear, descriptive commit messages
   - Add or update tests as needed
   - Update documentation if required

4. **Test Your Changes**
   ```bash
   dotnet build
   dotnet test
   ```

5. **Commit Your Changes**
   ```bash
   git add .
   git commit -m "feat: add new feature description"
   ```
   
   Commit message format:
   - `feat:` - New feature
   - `fix:` - Bug fix
   - `docs:` - Documentation changes
   - `style:` - Formatting, missing semicolons, etc.
   - `refactor:` - Code restructuring
   - `test:` - Adding tests
   - `chore:` - Updating build tasks, package manager configs, etc.

6. **Push to Your Fork**
   ```bash
   git push origin feature/your-feature-name
   ```

7. **Create a Pull Request**
   - Go to the original repository on GitHub
   - Click "New Pull Request"
   - Select your fork and branch
   - Provide a clear description of your changes
   - Reference any related issues

## ?? Coding Standards

### General Guidelines

- Follow C# coding conventions and best practices
- Use meaningful variable and method names
- Keep methods small and focused (single responsibility)
- Write self-documenting code with clear intent
- Add XML documentation comments to public APIs
- Use nullable reference types appropriately

### Code Style

```csharp
// Good: Clear, concise, and well-documented
/// <summary>
/// Retrieves a movie by its unique identifier.
/// </summary>
/// <param name="id">The movie identifier.</param>
/// <param name="ct">Cancellation token.</param>
/// <returns>A movie DTO or null if not found.</returns>
public async Task<MovieDto?> GetByIdAsync(int id, CancellationToken ct = default)
{
    var movie = await _repository.GetByIdAsync(id, ct);
    return movie?.ToDto();
}
```

### Architecture Principles

1. **Maintain Clean Architecture**
   - Domain layer has no dependencies
   - Application layer depends only on Domain
   - Infrastructure depends on Domain and Application
   - API depends on all layers but contains only presentation logic

2. **Follow SOLID Principles**
   - Single Responsibility Principle
   - Open/Closed Principle
   - Liskov Substitution Principle
   - Interface Segregation Principle
   - Dependency Inversion Principle

3. **Use Dependency Injection**
   - Register services in appropriate extension methods
   - Use constructor injection
   - Prefer interfaces over concrete implementations

### Testing Standards

- Write unit tests for business logic
- Write integration tests for repositories
- Aim for high code coverage (>80%)
- Use meaningful test names that describe what is being tested
- Follow AAA pattern (Arrange, Act, Assert)

```csharp
[Fact]
public async Task GetByIdAsync_WithValidId_ReturnsMovie()
{
    // Arrange
    var movieId = 1;
    
    // Act
    var result = await _service.GetByIdAsync(movieId);
    
    // Assert
    Assert.NotNull(result);
    Assert.Equal(movieId, result.Id);
}
```

### Documentation Standards

- Add XML documentation to all public types and members
- Keep documentation concise and clear
- Document parameters, return values, and exceptions
- Update README.md when adding new features

## ?? Code Review Process

All submissions require review before merging:

1. Automated checks must pass (build, tests)
2. At least one maintainer must approve
3. All comments and suggestions must be addressed
4. Code must follow the established patterns and standards

## ?? Pull Request Checklist

Before submitting your PR, ensure:

- [ ] Code builds without errors
- [ ] All tests pass
- [ ] New tests added for new functionality
- [ ] XML documentation added/updated
- [ ] README updated if needed
- [ ] No unnecessary changes or files
- [ ] Commit messages follow convention
- [ ] Branch is up to date with main

## ?? Development Setup

### Required Tools

- .NET 10 SDK
- Visual Studio 2025 or VS Code with C# extension
- SQL Server (LocalDB, Express, or full version)
- Git

### Setup Steps

1. Clone the repository
2. Install dependencies: `dotnet restore`
3. Update database: `dotnet ef database update --project MovieReviews.Infrastructure --startup-project MovieReviews.Api`
4. Run the application: `dotnet run --project MovieReviews.Api`

### Running Tests

```bash
# Run all tests
dotnet test

# Run with coverage
dotnet test /p:CollectCoverage=true
```

## ?? Resources

- [.NET Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [C# Documentation](https://docs.microsoft.com/en-us/dotnet/csharp/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)

## ?? Questions?

If you have questions about contributing:

1. Check existing issues and discussions
2. Create a new discussion in the [Discussions](https://github.com/yourusername/MovieReviewsApi/discussions) section
3. Reach out to maintainers

## ?? License

By contributing, you agree that your contributions will be licensed under the MIT License.

---

Thank you for contributing to Movie Reviews API! ???

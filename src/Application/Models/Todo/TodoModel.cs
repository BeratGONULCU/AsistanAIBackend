namespace GeminiAsistanBackend.Application.Models.Todo;

public sealed record TodoModel(
    Guid Id,
    string Title,
    string Description,
    bool IsCompleted,
    DateOnly? DueDate,
    DateTime CreatedOnUtc,
    DateTime LastUpdatedOnUtc);

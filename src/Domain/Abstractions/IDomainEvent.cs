namespace GeminiAsistanBackend.Domain.Abstractions;

/// <summary>
/// Marker interface for domain events raised by entities.
/// </summary>
public interface IDomainEvent
{
    DateTime OccurredOnUtc { get; }
}

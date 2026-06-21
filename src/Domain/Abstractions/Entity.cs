using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GeminiAsistanBackend.Domain.Abstractions;

/// <summary>
/// Base entity that ensures identity equality and tracks domain events.
/// </summary>
public abstract class Entity
{
    private readonly List<IDomainEvent> _domainEvents = new();

    protected Entity()
    {
        Id = Guid.NewGuid();
    }

    public Guid Id { get; protected init; }

    public IReadOnlyCollection<IDomainEvent> DomainEvents => new ReadOnlyCollection<IDomainEvent>(_domainEvents);

    protected void Raise(IDomainEvent domainEvent)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents() => _domainEvents.Clear();

    public override bool Equals(object? obj)
    {
        if (obj is not Entity other)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Id == other.Id;
    }

    public override int GetHashCode() => Id.GetHashCode();
}

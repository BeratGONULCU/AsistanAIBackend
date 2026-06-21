using System.Collections.Generic;
using System.Linq;

namespace GeminiAsistanBackend.Domain.Abstractions;

/// <summary>
/// Base type for immutable value objects.
/// </summary>
public abstract class ValueObject : IEquatable<ValueObject>
{
    public abstract IEnumerable<object?> GetAtomicValues();

    public override bool Equals(object? obj) => obj is ValueObject other && Equals(other);

    public bool Equals(ValueObject? other)
    {
        if (other is null || other.GetType() != GetType())
        {
            return false;
        }

        return GetAtomicValues().SequenceEqual(other.GetAtomicValues());
    }

    public override int GetHashCode() =>
        GetAtomicValues().Aggregate(0, (hash, value) => HashCode.Combine(hash, value));

    public static bool operator ==(ValueObject? left, ValueObject? right) => Equals(left, right);

    public static bool operator !=(ValueObject? left, ValueObject? right) => !Equals(left, right);
}

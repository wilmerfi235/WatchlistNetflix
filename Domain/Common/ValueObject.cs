using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Common;

/// <summary>
/// Clase base para los objetos de valor del dominio.
///
/// Dos Value Objects son iguales cuando todos sus
/// componentes tienen los mismos valores.
/// </summary>
public abstract class ValueObject
{
    protected abstract IEnumerable<object?> GetEqualityComponents();

    public override bool Equals(object? obj)
    {
        if (obj is not ValueObject other)
            return false;

        return GetEqualityComponents()
            .SequenceEqual(other.GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Aggregate(
                0,
                (current, component) =>
                    HashCode.Combine(current, component));
    }
}

using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Catalog.ValueObjects;

/// <summary>
/// Identificador único de un género.
/// </summary>
public sealed class GenreId : ValueObject
{
    public Guid Value { get; }

    private GenreId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException(
                "El identificador del género no puede estar vacío.");

        Value = value;
    }

    public static GenreId New() => new(Guid.NewGuid());

    public static GenreId From(Guid value) => new(value);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}

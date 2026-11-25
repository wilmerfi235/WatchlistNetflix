using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Catalog.ValueObjects;

/// <summary>
/// Identificador único de un contenido del catálogo.
/// </summary>
public sealed class ContentId : ValueObject
{
    public Guid Value { get; }

    private ContentId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException(
                "El identificador del contenido no puede estar vacío.");

        Value = value;
    }

    public static ContentId New() => new(Guid.NewGuid());

    public static ContentId From(Guid value) => new(value);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}

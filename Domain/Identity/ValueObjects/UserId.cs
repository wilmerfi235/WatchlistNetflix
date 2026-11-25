using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Identity.ValueObjects;

/// <summary>
/// Identificador único de un usuario.
/// Evita utilizar Guid directamente en todo el dominio.
/// </summary>
public sealed class UserId : ValueObject
{
    public Guid Value { get; }

    private UserId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException(
                "El identificador del usuario no puede estar vacío.");

        Value = value;
    }

    public static UserId New()
        => new(Guid.NewGuid());

    public static UserId From(Guid value)
        => new(value);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}

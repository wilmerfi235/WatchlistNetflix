using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Watchlist.ValueObjects;

/// <summary>
/// Identificador único de un elemento perteneciente
/// a una lista personal.
/// </summary>
public sealed class WatchlistItemId : ValueObject
{
    public Guid Value { get; }

    private WatchlistItemId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException(
                "El identificador del elemento no puede estar vacío.");

        Value = value;
    }

    public static WatchlistItemId New()
        => new(Guid.NewGuid());

    public static WatchlistItemId From(Guid value)
        => new(value);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}

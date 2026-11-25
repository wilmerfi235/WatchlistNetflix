using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Watchlist.ValueObjects;

/// <summary>
/// Identificador único de una lista personal.
/// </summary>
public sealed class WatchlistId : ValueObject
{
    public Guid Value { get; }

    private WatchlistId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException(
                "El identificador de la lista no puede estar vacío.");

        Value = value;
    }

    public static WatchlistId New() => new(Guid.NewGuid());

    public static WatchlistId From(Guid value) => new(value);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Watchlist.Exceptions;

/// <summary>
/// Se produce cuando se intenta añadir un contenido
/// que ya existe en una lista personal.
/// </summary>
public sealed class DuplicateWatchlistItemException : Exception
{
    public DuplicateWatchlistItemException()
        : base("Este contenido ya existe en la lista.")
    {
    }
}

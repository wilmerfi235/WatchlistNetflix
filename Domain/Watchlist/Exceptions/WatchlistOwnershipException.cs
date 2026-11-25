using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Watchlist.Exceptions;

/// <summary>
/// Se produce cuando un usuario intenta realizar
/// una operación sobre una lista que no le pertenece.
/// </summary>
public sealed class WatchlistOwnershipException : Exception
{
    public WatchlistOwnershipException()
        : base("El usuario no tiene permisos para modificar esta lista.")
    {
    }
}

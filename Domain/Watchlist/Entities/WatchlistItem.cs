using Domain.Catalog.ValueObjects;
using Domain.Common;
using Domain.Watchlist.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Watchlist.Entities;

/// <summary>
/// Representa la asociación entre una lista personal
/// y un contenido del catálogo.
///
/// No contiene la entidad Content, sino únicamente
/// su identificador.
/// </summary>
public sealed class WatchlistItem : Entity<WatchlistItemId>
{
    public ContentId ContentId { get; private set; } = default!;

    public DateTime AddedAtUtc { get; private set; }

    private WatchlistItem()
    {
        // Constructor requerido por EF Core.
    }

    private WatchlistItem(
        WatchlistItemId id,
        ContentId contentId)
        : base(id)
    {
        ContentId = contentId;
        AddedAtUtc = DateTime.UtcNow;
    }

    /// <summary>
    /// Crea un nuevo elemento de una lista.
    /// Su creación debe estar controlada por Watchlist.
    /// </summary>
    internal static WatchlistItem Create(ContentId contentId)
    {
        ArgumentNullException.ThrowIfNull(contentId);

        return new WatchlistItem(
            WatchlistItemId.New(),
            contentId);
    }
}

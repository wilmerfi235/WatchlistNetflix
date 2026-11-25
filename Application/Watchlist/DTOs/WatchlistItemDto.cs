using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Watchlist.DTOs;

/// <summary>
/// Representa un contenido incluido en una lista de seguimiento.
/// </summary>
public sealed record WatchlistItemDto(
    Guid ContentId,
    string Title,
    string ContentType,
    string? ImageUrl,
    DateTime AddedAtUtc);

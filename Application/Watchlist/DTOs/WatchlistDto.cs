using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Watchlist.DTOs;

/// <summary>
/// Representa una lista de seguimiento perteneciente a un usuario.
/// </summary>
public sealed record WatchlistDto(
    Guid Id,
    string Name,
    string? Description,
    DateTime CreatedAtUtc,
    IReadOnlyCollection<WatchlistItemDto> Items);

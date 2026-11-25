using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Watchlist.Commands.CreateWatchlist;

/// <summary>
/// Representa la solicitud para crear una nueva lista de seguimiento.
/// </summary>
public sealed record CreateWatchlistCommand(
    string Name,
    string? Description
) : IRequest<Guid>;

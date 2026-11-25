using Application.Watchlist.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Watchlist.Queries.GetWatchlistById;

/// <summary>
/// Solicita una lista de seguimiento mediante su identificador.
/// </summary>
public sealed record GetWatchlistByIdQuery(
    Guid WatchlistId
) : IRequest<WatchlistDto>;

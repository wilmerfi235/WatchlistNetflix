using Application.Watchlist.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Watchlist.Queries.GetWatchlists;


/// <summary>
/// Solicita todas las listas de seguimiento del usuario autenticado.
/// </summary>
public sealed record GetWatchlistsQuery
    : IRequest<IReadOnlyCollection<WatchlistDto>>;

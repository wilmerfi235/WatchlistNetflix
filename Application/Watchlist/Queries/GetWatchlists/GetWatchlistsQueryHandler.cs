using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Watchlist.DTOs;
using Application.Watchlist.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Watchlist.Queries.GetWatchlists;

/// <summary>
/// Ejecuta la consulta de listas pertenecientes al usuario autenticado.
/// </summary>
public sealed class GetWatchlistsQueryHandler
    : IRequestHandler<GetWatchlistsQuery, IReadOnlyCollection<WatchlistDto>>
{
    private readonly IWatchlistRepository _watchlistRepository;
    private readonly ICurrentUserService _currentUserService;

    /// <summary>
    /// Inicializa el handler con las dependencias necesarias.
    /// </summary>
    public GetWatchlistsQueryHandler(
        IWatchlistRepository watchlistRepository,
        ICurrentUserService currentUserService)
    {
        _watchlistRepository = watchlistRepository;
        _currentUserService = currentUserService;
    }

    /// <summary>
    /// Recupera todas las listas asociadas al usuario actual.
    /// </summary>
    public async Task<IReadOnlyCollection<WatchlistDto>> Handle(
        GetWatchlistsQuery request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId
            ?? throw new UnauthorizedException(
                "Es necesario iniciar sesión para consultar las listas.");

        return await _watchlistRepository.GetAllByUserIdAsync(
            userId,
            cancellationToken);
    }
}

using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Watchlist.DTOs;
using Application.Watchlist.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Watchlist.Queries.GetWatchlistById;

/// <summary>
/// Ejecuta la consulta de una lista perteneciente al usuario actual.
/// </summary>
public sealed class GetWatchlistByIdQueryHandler
    : IRequestHandler<GetWatchlistByIdQuery, WatchlistDto>
{
    private readonly IWatchlistRepository _watchlistRepository;
    private readonly ICurrentUserService _currentUserService;

    /// <summary>
    /// Inicializa el handler con las dependencias necesarias.
    /// </summary>
    public GetWatchlistByIdQueryHandler(
        IWatchlistRepository watchlistRepository,
        ICurrentUserService currentUserService)
    {
        _watchlistRepository = watchlistRepository;
        _currentUserService = currentUserService;
    }

    /// <summary>
    /// Busca la lista solicitada y comprueba que pertenece
    /// al usuario autenticado.
    /// </summary>
    public async Task<WatchlistDto> Handle(
        GetWatchlistByIdQuery request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId
            ?? throw new UnauthorizedException(
                "Es necesario iniciar sesión para consultar una lista.");

        var watchlist = await _watchlistRepository.GetByIdAndUserIdAsync(
            request.WatchlistId,
            userId,
            cancellationToken);

        if (watchlist is null)
        {
            throw new NotFoundException(
                "No se encontró la lista solicitada.");
        }

        return watchlist;
    }
}

using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Watchlist.Interfaces;
using Domain.Catalog.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Watchlist.Commands.RemoveContent;

/// <summary>
/// Ejecuta la eliminación de un contenido de una lista.
/// </summary>
public sealed class RemoveContentCommandHandler
    : IRequestHandler<RemoveContentCommand, Unit>
{
    private readonly IWatchlistRepository _watchlistRepository;
    private readonly ICurrentUserService _currentUserService;

    /// <summary>
    /// Inicializa el handler con las dependencias necesarias.
    /// </summary>
    public RemoveContentCommandHandler(
        IWatchlistRepository watchlistRepository,
        ICurrentUserService currentUserService)
    {
        _watchlistRepository = watchlistRepository;
        _currentUserService = currentUserService;
    }

    /// <summary>
    /// Verifica la propiedad de la lista y solicita al dominio
    /// eliminar el contenido correspondiente.
    /// </summary>
    public async Task<Unit> Handle(
        RemoveContentCommand request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId
            ?? throw new UnauthorizedException(
                "Es necesario iniciar sesión para modificar una lista.");

        var watchlist = await _watchlistRepository.GetByIdAsync(
            request.WatchlistId,
            cancellationToken);

        if (watchlist is null ||
            !await _watchlistRepository.ExistsForUserAsync(
                request.WatchlistId,
                userId,
                cancellationToken))
        {
            throw new NotFoundException(
                "No se encontró la lista solicitada.");
        }

        var contentId = ContentId.From(request.ContentId);
        watchlist.RemoveContent(contentId);

        await _watchlistRepository.UpdateAsync(
            watchlist,
            cancellationToken);

        return Unit.Value;
    }
}

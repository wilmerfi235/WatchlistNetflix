using Application.Catalog.Interfaces;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Watchlist.Interfaces;
using Domain.Catalog.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Watchlist.Commands.AddContent;

/// <summary>
/// Ejecuta la incorporación de un contenido a una lista.
/// </summary>
public sealed class AddContentCommandHandler
    : IRequestHandler<AddContentCommand, Unit>
{
    private readonly IWatchlistRepository _watchlistRepository;
    private readonly IContentRepository _contentRepository;
    private readonly ICurrentUserService _currentUserService;

    /// <summary>
    /// Inicializa el handler con los repositorios y el servicio de usuario.
    /// </summary>
    public AddContentCommandHandler(
        IWatchlistRepository watchlistRepository,
        IContentRepository contentRepository,
        ICurrentUserService currentUserService)
    {
        _watchlistRepository = watchlistRepository;
        _contentRepository = contentRepository;
        _currentUserService = currentUserService;
    }

    /// <summary>
    /// Comprueba los recursos, verifica la propiedad de la lista
    /// y solicita al dominio que incorpore el contenido.
    /// </summary>
    public async Task<Unit> Handle(
        AddContentCommand request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId
            ?? throw new UnauthorizedException(
                "Es necesario iniciar sesión para modificar una lista.");

        var belongsToUser =
            await _watchlistRepository.ExistsForUserAsync(
                request.WatchlistId,
                userId,
                cancellationToken);

        if (!belongsToUser)
        {
            throw new NotFoundException(
                "No se encontró la lista solicitada.");
        }

        var content = await _contentRepository.GetByIdAsync(
            request.ContentId,
            cancellationToken);

        if (content is null)
        {
            throw new NotFoundException(
                "No se encontró el contenido solicitado.");
        }

        var watchlist = await _watchlistRepository.GetByIdAsync(
            request.WatchlistId,
            cancellationToken);

        if (watchlist is null)
        {
            throw new NotFoundException(
                "No se encontró la lista solicitada.");
        }

        var contentId = ContentId.From(request.ContentId);
        // La validación de duplicados y las reglas de incorporación
        // corresponden al agregado Watchlist.
        watchlist.AddContent(contentId);

        await _watchlistRepository.UpdateAsync(
            watchlist,
            cancellationToken);

        return Unit.Value;
    }
}

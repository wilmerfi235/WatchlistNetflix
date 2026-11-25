using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Watchlist.Interfaces;
using Domain.Identity.ValueObjects;
using Domain.Watchlist.Entities;
using Application.Watchlist.Commands.CreateWatchlist;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using WatchlistAggregate = Domain.Watchlist.Entities.Watchlist;

namespace Application.Watchlist.Commands.CreateWatchlist;

/// <summary>
/// Ejecuta la creación de una lista de seguimiento.
/// </summary>
public sealed class CreateWatchlistCommandHandler
    : IRequestHandler<CreateWatchlistCommand, Guid>
{
    private readonly IWatchlistRepository _watchlistRepository;
    private readonly ICurrentUserService _currentUserService;

    /// <summary>
    /// Inicializa el handler con las dependencias necesarias.
    /// </summary>
    /// <param name="watchlistRepository">Repositorio de listas de seguimiento.</param>
    /// <param name="currentUserService">Servicio del usuario autenticado.</param>
    public CreateWatchlistCommandHandler(
        IWatchlistRepository watchlistRepository,
        ICurrentUserService currentUserService)
    {
        _watchlistRepository = watchlistRepository;
        _currentUserService = currentUserService;
    }

    /// <summary>
    /// Crea una lista de seguimiento para el usuario autenticado.
    /// </summary>
    /// <param name="request">Datos necesarios para crear la lista.</param>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    /// <returns>Identificador de la lista creada.</returns>
    public async Task<Guid> Handle(
        CreateWatchlistCommand request,
        CancellationToken cancellationToken)
    {
        // 1. Obtener el identificador del usuario autenticado.
        var currentUserId = _currentUserService.UserId
            ?? throw new UnauthorizedException(
                "Es necesario iniciar sesión para crear una lista.");

        // 2. Convertir Guid en el Value Object UserId.
        var ownerId = UserId.From(currentUserId);

        // 3. Normalizar el nombre.
        var name = request.Name.Trim();

        // 4. Comprobar si el usuario ya tiene una lista con ese nombre.
        var exists = await _watchlistRepository.ExistsByNameAsync(
            currentUserId,
            name,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "Ya existe una lista con ese nombre.");
        }

        // 5. Crear el agregado utilizando el método existente en Domain.
        var watchlist = WatchlistAggregate.Create(
            ownerId,
            name);

        // 6. Persistir la nueva lista.
        await _watchlistRepository.AddAsync(
            watchlist,
            cancellationToken);

        // 7. Devolver el identificador de la lista creada.
        return watchlist.Id.Value;
    }
}
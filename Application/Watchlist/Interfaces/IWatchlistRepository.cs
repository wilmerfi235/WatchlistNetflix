using Application.Watchlist.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Watchlist.Interfaces;

/// <summary>
/// Define las operaciones de persistencia y consulta de las listas.
/// </summary>
public interface IWatchlistRepository
{
    /// <summary>
    /// Recupera un agregado de lista mediante su identificador.
    /// </summary>
    Task<Domain.Watchlist.Entities.Watchlist?> GetByIdAsync(
        Guid watchlistId,
        CancellationToken cancellationToken);

    /// <summary>
    /// Comprueba si una lista pertenece al usuario indicado.
    /// </summary>
    Task<bool> ExistsForUserAsync(
        Guid watchlistId,
        Guid userId,
        CancellationToken cancellationToken);

    /// <summary>
    /// Guarda una nueva lista de seguimiento.
    /// </summary>
    Task AddAsync(
        Domain.Watchlist.Entities.Watchlist watchlist,
        CancellationToken cancellationToken);

    /// <summary>
    /// Persiste los cambios realizados sobre una lista existente.
    /// </summary>
    Task UpdateAsync(
        Domain.Watchlist.Entities.Watchlist watchlist,
        CancellationToken cancellationToken);

    /// <summary>
    /// Obtiene todas las listas pertenecientes a un usuario.
    /// </summary>
    Task<IReadOnlyCollection<WatchlistDto>> GetAllByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken);

    /// <summary>
    /// Obtiene una lista concreta perteneciente a un usuario.
    /// </summary>
    Task<WatchlistDto?> GetByIdAndUserIdAsync(
        Guid watchlistId,
        Guid userId,
        CancellationToken cancellationToken);

    /// <summary>
    /// Comprueba si el usuario ya tiene una lista con ese nombre.
    /// </summary>
    Task<bool> ExistsByNameAsync(
        Guid userId,
        string name,
        CancellationToken cancellationToken);
}

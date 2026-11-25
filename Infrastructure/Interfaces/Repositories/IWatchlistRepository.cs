using Domain.Identity.ValueObjects;
using Domain.Watchlist.Entities;
using Domain.Watchlist.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Interfaces.Repositories;

/// <summary>
/// Define las operaciones de persistencia del agregado Watchlist.
/// Permite recuperar y almacenar listas personales de los usuarios.
/// </summary>
public interface IWatchlistRepository
{
    /// <summary>
    /// Busca una lista mediante su identificador,
    /// incluyendo sus elementos asociados.
    /// </summary>
    Task<Watchlist?> GetByIdAsync(
        WatchlistId watchlistId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene todas las listas pertenecientes a un usuario.
    /// </summary>
    Task<IReadOnlyList<Watchlist>> GetByOwnerIdAsync(
        UserId ownerId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Comprueba si una lista pertenece al usuario indicado.
    /// </summary>
    Task<bool> ExistsForOwnerAsync(
        WatchlistId watchlistId,
        UserId ownerId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Añade un nuevo agregado Watchlist a la persistencia.
    /// </summary>
    Task AddAsync(
        Watchlist watchlist,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Marca el agregado Watchlist para actualizar sus cambios.
    /// </summary>
    void Update(Watchlist watchlist);

    /// <summary>
    /// Marca una lista para su eliminación.
    /// </summary>
    void Remove(Watchlist watchlist);
}

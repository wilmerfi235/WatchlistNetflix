using Domain.Catalog.ValueObjects;
using Domain.Common;
using Domain.Identity.ValueObjects;
using Domain.Watchlist.Exceptions;
using Domain.Watchlist.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Watchlist.Entities;

/// <summary>
/// Aggregate Root del agregado Watchlist.
///
/// Representa una lista personal perteneciente a un usuario.
///
/// Controla las operaciones de creación, modificación,
/// incorporación y eliminación de contenidos.
///
/// Garantiza que un contenido no aparezca duplicado
/// dentro de la misma lista.
/// </summary>
public sealed class Watchlist : Entity<WatchlistId>
{
    private readonly List<WatchlistItem> _items = [];

    public UserId OwnerId { get; private set; } = default!;

    public string Name { get; private set; } = string.Empty;

    public DateTime CreatedAtUtc { get; private set; }

    public IReadOnlyCollection<WatchlistItem> Items
        => _items.AsReadOnly();

    private Watchlist()
    {
        // Constructor requerido por EF Core.
    }

    private Watchlist(
        WatchlistId id,
        UserId ownerId,
        string name)
        : base(id)
    {
        OwnerId = ownerId;
        Name = name.Trim();
        CreatedAtUtc = DateTime.UtcNow;
    }

    /// <summary>
    /// Crea una lista personal para un usuario.
    /// </summary>
    public static Watchlist Create(
        UserId ownerId,
        string name)
    {
        ArgumentNullException.ThrowIfNull(ownerId);

        ValidateName(name);

        return new Watchlist(
            WatchlistId.New(),
            ownerId,
            name);
    }

    /// <summary>
    /// Comprueba que el usuario sea propietario de la lista.
    /// </summary>
    public void EnsureOwnership(UserId userId)
    {
        ArgumentNullException.ThrowIfNull(userId);

        if (!OwnerId.Equals(userId))
            throw new WatchlistOwnershipException();
    }

    /// <summary>
    /// Cambia el nombre de la lista.
    /// </summary>
    public void Rename(string newName)
    {
        ValidateName(newName);

        Name = newName.Trim();
    }

    /// <summary>
    /// Añade una película o serie a la lista.
    ///
    /// No permite duplicados por ContentId.
    /// La existencia del contenido se verifica en Application.
    /// </summary>
    public void AddContent(ContentId contentId)
    {
        ArgumentNullException.ThrowIfNull(contentId);

        bool alreadyExists = _items.Any(
            item => item.ContentId.Equals(contentId));

        if (alreadyExists)
            throw new DuplicateWatchlistItemException();

        var item = WatchlistItem.Create(contentId);

        _items.Add(item);
    }

    /// <summary>
    /// Elimina un contenido de la lista.
    ///
    /// Únicamente elimina la asociación con la lista.
    /// Nunca elimina el contenido del catálogo.
    /// </summary>
    public bool RemoveContent(ContentId contentId)
    {
        ArgumentNullException.ThrowIfNull(contentId);

        var item = _items.FirstOrDefault(
            item => item.ContentId.Equals(contentId));

        if (item is null)
            return false;

        _items.Remove(item);

        return true;
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(
                "El nombre de la lista es obligatorio.");

        if (name.Trim().Length > 100)
            throw new ArgumentException(
                "El nombre de la lista no puede superar 100 caracteres.");
    }
}

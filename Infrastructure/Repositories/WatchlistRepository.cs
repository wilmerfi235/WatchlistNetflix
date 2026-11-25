using Application.Watchlist.DTOs;
using Application.Watchlist.Interfaces;
using Domain.Catalog.Entities;
using Domain.Catalog.ValueObjects;
using Domain.Identity.ValueObjects;
using Domain.Watchlist.Entities;
using Domain.Watchlist.ValueObjects;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

/// <summary>
/// Implementa la persistencia del agregado Watchlist mediante Entity Framework Core.
/// </summary>
public sealed class WatchlistRepository : IWatchlistRepository
{
    private readonly ApplicationDbContext _context;

    public WatchlistRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Watchlist?> GetByIdAsync(
        Guid watchlistId,
        CancellationToken cancellationToken)
    {
        return await _context.Watchlists
            .Include(watchlist => watchlist.Items)
            .FirstOrDefaultAsync(
                watchlist => watchlist.Id == WatchlistId.From(watchlistId),
                cancellationToken);
    }

    public Task<bool> ExistsForUserAsync(
        Guid watchlistId,
        Guid userId,
        CancellationToken cancellationToken)
    {
        return _context.Watchlists.AnyAsync(
            watchlist =>
                watchlist.Id == WatchlistId.From(watchlistId) &&
                watchlist.OwnerId == UserId.From(userId),
            cancellationToken);
    }

    public async Task AddAsync(
        Watchlist watchlist,
        CancellationToken cancellationToken)
    {
        await _context.Watchlists.AddAsync(watchlist, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        Watchlist watchlist,
        CancellationToken cancellationToken)
    {
        _context.Watchlists.Update(watchlist);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<WatchlistDto>> GetAllByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var watchlists = await _context.Watchlists
            .AsNoTracking()
            .Include(watchlist => watchlist.Items)
            .Where(watchlist => watchlist.OwnerId == UserId.From(userId))
            .ToListAsync(cancellationToken);

        var contents = await GetContentsByIdsAsync(
            watchlists.SelectMany(watchlist => watchlist.Items)
                .Select(item => item.ContentId),
            cancellationToken);

        return watchlists
            .Select(watchlist => ToDto(watchlist, contents))
            .ToArray();
    }

    public async Task<WatchlistDto?> GetByIdAndUserIdAsync(
        Guid watchlistId,
        Guid userId,
        CancellationToken cancellationToken)
    {
        var watchlist = await _context.Watchlists
            .AsNoTracking()
            .Include(item => item.Items)
            .FirstOrDefaultAsync(
                item => item.Id == WatchlistId.From(watchlistId)
                    && item.OwnerId == UserId.From(userId),
                cancellationToken);

        if (watchlist is null)
        {
            return null;
        }

        var contents = await GetContentsByIdsAsync(
            watchlist.Items.Select(item => item.ContentId),
            cancellationToken);

        return ToDto(watchlist, contents);
    }

    public Task<bool> ExistsByNameAsync(
        Guid userId,
        string name,
        CancellationToken cancellationToken)
    {
        return _context.Watchlists.AnyAsync(
            watchlist =>
                watchlist.OwnerId == UserId.From(userId) &&
                watchlist.Name == name,
            cancellationToken);
    }

    private async Task<IReadOnlyDictionary<Guid, Content>> GetContentsByIdsAsync(
        IEnumerable<ContentId> contentIds,
        CancellationToken cancellationToken)
    {
        var ids = contentIds.Distinct().ToArray();
        if (ids.Length == 0)
        {
            return new Dictionary<Guid, Content>();
        }

        var contents = await _context.Contents
            .AsNoTracking()
            .Where(content => ids.Contains(content.Id))
            .ToListAsync(cancellationToken);

        return contents.ToDictionary(content => content.Id.Value);
    }

    private static WatchlistDto ToDto(
        Watchlist watchlist,
        IReadOnlyDictionary<Guid, Content> contents)
    {
        var items = watchlist.Items
            .Select(item =>
            {
                if (!contents.TryGetValue(item.ContentId.Value, out var content))
                {
                    throw new InvalidOperationException(
                        $"No se encontró el contenido {item.ContentId.Value} asociado a la lista {watchlist.Id.Value}.");
                }

                return new WatchlistItemDto(
                    item.ContentId.Value,
                    content.Title,
                    content.Type.ToString(),
                    content.PosterUrl,
                    item.AddedAtUtc);
            })
            .ToArray();

        return new WatchlistDto(
            watchlist.Id.Value,
            watchlist.Name,
            null,
            watchlist.CreatedAtUtc,
            items);
    }
}

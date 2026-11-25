using Application.Catalog.DTOs;
using Application.Catalog.Interfaces;
using Domain.Catalog.Entities;
using Domain.Catalog.Enums;
using Domain.Catalog.ValueObjects;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

/// <summary>
/// Implementa las consultas del catálogo mediante Entity Framework Core.
/// </summary>
public sealed class ContentRepository : IContentRepository
{
    private readonly ApplicationDbContext _context;

    public ContentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResultDto<ContentDto>> GetAllAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm,
        string? contentType,
        CancellationToken cancellationToken)
    {
        var query = _context.Contents.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(content => content.Title.Contains(searchTerm));
        }

        if (!string.IsNullOrWhiteSpace(contentType))
        {
            if (!Enum.TryParse<ContentType>(contentType, true, out var type)
                || !Enum.IsDefined(type))
            {
                throw new ArgumentException(
                    $"El tipo de contenido '{contentType}' no es válido.",
                    nameof(contentType));
            }

            query = query.Where(content => content.Type == type);
        }

        var totalCount = await query.CountAsync(cancellationToken);
        var contents = await query
            .OrderBy(content => content.Title)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var items = contents.Select(ToDto).ToArray();
        return new PagedResultDto<ContentDto>(
            items,
            pageNumber,
            pageSize,
            totalCount);
    }

    public async Task<ContentDto?> GetByIdAsync(
        Guid contentId,
        CancellationToken cancellationToken)
    {
        var content = await _context.Contents
            .AsNoTracking()
            .FirstOrDefaultAsync(
                item => item.Id == ContentId.From(contentId),
                cancellationToken);

        return content is null ? null : ToDto(content);
    }

    private static ContentDto ToDto(Content content)
    {
        return new ContentDto(
            content.Id.Value,
            content.Title,
            content.Description ?? string.Empty,
            content.Type.ToString(),
            content.ReleaseYear,
            content.PosterUrl,
            null);
    }
}

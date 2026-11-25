using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Catalog.DTOs;

/// <summary>
/// Representa un resultado paginado de una consulta.
/// </summary>
/// <typeparam name="T">Tipo de elemento devuelto.</typeparam>
public sealed record PagedResultDto<T>(
    IReadOnlyCollection<T> Items,
    int PageNumber,
    int PageSize,
    int TotalCount)
{
    /// <summary>
    /// Calcula el número total de páginas disponibles.
    /// </summary>
    public int TotalPages =>
        (int)Math.Ceiling((double)TotalCount / PageSize);

    /// <summary>
    /// Indica si existe una página anterior.
    /// </summary>
    public bool HasPreviousPage => PageNumber > 1;

    /// <summary>
    /// Indica si existe una página posterior.
    /// </summary>
    public bool HasNextPage => PageNumber < TotalPages;
}

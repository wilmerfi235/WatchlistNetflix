using Application.Catalog.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Catalog.Interfaces;

/// <summary>
/// Define las operaciones de consulta del catálogo de contenidos.
/// </summary>
public interface IContentRepository
{
    /// <summary>
    /// Obtiene los contenidos del catálogo aplicando filtros y paginación.
    /// </summary>
    /// <param name="pageNumber">Número de página solicitado.</param>
    /// <param name="pageSize">Cantidad máxima de resultados.</param>
    /// <param name="searchTerm">Texto opcional para buscar por título.</param>
    /// <param name="contentType">Tipo de contenido opcional.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    Task<PagedResultDto<ContentDto>> GetAllAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm,
        string? contentType,
        CancellationToken cancellationToken);

    /// <summary>
    /// Obtiene un contenido concreto mediante su identificador.
    /// </summary>
    /// <param name="contentId">Identificador del contenido.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    Task<ContentDto?> GetByIdAsync(
        Guid contentId,
        CancellationToken cancellationToken);
}

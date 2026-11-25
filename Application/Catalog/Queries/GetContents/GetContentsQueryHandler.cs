using Application.Catalog.DTOs;
using Application.Catalog.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Catalog.Queries.GetContents;

/// <summary>
/// Ejecuta la consulta paginada del catálogo.
/// </summary>
public sealed class GetContentsQueryHandler
    : IRequestHandler<GetContentsQuery, PagedResultDto<ContentDto>>
{
    private readonly IContentRepository _contentRepository;

    /// <summary>
    /// Inicializa el handler con el repositorio del catálogo.
    /// </summary>
    public GetContentsQueryHandler(
        IContentRepository contentRepository)
    {
        _contentRepository = contentRepository;
    }

    /// <summary>
    /// Recupera los contenidos aplicando los filtros solicitados.
    /// </summary>
    public async Task<PagedResultDto<ContentDto>> Handle(
        GetContentsQuery request,
        CancellationToken cancellationToken)
    {
        return await _contentRepository.GetAllAsync(
            request.PageNumber,
            request.PageSize,
            request.SearchTerm,
            request.ContentType,
            cancellationToken);
    }
}

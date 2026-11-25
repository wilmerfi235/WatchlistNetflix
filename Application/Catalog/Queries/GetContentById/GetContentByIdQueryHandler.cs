using Application.Catalog.DTOs;
using Application.Catalog.Interfaces;
using Application.Common.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Catalog.Queries.GetContentById;

/// <summary>
/// Ejecuta la consulta de un contenido específico.
/// </summary>
public sealed class GetContentByIdQueryHandler
    : IRequestHandler<GetContentByIdQuery, ContentDto>
{
    private readonly IContentRepository _contentRepository;

    /// <summary>
    /// Inicializa el handler con el repositorio del catálogo.
    /// </summary>
    public GetContentByIdQueryHandler(
        IContentRepository contentRepository)
    {
        _contentRepository = contentRepository;
    }

    /// <summary>
    /// Busca el contenido solicitado y devuelve su información.
    /// </summary>
    public async Task<ContentDto> Handle(
        GetContentByIdQuery request,
        CancellationToken cancellationToken)
    {
        var content = await _contentRepository.GetByIdAsync(
            request.ContentId,
            cancellationToken);

        if (content is null)
        {
            throw new NotFoundException(
                $"No se encontró el contenido con identificador {request.ContentId}.");
        }

        return content;
    }
}

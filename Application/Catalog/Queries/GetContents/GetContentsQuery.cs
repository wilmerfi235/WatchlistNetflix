using Application.Catalog.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Catalog.Queries.GetContents;

/// <summary>
/// Solicita la consulta paginada de los contenidos disponibles.
/// </summary>
public sealed record GetContentsQuery(
    int PageNumber = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    string? ContentType = null
) : IRequest<PagedResultDto<ContentDto>>;

using Application.Catalog.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Catalog.Queries.GetContentById;


/// <summary>
/// Solicita la información de un contenido mediante su identificador.
/// </summary>
public sealed record GetContentByIdQuery(
    Guid ContentId
) : IRequest<ContentDto>;

using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Catalog.DTOs;

/// <summary>
/// Representa la información de un contenido del catálogo
/// que puede consultar el cliente.
/// </summary>
public sealed record ContentDto(
    Guid Id,
    string Title,
    string Description,
    string ContentType,
    int? ReleaseYear,
    string? ImageUrl,
    int? DurationMinutes);

using Domain.Catalog.Entities;
using Domain.Catalog.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Interfaces.Repositories;

/// <summary>
/// Define las operaciones de acceso al catálogo audiovisual.
/// Permite consultar contenidos sin modificar sus reglas de dominio.
/// </summary>
public interface IContentRepository
{
    /// <summary>
    /// Recupera un contenido mediante su identificador.
    /// </summary>
    Task<Content?> GetByIdAsync(
        ContentId contentId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene todos los contenidos disponibles en el catálogo.
    /// </summary>
    Task<IReadOnlyList<Content>> GetAllAsync(
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca contenidos cuyo título coincida parcial o totalmente
    /// con el texto proporcionado.
    /// </summary>
    Task<IReadOnlyList<Content>> SearchAsync(
        string searchTerm,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Comprueba si existe un contenido con el identificador indicado.
    /// </summary>
    Task<bool> ExistsAsync(
        ContentId contentId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Añade un nuevo contenido al catálogo.
    /// </summary>
    Task AddAsync(
        Content content,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Marca un contenido para actualizar sus datos.
    /// </summary>
    void Update(Content content);
}

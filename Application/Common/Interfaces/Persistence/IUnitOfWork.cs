using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Interfaces.Persistence;

/// <summary>
/// Define el contrato de la unidad de trabajo.
///
/// Centraliza la confirmación de los cambios realizados
/// por los diferentes repositorios durante un caso de uso.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Confirma todos los cambios pendientes en la persistencia.
    /// </summary>
    Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default);
}

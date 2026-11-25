using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence;

/// <summary>
/// Define el contrato para confirmar las operaciones
/// de persistencia realizadas durante un caso de uso.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Persiste todos los cambios pendientes en la base de datos.
    /// </summary>
    Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default);
}

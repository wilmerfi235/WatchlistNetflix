using Domain.Identity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Identity.Interfaces;

/// <summary>
/// Define las operaciones de consulta necesarias para gestionar
/// la identidad de los usuarios.
/// </summary>
public interface IIdentityRepository
{
    /// <summary>
    /// Busca un usuario mediante su correo electrónico.
    /// </summary>
    /// <param name="email">Correo electrónico del usuario.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Usuario encontrado o null si no existe.</returns>
    Task<User?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken);

    Task<bool> ExistsByEmailAsync(
        string email,
        CancellationToken cancellationToken);

    Task AddAsync(
        User user,
        CancellationToken cancellationToken);
}

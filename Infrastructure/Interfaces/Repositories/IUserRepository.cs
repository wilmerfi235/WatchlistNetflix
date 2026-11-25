using Domain.Identity.Entities;
using Domain.Identity.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Interfaces.Repositories;

/// <summary>
/// Define las operaciones de persistencia relacionadas con los usuarios.
/// Infrastructure proporciona la implementación concreta.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Busca un usuario utilizando su identificador.
    /// </summary>
    Task<User?> GetByIdAsync(
        UserId userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca un usuario mediante su dirección de correo electrónico.
    /// Se utiliza principalmente durante el proceso de autenticación.
    /// </summary>
    Task<User?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Comprueba si ya existe un usuario registrado con el correo indicado.
    /// </summary>
    Task<bool> ExistsByEmailAsync(
        string email,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Registra un nuevo usuario en la unidad de persistencia.
    /// </summary>
    Task AddAsync(
        User user,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Marca un usuario existente para actualizar sus datos.
    /// </summary>
    void Update(User user);

    /// <summary>
    /// Marca un usuario para su eliminación.
    /// </summary>
    void Remove(User user);
}

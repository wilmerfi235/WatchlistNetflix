using Domain.Common;
using Domain.Identity.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Identity.Entities;

/// <summary>
/// Representa un usuario registrado en WatchlistNetflix.
///
/// Un usuario puede ser propietario de múltiples listas.
/// La gestión de contraseñas y autenticación no se implementa
/// directamente en esta entidad.
/// </summary>
public sealed class User : Entity<UserId>
{
    public Email Email { get; private set; } = default!;

    public DateTime CreatedAtUtc { get; private set; }

    public bool IsActive { get; private set; }

    public string PasswordHash { get; private set; } = string.Empty;

    private User()
    {
        // Constructor requerido por EF Core.
    }

    private User(UserId id, Email email, string passwordHash)
        : base(id)
    {
        Email = email;
        PasswordHash = passwordHash;
        CreatedAtUtc = DateTime.UtcNow;
        IsActive = true;
    }

    /// <summary>
    /// Crea un nuevo usuario.
    /// </summary>
    public static User Create(Email email, string passwordHash)
    {
        ArgumentNullException.ThrowIfNull(email);
        ArgumentException.ThrowIfNullOrWhiteSpace(passwordHash);

        return new User(UserId.New(), email, passwordHash);
    }

    /// <summary>
    /// Permite actualizar el correo electrónico.
    /// La unicidad se comprobará en la capa Application
    /// mediante el repositorio correspondiente.
    /// </summary>
    public void ChangeEmail(Email newEmail)
    {
        ArgumentNullException.ThrowIfNull(newEmail);

        Email = newEmail;
    }

    /// <summary>
    /// Desactiva el usuario sin eliminar su información.
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
    }

    public void Activate()
    {
        IsActive = true;
    }
}

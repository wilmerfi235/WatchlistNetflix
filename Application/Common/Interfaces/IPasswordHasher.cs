using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Interfaces;

/// <summary>
/// Define las operaciones necesarias para verificar contraseñas.
/// </summary>
public interface IPasswordHasher
{
    /// <summary>
    /// Genera un hash seguro a partir de una contraseña.
    /// </summary>
    string HashPassword(string password);

    /// <summary>
    /// Comprueba si la contraseña proporcionada coincide
    /// con el hash almacenado del usuario.
    /// </summary>
    /// <param name="password">Contraseña introducida por el usuario.</param>
    /// <param name="passwordHash">Hash almacenado.</param>
    /// <returns>True si la contraseña es válida.</returns>
    bool Verify(string password, string passwordHash);
}

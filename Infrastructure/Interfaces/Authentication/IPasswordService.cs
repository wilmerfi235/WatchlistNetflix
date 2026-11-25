using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Interfaces.Authentication;

/// <summary>
/// Define las operaciones necesarias para proteger y verificar
/// las contraseñas de los usuarios.
/// </summary>
public interface IPasswordService
{
    /// <summary>
    /// Genera un hash seguro a partir de una contraseña.
    /// Nunca debe almacenarse la contraseña original.
    /// </summary>
    string HashPassword(string password);

    /// <summary>
    /// Verifica si una contraseña coincide con el hash almacenado.
    /// </summary>
    bool VerifyPassword(string password, string passwordHash);
}


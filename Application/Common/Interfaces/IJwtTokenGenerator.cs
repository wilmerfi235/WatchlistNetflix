using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Interfaces;

/// <summary>
/// Define el contrato para generar tokens de autenticación.
/// </summary>
public interface IJwtTokenGenerator
{
    /// <summary>
    /// Genera un token de acceso para el usuario indicado.
    /// </summary>
    /// <param name="userId">Identificador del usuario.</param>
    /// <param name="email">Correo electrónico del usuario.</param>
    /// <returns>Token JWT generado.</returns>
    string GenerateToken(Guid userId, string email);
}

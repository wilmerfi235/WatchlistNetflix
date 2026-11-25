using Domain.Identity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Interfaces.Authentication;

/// <summary>
/// Define el contrato para generar tokens JWT de autenticación.
/// La implementación concreta pertenece a Infrastructure.
/// </summary>
public interface IJwtTokenService
{
    /// <summary>
    /// Genera un token JWT firmado para identificar al usuario
    /// durante las solicitudes autenticadas.
    /// </summary>
    string GenerateToken(User user);
}

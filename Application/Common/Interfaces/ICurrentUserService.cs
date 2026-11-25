using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Interfaces;

/// <summary>
/// Proporciona información sobre el usuario que ha iniciado sesión.
/// </summary>
public interface ICurrentUserService
{
    /// <summary>
    /// Obtiene el identificador del usuario autenticado.
    /// Devuelve null cuando no existe un usuario autenticado.
    /// </summary>
    Guid? UserId { get; }
}

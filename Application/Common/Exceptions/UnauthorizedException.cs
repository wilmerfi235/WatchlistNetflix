using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Exceptions;

/// <summary>
/// Representa un intento de acceso no autorizado.
/// </summary>
public sealed class UnauthorizedException : Exception
{
    /// <summary>
    /// Inicializa la excepción con un mensaje descriptivo.
    /// </summary>
    public UnauthorizedException(string message)
        : base(message)
    {
    }
}

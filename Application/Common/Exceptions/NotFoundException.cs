using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Exceptions;

/// <summary>
/// Representa un error cuando no se encuentra un recurso solicitado.
/// </summary>
public sealed class NotFoundException : Exception
{
    /// <summary>
    /// Inicializa la excepción con un mensaje descriptivo.
    /// </summary>
    public NotFoundException(string message)
        : base(message)
    {
    }
}

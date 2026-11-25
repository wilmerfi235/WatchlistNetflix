using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Exceptions;

/// <summary>
/// Representa un conflicto con el estado actual del sistema.
/// </summary>
public sealed class ConflictException : Exception
{
    /// <summary>
    /// Inicializa la excepción con el motivo del conflicto.
    /// </summary>
    public ConflictException(string message)
        : base(message)
    {
    }
}

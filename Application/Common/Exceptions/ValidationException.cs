using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Exceptions;

/// <summary>
/// Representa errores de validación de una operación.
/// </summary>
public sealed class ValidationException : Exception
{
    /// <summary>
    /// Inicializa la excepción con los errores encontrados.
    /// </summary>
    public ValidationException(IEnumerable<string> errors)
        : base("Se han producido errores de validación.")
    {
        ArgumentNullException.ThrowIfNull(errors);
        Errors = errors.ToArray();
    }

    /// <summary>
    /// Obtiene la colección de errores de validación.
    /// </summary>
    public IReadOnlyCollection<string> Errors { get; }
}

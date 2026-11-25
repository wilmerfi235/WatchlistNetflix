using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Interfaces;

/// <summary>
/// Proporciona acceso abstracto a la fecha y hora actual.
/// </summary>
public interface IDateTimeProvider
{
    /// <summary>
    /// Obtiene la fecha y hora actual en UTC.
    /// </summary>
    DateTime UtcNow { get; }
}

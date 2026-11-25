using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Identity.DTOs;

/// <summary>
/// Contiene la información devuelta después de un inicio de sesión exitoso.
/// </summary>
public sealed record LoginResponseDto(
    Guid UserId,
    string Email,
    string AccessToken,
    DateTime ExpiresAtUtc);

using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Identity.DTOs;


/// <summary>
/// Contiene las credenciales necesarias para iniciar sesión.
/// </summary>
public sealed record LoginRequestDto(
    string Email,
    string Password);

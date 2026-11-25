using Application.Identity.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Identity.Commands.Login;

/// <summary>
/// Representa la solicitud de autenticación de un usuario.
/// </summary>
public sealed record LoginCommand(
    string Email,
    string Password
) : IRequest<LoginResponseDto>;

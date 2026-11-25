using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Identity.DTOs;
using Application.Identity.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Identity.Commands.Login;

/// <summary>
/// Ejecuta el proceso de autenticación de un usuario.
/// </summary>
public sealed class LoginCommandHandler
    : IRequestHandler<LoginCommand, LoginResponseDto>
{
    private readonly IIdentityRepository _identityRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    /// <summary>
    /// Inicializa el handler con las dependencias necesarias.
    /// </summary>
    public LoginCommandHandler(
        IIdentityRepository identityRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _identityRepository = identityRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    /// <summary>
    /// Comprueba las credenciales y genera un token de acceso.
    /// </summary>
    public async Task<LoginResponseDto> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _identityRepository.GetByEmailAsync(
            request.Email,
            cancellationToken);

        if (user is null || !user.IsActive)
        {
            throw new UnauthorizedException(
                "El correo electrónico o la contraseña no son correctos.");
        }

        if (!_passwordHasher.Verify(
                request.Password,
                user.PasswordHash))
        {
            throw new UnauthorizedException(
                "El correo electrónico o la contraseña no son correctos.");
        }

        // Se asume que el dominio expone User.Id como Guid.
        // Si UserId es un Value Object, se utilizará su propiedad correspondiente.
        var userId = user.Id.Value;

        var token = _jwtTokenGenerator.GenerateToken(
            userId,
            user.Email.Value);

        return new LoginResponseDto(
            userId,
            user.Email.Value,
            token,
            DateTime.UtcNow.AddHours(1));
    }
}

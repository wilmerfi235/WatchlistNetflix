using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Identity.Commands.Login;

/// <summary>
/// Valida las credenciales recibidas durante el inicio de sesión.
/// </summary>
public sealed class LoginCommandValidator
    : AbstractValidator<LoginCommand>
{
    /// <summary>
    /// Configura las reglas de validación de las credenciales.
    /// </summary>
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("El correo electrónico es obligatorio.")
            .EmailAddress()
            .WithMessage("El correo electrónico no tiene un formato válido.");

        RuleFor(x => x.Password)
            .Must(password => !string.IsNullOrWhiteSpace(password))
            .WithMessage("La contraseña es obligatoria.");
    }
}

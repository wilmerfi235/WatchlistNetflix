using Domain.Common;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace Domain.Identity.ValueObjects;

/// <summary>
/// Representa una dirección de correo electrónico válida.
///
/// Normaliza el valor para facilitar las comparaciones
/// y evitar diferencias por mayúsculas y minúsculas.
/// </summary>
public sealed class Email : ValueObject
{
    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    public static Email Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException(
                "El correo electrónico es obligatorio.");

        var normalized = value.Trim().ToLowerInvariant();

        try
        {
            var address = new MailAddress(normalized);

            if (address.Address != normalized)
                throw new ArgumentException(
                    "El correo electrónico no tiene un formato válido.");
        }
        catch (FormatException)
        {
            throw new ArgumentException(
                "El correo electrónico no tiene un formato válido.");
        }

        return new Email(normalized);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}

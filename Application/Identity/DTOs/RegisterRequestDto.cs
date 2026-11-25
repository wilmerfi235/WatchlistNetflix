namespace Application.Identity.DTOs;

public sealed record RegisterRequestDto(
    string Email,
    string Password);

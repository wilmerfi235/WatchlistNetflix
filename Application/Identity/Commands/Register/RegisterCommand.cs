using Application.Identity.DTOs;
using MediatR;

namespace Application.Identity.Commands.Register;

public sealed record RegisterCommand(
    string Email,
    string Password
) : IRequest<RegisterResponseDto>;

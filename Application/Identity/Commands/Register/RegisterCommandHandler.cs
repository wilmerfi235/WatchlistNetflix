using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Persistence;
using Application.Identity.DTOs;
using Application.Identity.Interfaces;
using Domain.Identity.Entities;
using Domain.Identity.ValueObjects;
using MediatR;

namespace Application.Identity.Commands.Register;

public sealed class RegisterCommandHandler
    : IRequestHandler<RegisterCommand, RegisterResponseDto>
{
    private readonly IIdentityRepository _identityRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterCommandHandler(
        IIdentityRepository identityRepository,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork)
    {
        _identityRepository = identityRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }

    public async Task<RegisterResponseDto> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        var email = Email.Create(request.Email);
        var emailExists = await _identityRepository.ExistsByEmailAsync(
            email.Value,
            cancellationToken);

        if (emailExists)
        {
            throw new ConflictException(
                "Ya existe una cuenta con ese correo electrónico.");
        }

        var user = User.Create(
            email,
            _passwordHasher.HashPassword(request.Password));

        await _identityRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new RegisterResponseDto(user.Id.Value, user.Email.Value);
    }
}

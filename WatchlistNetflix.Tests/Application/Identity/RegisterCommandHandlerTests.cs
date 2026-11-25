using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Persistence;
using Application.Identity.Commands.Login;
using Application.Identity.Commands.Register;
using Application.Identity.DTOs;
using Application.Identity.Interfaces;
using Domain.Identity.Entities;
using Domain.Identity.ValueObjects;
using WatchlistNetflix.Api.Services;

namespace WatchlistNetflix.Tests.Application.Identity;

public sealed class RegisterCommandHandlerTests
{
    [Fact]
    public async Task Register_creates_hashed_user_that_can_log_in()
    {
        const string email = "usuario@test.com";
        const string password = "Password123!";
        var repository = new FakeIdentityRepository();
        var passwordHasher = new PasswordHasherService();
        var unitOfWork = new FakeUnitOfWork();

        var response = await new RegisterCommandHandler(
                repository,
                passwordHasher,
                unitOfWork)
            .Handle(new RegisterCommand(email, password), CancellationToken.None);

        var user = Assert.Single(repository.Users);
        Assert.Equal(response.Id, user.Id.Value);
        Assert.Equal(email, response.Email);
        Assert.NotEqual(password, user.PasswordHash);
        Assert.True(passwordHasher.Verify(password, user.PasswordHash));
        Assert.Equal(1, unitOfWork.SaveChangesCalls);

        var loginResponse = await new LoginCommandHandler(
                repository,
                passwordHasher,
                new FakeJwtTokenGenerator())
            .Handle(new LoginCommand(email, password), CancellationToken.None);

        Assert.Equal(response.Id, loginResponse.UserId);
        Assert.Equal(email, loginResponse.Email);
        Assert.Equal("test-token", loginResponse.AccessToken);
    }

    [Fact]
    public async Task Register_rejects_an_existing_email()
    {
        const string email = "usuario@test.com";
        var passwordHasher = new PasswordHasherService();
        var repository = new FakeIdentityRepository();
        repository.Users.Add(User.Create(
            Email.Create(email),
            passwordHasher.HashPassword("Password123!")));
        var unitOfWork = new FakeUnitOfWork();

        await Assert.ThrowsAsync<ConflictException>(() =>
            new RegisterCommandHandler(repository, passwordHasher, unitOfWork)
                .Handle(
                    new RegisterCommand(email, "Password123!"),
                    CancellationToken.None));

        Assert.Single(repository.Users);
        Assert.Equal(0, unitOfWork.SaveChangesCalls);
    }

    private sealed class FakeIdentityRepository : IIdentityRepository
    {
        public List<User> Users { get; } = [];

        public Task<User?> GetByEmailAsync(
            string email,
            CancellationToken cancellationToken)
        {
            var normalizedEmail = Email.Create(email);
            return Task.FromResult(Users.SingleOrDefault(
                user => user.Email.Value == normalizedEmail.Value));
        }

        public Task<bool> ExistsByEmailAsync(
            string email,
            CancellationToken cancellationToken)
        {
            var normalizedEmail = Email.Create(email);
            return Task.FromResult(Users.Any(
                user => user.Email.Value == normalizedEmail.Value));
        }

        public Task AddAsync(User user, CancellationToken cancellationToken)
        {
            Users.Add(user);
            return Task.CompletedTask;
        }
    }

    private sealed class FakeUnitOfWork : IUnitOfWork
    {
        public int SaveChangesCalls { get; private set; }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SaveChangesCalls++;
            return Task.FromResult(1);
        }
    }

    private sealed class FakeJwtTokenGenerator : IJwtTokenGenerator
    {
        public string GenerateToken(Guid userId, string email) => "test-token";
    }
}

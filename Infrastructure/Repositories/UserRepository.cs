using Application.Identity.Interfaces;
using Domain.Identity.Entities;
using Domain.Identity.ValueObjects;
using Infrastructure.Interfaces.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

/// <summary>
/// Implementa las operaciones de persistencia de usuarios
/// utilizando Entity Framework Core.
/// </summary>
public sealed class UserRepository : IUserRepository, IIdentityRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Inicializa el repositorio con el contexto de persistencia.
    /// </summary>
    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Recupera un usuario por su identificador.
    /// </summary>
    public async Task<User?> GetByIdAsync(
        UserId userId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .FirstOrDefaultAsync(
                user => user.Id == userId,
                cancellationToken);
    }

    /// <summary>
    /// Recupera un usuario mediante su correo electrónico.
    /// </summary>
    public async Task<User?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        var normalizedEmail = Email.Create(email);

        return await _context.Users
            .FirstOrDefaultAsync(
                user => user.Email == normalizedEmail,
                cancellationToken);
    }

    /// <summary>
    /// Comprueba si existe un usuario registrado con ese correo.
    /// </summary>
    public async Task<bool> ExistsByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        var normalizedEmail = Email.Create(email);

        return await _context.Users
            .AnyAsync(
                user => user.Email == normalizedEmail,
                cancellationToken);
    }

    /// <summary>
    /// Incorpora un usuario nuevo al contexto de persistencia.
    /// </summary>
    public async Task AddAsync(
        User user,
        CancellationToken cancellationToken = default)
    {
        await _context.Users.AddAsync(user, cancellationToken);
    }

    /// <summary>
    /// Marca un usuario como modificado.
    /// </summary>
    public void Update(User user)
    {
        _context.Users.Update(user);
    }

    /// <summary>
    /// Marca un usuario para su eliminación.
    /// </summary>
    public void Remove(User user)
    {
        _context.Users.Remove(user);
    }
}

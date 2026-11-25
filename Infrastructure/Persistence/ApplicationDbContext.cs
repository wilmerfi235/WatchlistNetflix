using Domain.Catalog.Entities;
using Domain.Identity.Entities;
using Domain.Watchlist.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Interfaces.Persistence;


namespace Infrastructure.Persistence;

/// <summary>
/// Contexto principal de Entity Framework Core.
///
/// Representa la sesión de trabajo con la base de datos MySQL
/// y proporciona acceso a los conjuntos de entidades del dominio.
/// </summary>
public sealed class ApplicationDbContext : DbContext,
    IUnitOfWork,
    Application.Common.Interfaces.Persistence.IUnitOfWork
{
    /// <summary>
    /// Inicializa el contexto utilizando las opciones
    /// configuradas mediante la inyección de dependencias.
    /// </summary>
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Representa la tabla de usuarios.
    /// </summary>
    public DbSet<User> Users => Set<User>();

    /// <summary>
    /// Representa la tabla de listas personales.
    /// </summary>
    public DbSet<Watchlist> Watchlists => Set<Watchlist>();

    /// <summary>
    /// Representa los elementos asociados a cada lista.
    /// </summary>
    public DbSet<WatchlistItem> WatchlistItems => Set<WatchlistItem>();

    /// <summary>
    /// Representa el catálogo audiovisual.
    /// </summary>
    public DbSet<Content> Contents => Set<Content>();

    /// <summary>
    /// Representa los géneros disponibles en el catálogo.
    /// </summary>
    public DbSet<Genre> Genres => Set<Genre>();

    /// <summary>
    /// Aplica todas las configuraciones de entidades
    /// definidas en el ensamblado de Infrastructure.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ApplicationDbContext).Assembly);
    }

    /// <summary>
    /// Confirma todos los cambios pendientes en la base de datos.
    /// </summary>
    public override Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        return base.SaveChangesAsync(cancellationToken);
    }
}

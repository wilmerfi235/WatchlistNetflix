using Application.Catalog.Interfaces;
using Application.Identity.Interfaces;
using Application.Watchlist.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;


namespace Infrastructure;

public static class DependencyInjection
{
    /// <summary>
    /// Registra los servicios de infraestructura.
    /// </summary>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString =
        configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseMySql(
                connectionString,
                ServerVersion.AutoDetect(connectionString)));


        // Registro de Entity Framework Core y MySQL.
        // Aquí irá la configuración de ApplicationDbContext.

        // Registro de la unidad de trabajo.
        services.AddScoped<IUnitOfWork>(
            provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<Application.Common.Interfaces.Persistence.IUnitOfWork>(
            provider => provider.GetRequiredService<ApplicationDbContext>());

        // Aquí se registrarán los repositorios y servicios
        // de autenticación.
        // Registro de repositorios.
        services.AddScoped<IWatchlistRepository, WatchlistRepository>();

        services.AddScoped<IIdentityRepository, UserRepository>();

        services.AddScoped<IContentRepository, ContentRepository>();

        return services;
    }
}

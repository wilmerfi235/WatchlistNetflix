namespace WatchlistNetflix.Api.Extensions;

public static class SwaggerServiceCollectionExtensions
{
    public static IServiceCollection AddApiSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen();
        return services;
    }
}

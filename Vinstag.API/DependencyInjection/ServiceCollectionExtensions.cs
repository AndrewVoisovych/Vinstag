using Vinstag.API.Services;
using Vinstag.Common.Models.Options;
using Vinstag.Common.Utils;
using Vinstag.DataAccess.CsvProcessor;
using Vinstag.InstagramAPI;
using Vinstag.InstagramAPI.Data;

namespace Vinstag.API.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHttpClientConfiguration(this IServiceCollection services)
    {
        services.AddHttpClient("instagram", c =>
            {
                c.BaseAddress = new Uri(Endpoints.BaseAdress);
                c.DefaultRequestHeaders.Add("Accept", "application/json");
            }
        );

        services.AddSingleton<IInstagramApi, InstagramApi>();

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<ConnectionService>();
        services.AddSingleton<AuthenticationService>();
        services.AddSingleton<UserService>();
        services.AddSingleton<EnumHelper>();

        return services;
    }

    public static IServiceCollection AddDataProviders(this IServiceCollection services)
    {
        services.AddSingleton<IDataProccesor, CsvProcessor>();

        return services;
    }
    
    public static IServiceCollection AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CsvSettingsOptions>(configuration.GetSection("CsvSettings"));

        return services;
    }
    
}
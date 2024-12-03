using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using CustomLoggerHelper;
using MongoConnect;
using BusinessModel;
using System.Reflection;
using SqlConnect;
using HttpClientConnect;
using ExceptionHandlerCustom;
using EmailConnect;
using Resiliency;
using SecretsKeyVault;
using BusinessModel.Interface;
using BusinessModel.Di;

namespace AllPackages
{
    public static class DiConfigurationCommon
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {            
            var configurationRoot = configuration as IConfigurationRoot;
            services.AddSingleton<IConfigurationRoot>((IConfigurationRoot)configuration);

            // Singleton services
            services.AddSingleton<IKeyVaultManagedIdentityHelper, KeyVaultManagedIdentityHelper>();
            services.AddSingleton<IKeyVaultRbacHelper, KeyVaultRbacHelper>();
            services.AddSingleton<ILoggerHelper, LoggerHelper>();
            services.AddSingleton<IResiliencyHelper, ResiliencyHelper>();
            services.AddSingleton<IExceptionHelper, ExceptionHelper>();
            services.AddSingleton<IEmailHelper, EmailHelper>();
            services.AddSingleton<IHttpClientHelper, HttpClientHelper>();
            services.AddSingleton<ISqlHelper, SqlHelper>();
            services.AddSingleton(typeof(IMongoDbHelper<>), typeof(MongoDbHelper<>));
            services.AddHttpClient<HttpClientHelper>();
            services.AddSingleton<IConnectionStringFactory, ConnectionStringFactory>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            DIRegistry.RegisterDependencies(services, configuration);
            return services;
        }
    }
}

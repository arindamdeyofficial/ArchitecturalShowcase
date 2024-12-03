using Api.Repository.Models;
using Microsoft.EntityFrameworkCore;
using SqlConnect;

namespace ApiDummy
{
    public static class DiConfiguration
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            var orchestrationAssembly = typeof(Tfg.Api.Orchestration.AssemblyMarker).Assembly;

            services.AddDbContext<PaymentDbContext>(async (serviceProvider, options) =>
            {
                var connectionStringFactory = serviceProvider.GetRequiredService<IConnectionStringFactory>();
                var connectionString = await connectionStringFactory.GetConnectionStringAsync(SqlDbEnum.PaymentDbConStr);
                options.UseSqlServer(connectionString);
            });

            // If you have background services
            //services.AddHostedService<BackgroundWorkerService>();



            return services;
        }
    }
}

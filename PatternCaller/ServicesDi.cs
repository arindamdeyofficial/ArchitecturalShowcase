using Factory;
using Microsoft.Extensions.DependencyInjection;

namespace PatternCaller
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddPatternsLibrary(this IServiceCollection services)
        {
            services.AddSingleton<IFactoryPattern, FactoryPattern>();
            return services;
        }
    }
}

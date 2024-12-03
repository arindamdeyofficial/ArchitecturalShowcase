using BusinessModel.Context;
using BusinessModel.Interface;
using BusinessModel.Interface.Common;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BusinessModel.Di
{
    public static class DIRegistry
    {
        public static void RegisterDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddScoped(typeof(BaseContext<>));
            services.AddScoped(typeof(IQueryContext<>), typeof(QueryContext<>));
            services.AddScoped(typeof(ICommandContext<>), typeof(CommandContext<>));

            // Define the interface types and their respective registration logic
            var interfaceMappings = new (Type InterfaceType, Action<Type, IServiceCollection> RegisterAction)[]
            {
                (typeof(IBaseRequest<,>), (type, svc) =>
                    svc.AddScoped(type.GetInterfaces()
                        .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IBaseRequest<,>)), type)),

                (typeof(IBaseQuery<,>), (type, svc) =>
                    svc.AddScoped(type.GetInterfaces()
                        .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IBaseQuery<,>)), type)),

                (typeof(IBaseStreamRequest<,>), (type, svc) =>
                    svc.AddScoped(type.GetInterfaces()
                        .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IBaseStreamRequest<,>)), type)),

                (typeof(IBaseStreamQuery<,>), (type, svc) =>
                    svc.AddScoped(type.GetInterfaces()
                        .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IBaseStreamQuery<,>)), type)),

                (typeof(IBaseCommand<,>), (type, svc) =>
                    svc.AddScoped(type.GetInterfaces()
                        .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IBaseCommand<,>)), type)),

                (typeof(IBaseResponse), (type, svc) =>
                    svc.AddScoped(type.GetInterfaces()
                        .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IBaseResponse)), type)),

                (typeof(IRequestHandler<,>), (type, svc) =>
                    svc.AddScoped(type.GetInterfaces()
                        .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)), type)),

                (typeof(IStreamRequestHandler<,>), (type, svc) =>
                    svc.AddScoped(type.GetInterfaces()
                        .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IStreamRequestHandler<,>)), type)),

                (typeof(IValidator<>), (type, svc) =>
                    svc.AddScoped(type.GetInterfaces()
                        .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidator<>)), type))
            };            

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            //IValidator
            services.AddFluentValidationAutoValidation();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                services.AddValidatorsFromAssembly(assembly);
            }

            // Loop through all types in the assemblies
            foreach (var (interfaceType, registerAction) in interfaceMappings)
            {
                IEnumerable<Type> types;

                if (interfaceType == typeof(IValidator<>))
                {
                    types = assemblies
                        .SelectMany(a => a.GetTypes())
                        .Where(t => t.IsClass && !t.IsAbstract && t.BaseType != null && t.BaseType.IsGenericType
                                    && t.BaseType.GetGenericTypeDefinition() == typeof(AbstractValidator<>)
                                    && !(t.IsGenericType && t.GetGenericTypeDefinition() == typeof(FluentValidation.InlineValidator<>)))
                        .ToList();
                }
                else
                {
                    types = assemblies
                    .SelectMany(a => a.GetTypes())
                    .Where(t => t.IsClass && !t.IsAbstract
                                && t.GetInterfaces()
                                    .Any(i => i.IsGenericType
                                              && i.GetGenericTypeDefinition() == interfaceType))
                    .ToList();
                }

                // Loop through each type found and register the relevant services
                foreach (var type in types)
                {
                    var interfaceInstance = type.GetInterfaces()
                    .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == interfaceType);

                    if (interfaceInstance != null)
                    {
                        var genericInterfaceType = interfaceInstance.GetGenericTypeDefinition();
                        var serviceType = genericInterfaceType.MakeGenericType(interfaceInstance.GetGenericArguments());
                        services.AddScoped(interfaceInstance, type);
                    }
                }
            }
        }
    }
}

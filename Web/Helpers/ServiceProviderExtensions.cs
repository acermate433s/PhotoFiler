using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

using Microsoft.Extensions.DependencyInjection;

namespace PhotoFiler.Web.Helpers
{
    public static class ServiceProviderExtensions
    {
        public static IServiceCollection AddControllersAsServices(this IServiceCollection services, IEnumerable<Type> controllerTypes)
        {
            foreach (var type in controllerTypes)
            {
                services.AddTransient(type);
            }

            return services;
        }

        public static IServiceCollection AddControllersAsServices(this IServiceCollection services)
        {
            return services.AddControllersAsServices(Assembly.GetExecutingAssembly()
                .GetExportedTypes()
                .Where(t => !t.IsAbstract && !t.IsGenericTypeDefinition)
                .Where(t => typeof(IController).IsAssignableFrom(t)
                    || t.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase)));
        }

        public static IServiceCollection AddControllersAsServices(this IServiceCollection services, Assembly assembly)
        {
            return services.AddControllersAsServices(assembly.GetExportedTypes()
                .Where(t => !t.IsAbstract && !t.IsGenericTypeDefinition)
                .Where(t => typeof(IController).IsAssignableFrom(t)
                    || t.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase)));
        }
    }
}
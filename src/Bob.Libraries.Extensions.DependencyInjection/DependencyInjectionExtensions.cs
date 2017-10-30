using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            services.AddSingleton<ITypeFinder, TypeFinder>();
            var finder = new TypeFinder();

            AddTypes(services, finder, typeof(ISingletonDependency), DependencyLifeStyle.Singleton);
            AddTypes(services, finder, typeof(IScopedTransientDependency), DependencyLifeStyle.Scoped);
            AddTypes(services, finder, typeof(ITransientDependency), DependencyLifeStyle.Transient);

            IocManager.ServiceProvider = services.BuildServiceProvider();
            return services;
        }

        private static void AddTypes(IServiceCollection services, TypeFinder finder, Type findType, DependencyLifeStyle life)
        {
            var a = FindClassesOfType(findType, finder);
            foreach (var type in a)
            {
                var implementedInterfaces = type.GetTypeInfo().ImplementedInterfaces.Where(t => t != findType).ToList();
                if (implementedInterfaces.Any())
                {
                    foreach (var implementedInterface in implementedInterfaces)
                    {
                        switch (life)
                        {
                            case DependencyLifeStyle.Transient:
                                services.AddTransient(implementedInterface, type);
                                break;
                            case DependencyLifeStyle.Scoped:
                                services.AddScoped(implementedInterface, type);
                                break;
                            case DependencyLifeStyle.Singleton:
                                services.AddSingleton(implementedInterface, type);
                                break;
                            default:
                                services.AddTransient(implementedInterface, type);
                                break;
                        }
                    }
                }
                else
                {
                    switch (life)
                    {
                        case DependencyLifeStyle.Transient:
                            services.AddTransient(type, type);
                            break;
                        case DependencyLifeStyle.Scoped:
                            services.AddScoped(type, type);
                            break;
                        case DependencyLifeStyle.Singleton:
                            services.AddSingleton(type, type);
                            break;
                        default:
                            services.AddTransient(type, type);
                            break;
                    }
                }
            }
        }

        private static Type[] FindClassesOfType(this Type type, TypeFinder finder)
        {
            return finder.Find(t =>
            (t.GetTypeInfo().IsClass && !t.GetTypeInfo().IsAbstract && type.IsAssignableFrom(t)) ||
            (type.GetTypeInfo().IsGenericTypeDefinition && DoesTypeImplementOpenGeneric(t, type)));
        }

        /// <summary>
        /// Does type implement generic?
        /// </summary>
        /// <param name="type"></param>
        /// <param name="openGeneric"></param>
        /// <returns></returns>
        private static bool DoesTypeImplementOpenGeneric(Type type, Type openGeneric)
        {
            try
            {
                var genericTypeDefinition = openGeneric.GetGenericTypeDefinition();
                foreach (var implementedInterface in type.GetTypeInfo().FindInterfaces((objType, objCriteria) => true, null))
                {
                    if (!implementedInterface.GetTypeInfo().IsGenericType)
                        continue;

                    var isMatch = genericTypeDefinition.IsAssignableFrom(implementedInterface.GetGenericTypeDefinition());
                    return isMatch;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}

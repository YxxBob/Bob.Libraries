using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Reflection;

namespace AutoMapper
{
    public static class AutoMapperExtensions
    {
        private static volatile bool _createdMappingsBefore;
        private static readonly object SyncObj = new object();

        public static void AddAutoMapper(this IServiceCollection services)
        {
            CreateMappings();
        }

        private static void CreateMappings()
        {
            lock (SyncObj)
            {
                Action<IMapperConfigurationExpression> configurer = FindAndAutoMapTypes;

                //We should prevent duplicate mapping in an application, since Mapper is static.
                if (!_createdMappingsBefore)
                {
                    Mapper.Initialize(configurer);
                    _createdMappingsBefore = true;
                }
            }
        }

        private static void FindAndAutoMapTypes(IMapperConfigurationExpression configuration)
        {
            var types = IocManager.ServiceProvider.GetService<ITypeFinder>()
                .Find(type =>
                {
                    var typeInfo = type.GetTypeInfo();
                    return typeInfo.IsDefined(typeof(AutoMapAttribute)) ||
                           typeInfo.IsDefined(typeof(AutoMapFromAttribute)) ||
                           typeInfo.IsDefined(typeof(AutoMapToAttribute));
                }
            );

            foreach (var type in types)
            {
                configuration.CreateAutoAttributeMaps(type);
            }
        }

    }
}

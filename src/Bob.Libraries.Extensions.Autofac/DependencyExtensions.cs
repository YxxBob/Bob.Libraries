using System.Reflection;

namespace Autofac
{
    public static class DependencyExtensions
    {
        public static void Register(this ContainerBuilder builder,Assembly assembly)
        {
            var transientClass = new TypeFinder().FindClassesOfType<ITransientDependency>(assembly, true);
            foreach (var c in transientClass)
            {
                builder.RegisterType(c)
                    .AsImplementedInterfaces().AsSelf()
                    .InstancePerLifetimeScope();
            }
            var singletonClass = new TypeFinder().FindClassesOfType<ISingletonDependency>(assembly, true);
            foreach (var c in singletonClass)
            {
                builder.RegisterType(c)
                    .AsImplementedInterfaces().AsSelf()
                    .SingleInstance();
            }
        }
    }
}
using System;

namespace Microsoft.Extensions.DependencyInjection.Reflection
{
    public interface ITypeFinder
    {
        Type[] Find(Func<Type, bool> predicate);

        Type[] FindAll();
    }
}
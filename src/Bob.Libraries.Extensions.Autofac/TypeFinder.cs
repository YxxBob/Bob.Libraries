using System;
using System.Collections.Generic;
using System.Reflection;

namespace Autofac
{
    public class TypeFinder
    {
        #region Fields

        private readonly bool _ignoreReflectionErrors = true;

        #endregion

        #region Methods
        
        //public IEnumerable<Type> FindClassesOfType<T>(IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true)
        //{
        //    return FindClassesOfType(typeof(T), assemblies, onlyConcreteClasses);
        //}

        //public IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true)
        //{
        //    var result = new List<Type>();
        //    try
        //    {
        //        foreach (var a in assemblies)
        //        {
        //            result.AddRange(FindClassesOfType(assignTypeFrom,a, onlyConcreteClasses));
        //        }
        //    }
        //    catch (ReflectionTypeLoadException ex)
        //    {
        //        var msg = string.Empty;
        //        foreach (var e in ex.LoaderExceptions)
        //            msg += e.Message + Environment.NewLine;

        //        var fail = new Exception(msg, ex);
        //        Debug.WriteLine(fail.Message, fail);

        //        throw fail;
        //    }
        //    return result;
        //}

        public IEnumerable<Type> FindClassesOfType<T>(Assembly assembly, bool onlyConcreteClasses)
        {
            return FindClassesOfType(typeof(T), assembly, onlyConcreteClasses);
        }

        public IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, Assembly assembly, bool onlyConcreteClasses)
        {
            Type[] types = null;
            var result=new List<Type>();
            try
            {
                types = assembly.GetTypes();
            }
            catch
            {
                //Entity Framework 6 doesn't allow getting types (throws an exception)
                if (!_ignoreReflectionErrors)
                {
                    throw;
                }
            }
            if (types != null)
            {
                foreach (var t in types)
                {
                    if (assignTypeFrom.IsAssignableFrom(t) || (assignTypeFrom.GetTypeInfo().IsGenericTypeDefinition &&
                                                               DoesTypeImplementOpenGeneric(t, assignTypeFrom)))
                    {
                        var typeInfo = t.GetTypeInfo();
                        if (!typeInfo.IsInterface)
                        {
                            if (onlyConcreteClasses)
                            {
                                if (typeInfo.IsClass && !typeInfo.IsAbstract)
                                {
                                    result.Add(t);
                                }
                            }
                            else
                            {
                                result.Add(t);
                            }
                        }
                    }
                }
            }
            return result;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Does type implement generic?
        /// </summary>
        /// <param name="type"></param>
        /// <param name="openGeneric"></param>
        /// <returns></returns>
        protected virtual bool DoesTypeImplementOpenGeneric(Type type, Type openGeneric)
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

        #endregion
    }
}
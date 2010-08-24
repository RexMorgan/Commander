using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FubuCore;

namespace Commander
{
    internal static class CoreExtensions
    {
        public static object GetDefaultInstance(this Type type)
        {
            return Activator.CreateInstance(type);
        }

        public static IEnumerable<PropertyInfo> GetPublicProperties(this Type type)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }

        public static bool IsInNamespace(this Type type, string nameSpace)
        {
            return type.Namespace.StartsWith(nameSpace);
        }

        public static Type FindFirstInterfaceThatCloses(this Type pluggedType, Type templateType)
        {
            return pluggedType.FindInterfacesThatClose(templateType).FirstOrDefault();
        }

        public static IEnumerable<Type> FindInterfacesThatClose(this Type pluggedType, Type templateType)
        {
            return rawFindInterfacesThatCloses(pluggedType, templateType).Distinct();
        }

        private static IEnumerable<Type> rawFindInterfacesThatCloses(Type pluggedType, Type templateType)
        {
            if (!pluggedType.IsConcrete()) yield break;

            if (templateType.IsInterface)
            {
                foreach (var interfaceType in pluggedType.GetInterfaces().Where(type => type.IsGenericType && (type.GetGenericTypeDefinition() == templateType)))
                {
                    yield return interfaceType;
                }
            }
            else if (pluggedType.BaseType.IsGenericType && (pluggedType.BaseType.GetGenericTypeDefinition() == templateType))
            {
                yield return pluggedType.BaseType;
            }

            if (pluggedType.BaseType == typeof(object)) yield break;

            foreach (var interfaceType in rawFindInterfacesThatCloses(pluggedType.BaseType, templateType))
            {
                yield return interfaceType;
            }
        }

        public static bool ImplementsInterfaceTemplate(this Type pluggedType, Type templateType)
        {
            if (!pluggedType.IsConcrete()) return false;

            foreach (Type interfaceType in pluggedType.GetInterfaces())
            {
                if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == templateType)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
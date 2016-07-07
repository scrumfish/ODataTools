using System;
using System.Collections.Generic;
using Scrumfish.OData.Objects;

namespace Scrumfish.OData.Client.Factories
{
    internal static class StringifierFactory
    {

        private static readonly Dictionary<Type, Type> ProvidedLibraries = GetProviderLibaries();

        private static Dictionary<Type, Type> GetProviderLibaries()
        {
            var result = new Dictionary<Type, Type>();
            var providers = TypeFactoryConfiguration.GetConfiguration();
            for (var i = 0; i < providers.Stringifiers.Count; i++)
            {
                var dataType = Type.GetType(providers.Stringifiers[i].TypeName);
                var stringifierType = Type.GetType(providers.Stringifiers[i].StringifierName);
                result.Add(dataType, stringifierType);
            }
            return result;
        }

        public static Stringifier GetStringifier(object target)
        {
            var library = FindLibrary(target.GetType());
            if (library == null) return null;
            var result = Activator.CreateInstance(library, target) as Stringifier;
            return result;
        }

        private static Type FindLibrary(Type type)
        {
            if (type == null) return null;
            Type library;
            ProvidedLibraries.TryGetValue(Type.GetType(type.AssemblyQualifiedName), out library);
            return library ?? FindLibrary(type.BaseType);
        }
    }
}

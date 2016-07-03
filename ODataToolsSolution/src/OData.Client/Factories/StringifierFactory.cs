using System;
using System.Collections.Generic;
using Scrumfish.OData.Objects;

namespace Scrumfish.OData.Client.Factories
{
    internal static class StringifierFactory
    {

        private static Dictionary<string, string> _providedLibraries = null;

        public static Stringifier GetStringifier(object target)
        {
            var library = FindLibrary(target.GetType());
            if (library == null) return null;
            var type = Type.GetType(library);
            if (type == null) return null;
            var result = Activator.CreateInstance(type, target) as Stringifier;
            return result;
        }

        private static string FindLibrary(Type type)
        {
            if (type == null) return null;
            string library;
            _providedLibraries.TryGetValue(type.AssemblyQualifiedName, out library);
            return library ?? FindLibrary(type.BaseType);
        }
    }
}

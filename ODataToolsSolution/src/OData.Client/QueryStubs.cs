using System;
using System.Linq.Expressions;

namespace Scrumfish.OData.Client
{
    public static class QueryStubs
    {
        public static object WithDependency<T>(this object target, Expression<Func<T, object>> dependency) 
            where T : class
        {
            return target;
        }

        public static string Not(this string target, string searchTerm)
        {
            return target;
        }

        public static string Match(this string target, string searchTerm)
        {
            return target;
        }

        public static string And(this string target, string searchTerm)
        {
            return target;
        }

        public static string And(this string target)
        {
            return target;
        }

        public static string Or(this string target, string searchTerm)
        {
            return target;
        }

        public static string Or(this string target)
        {
            return target;
        }

        public static string Group(this string target, Expression<Func<string, string>> searchTerm)
        {
            return target;
        }

        public static T AsAlias<T>(this string target)
        {
            return default(T);
        }
    }
}
using System;
using System.Linq;
using System.Linq.Expressions;
using Scrumfish.OData.Client.Common;

namespace Scrumfish.OData.Client.v4
{
    public static class Query
    {
        private static string _desc = " desc";

        public static ODataQuery<T> Filter<T>(this ODataQuery<T> target, Expression<Func<T, object>> action) where T : class
        {
            return target.AppendOperation("$filter")
                 .AppendExpression(action
                     .ParseExpression());
        }

        public static ODataQuery<T> Top<T>(this ODataQuery<T> target, int topNum)
        {
            return target.AppendOperation("$top")
                .AppendExpression(topNum);
        }

        public static ODataQuery<T> Skip<T>(this ODataQuery<T> target, int skipNum)
        {
            return target.AppendOperation("$skip")
                .AppendExpression(skipNum);
        }

        public static ODataQuery<T> OrderBy<T>(this ODataQuery<T> target, Expression<Func<T, object>> action) where T : class
        {
            return target.AppendOperation("$orderBy")
                .AppendExpression(action
                     .ParseExpression());
        }

        public static ODataQuery<T> OrderByDesc<T>(this ODataQuery<T> target, Expression<Func<T, object>> action) where T : class
        {
            return target.AppendOperation("$orderBy")
                .AppendExpression(action
                    .ParseExpression())
                    .AppendModifier(_desc);
        }

        public static ODataQuery<T> ThenBy<T>(this ODataQuery<T> target, Expression<Func<T, object>> action) where T : class
        {
            return target.AssertCurrentOperation("$orderBy")
                .AppendChainingExpression(action
                    .ParseExpression());
        }

        public static ODataQuery<T> ThenByDesc<T>(this ODataQuery<T> target, Expression<Func<T, object>> action) where T : class
        {
            return target.AssertCurrentOperation("$orderBy")
                .AppendChainingExpression(action
                    .ParseExpression())
                    .AppendModifier(_desc);
        }

        public static ODataQuery<T> Count<T>(this ODataQuery<T> target, bool returnCount)
        {
            return target.AppendOperation("$count")
                .AppendExpression(returnCount ? "true" : "false");
        }

        public static ODataQuery<T> Select<T>(this ODataQuery<T> target, params Expression<Func<T, object>>[] actions) where T : class
        {
            var expressions = actions.Select(a => a.ParseExpression());
            return target.AppendOperation("$select")
                .AppendExpression(string.Join(",", expressions));
        }

        public static ODataQuery<T> SelectAll<T>(this ODataQuery<T> target) where T : class
        {
            return target.AppendOperation("$select")
                 .AppendExpression("*");
        }

        public static ODataQuery<T> Expand<T>(this ODataQuery<T> target, Expression<Func<T, object>> action) where T : class
        {
            return target.AppendOperation("$expand")
                 .AppendExpression(action.ParseExpression());
        }

        public static ODataQuery<T> Expand<T,TY>(this ODataQuery<T> target, Expression<Func<T, object>> action, ODataQuery<TY> subquery)
        {
            return target.AppendOperation("$expand")
                .AppendExpression(action
                    .ParseExpression())
                .StartSubQuery()
                .AppendQuery(subquery)
                .EndSubQuery();
        }

        public static ODataQuery<T> Search<T>(this ODataQuery<T> target, Expression<Func<string, string>> searchTerm)
        {
            return target.AppendOperation("$search")
                .AppendExpression(searchTerm.ParseExpression());
        }


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

    }
}


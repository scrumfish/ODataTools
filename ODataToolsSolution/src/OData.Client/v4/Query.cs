using System;
using System.Linq.Expressions;
using Scrumfish.OData.Client.Common;

namespace Scrumfish.OData.Client.v4
{
    public static class Query
    {

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

        public static ODataQuery<T> Count<T>(this ODataQuery<T> target)
        {
            return target.AssertNotInQuery()
                .AppendUriElement("$count");
        }

        private static string _desc = " desc";
    }
}


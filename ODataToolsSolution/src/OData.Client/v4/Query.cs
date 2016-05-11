using System;
using System.Linq.Expressions;
using Scrumfish.OData.Client.Common;

namespace Scrumfish.OData.Client.v4
{
    public static class Query
    {

        public static string Filter<T>(this string target, Expression<Func<T, object>> action) where T : class
        {
            var filter = target.GetStartQuery()
                .Append("$filter=")
                .Append(action.GetLambdaBody()
                    .ParseExpression());
            return filter.ToString();
        }
    }
}

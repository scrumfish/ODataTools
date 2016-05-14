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

        public static string Top(this string target, int topNum)
        {
            var top = target.GetStartQuery()
               .AppendFormat("$top={0}", topNum);

            return top.ToString();
        }

        public static string Skip(this string target, int skipNum)
        {
            var skip = target.GetStartQuery()
               .AppendFormat("$skip={0}", skipNum);

            return skip.ToString();
        }
    }
}

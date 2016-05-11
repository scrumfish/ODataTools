using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Scrumfish.OData.Client.Common
{
    internal static class ParserExtensions
    {
        public static StringBuilder GetStartQuery(this string target)
        {
            var query = new StringBuilder(target);

            if (!target.EndsWith("?"))
            {
                query.Append('&');
            }
            return query;
        }

        public static UnaryExpression GetLambdaBody<TParam,TResult>(this Expression<Func<TParam,TResult>> expression)
        {
            UnaryExpression result = null;
            if (expression.NodeType == ExpressionType.Lambda)
            {
                result = expression.Body as UnaryExpression;
            }
            if (result == null)
            {
                throw new InvalidExpressionException("Expression is not a lambda expression.");
            }
            return result;
        }

        public static string AsOperator(this ExpressionType type)
        {
            
        }

        public static 

        public static string ParseExpression(this Expression expression)
        {
            new StringBuilder();
            if (expression.NodeType == ExpressionType.Convert)
            {
                var unaryExpression = expression as UnaryExpression;
                if (unaryExpression != null)
                {
                    return unaryExpression.Operand.ParseExpression();
                }
            }
            if (expression.NodeType.IsLogicalOperator())
            {
                var logicalExpression = expression as BinaryExpression;
                if (logicalExpression == null)
                {
                    throw new InvalidExpressionException("Could not find logical expression to parse.");
                }
                return new StringBuilder()
                    .Append(logicalExpression.Left.ParseExpression())
                    .Append(logicalExpression.NodeType.AsOperator())
                    .Append(logicalExpression.Right.ParseExpression())
                    .ToString();
            }
            return string.Empty;
        }
        
    }
}

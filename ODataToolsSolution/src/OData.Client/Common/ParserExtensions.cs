using System;
using System.Linq.Expressions;
using System.Text;

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

        public static UnaryExpression GetLambdaBody<TParam, TResult>(this Expression<Func<TParam, TResult>> expression)
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
            switch (type)
            {
                case ExpressionType.Equal:
                    return " eq ";
                case ExpressionType.NotEqual:
                    return " ne ";
                case ExpressionType.GreaterThan:
                    return " gt ";
                case ExpressionType.GreaterThanOrEqual:
                    return " ge ";
                case ExpressionType.LessThan:
                    return " lt ";
                case ExpressionType.LessThanOrEqual:
                    return " le ";
            }
            throw new InvalidExpressionOperatorException("Unknown operator in the expression.");
        }

        public static bool IsLogicalOperator(this ExpressionType expressionType)
        {
            return expressionType == ExpressionType.Equal
                   || expressionType == ExpressionType.GreaterThan
                   || expressionType == ExpressionType.GreaterThanOrEqual
                   || expressionType == ExpressionType.NotEqual
                   || expressionType == ExpressionType.LessThan
                   || expressionType == ExpressionType.LessThanOrEqual;
        }

        public static bool IsMemberAccess(this ExpressionType expressionType)
        {
            return expressionType == ExpressionType.MemberAccess;
        }

        public static bool IsConstant(this ExpressionType expressionType)
        {
            return expressionType == ExpressionType.Constant;
        }

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
            if (expression.NodeType.IsMemberAccess())
            {
                var memberExpression = expression as MemberExpression;
                if (memberExpression == null)
                {
                    throw new InvalidExpressionException("Could not find a member expression to parse.");
                }
                return memberExpression.Member.Name;
            }
            if (expression.NodeType.IsConstant())
            {
                var memeberConstant = expression as ConstantExpression;
                if (memeberConstant == null)
                {
                    throw new InvalidExpressionException("Could not find a constant expression to parse.");
                }
                if (memeberConstant.Type == typeof (Int32))
                {
                    if (memeberConstant.Value != null) return (memeberConstant.Value as int? ?? 0).ToString();
                }
            }
            return string.Empty;
        }

    }
}

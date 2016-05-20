using System;
using System.Collections.Generic;
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

        public static Expression GetLambdaBody<TParam, TResult>(this Expression<Func<TParam, TResult>> expression)
        {
            Expression result = GetValidExpression(expression);

            if (result == null)
            {
                throw new InvalidExpressionException("Expression is not a lambda expression.");
            }
            return result;
        }

        public static Expression GetValidExpression(LambdaExpression expression)
        {
            Expression result = null;
            Expression opEvalResult = null;

            if (expression.NodeType == ExpressionType.Lambda)
            {
                if (expression.Body.NodeType == ExpressionType.Convert)
                {
                    opEvalResult = ((UnaryExpression)expression.Body).Operand as Expression;
                    if (opEvalResult.NodeType.IsLogicalOperator())
                    {
                        result = expression.Body as UnaryExpression;
                    }
                    else
                    {
                        if (opEvalResult.NodeType == ExpressionType.MemberAccess)
                        {
                            result = opEvalResult;
                        }
                    }
                }
                else if (expression.Body.NodeType == ExpressionType.MemberAccess)
                {
                    result = expression.Body as MemberExpression;
                }
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
            if (expression.NodeType == ExpressionType.Convert)
            {
                return expression.ParseUnaryExpression();
            }
            if (expression.NodeType.IsLogicalOperator())
            {
                return expression.ParseLogicalExpression();
            }
            if (expression.NodeType.IsMemberAccess())
            {
                return expression.ParseMemberExpression();
            }
            if (expression.NodeType.IsConstant())
            {
                return expression.ParseConstantExpression();
            }
            return string.Empty;
        }

        private static string ParseConstantExpression(this Expression expression)
        {
            var memberConstant = expression as ConstantExpression;
            if (memberConstant == null)
            {
                throw new InvalidExpressionException("Could not find a constant expression to parse.");
            }
            Func<object, string> convertToString;
            TypeConverter.TryGetValue(memberConstant.Type, out convertToString);
            if (convertToString == null)
            {
                throw new InvalidExpressionException("Unknown data type for member constant expression.");
            }
            return convertToString(memberConstant.Value);
        }

        private static readonly Dictionary<Type, Func<object, string>> TypeConverter = new Dictionary
            <Type, Func<object, string>>
        {
            {typeof (int), (o) => o.NumberOrNull<int>()},
            {typeof (short), (o) => o.NumberOrNull<short>()},
            {typeof (long), (o) => o.NumberOrNull<long>()},
            {typeof (int?), (o) => o.NumberOrNull<int?>()},
            {typeof (short?), (o) => o.NumberOrNull<short?>()},
            {typeof (long?), (o) => o.NumberOrNull<long?>()},
            {typeof (string), (o) => o.StringOrNull()}
        };

        private static string NumberOrNull<T>(this object value)
        {
            return value == null ? "null" : ((T) value).ToString();
        }

        private static string StringOrNull(this object value)
        {
            return value == null ? "null" : $"'{((string)value)}'";
        }

        private static string ParseMemberExpression(this Expression expression)
        {
            var memberExpression = expression as MemberExpression;
            if (memberExpression == null)
            {
                throw new InvalidExpressionException("Could not find a member expression to parse.");
            }
            return memberExpression.Member.Name;
        }

        private static string ParseLogicalExpression(this Expression expression)
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

        private static string ParseUnaryExpression(this Expression expression)
        {
            var unaryExpression = expression as UnaryExpression;
            if (unaryExpression == null)
            {
                throw new InvalidExpressionException("Could not find unary expression to parse.");
            }
            return unaryExpression.Operand.ParseExpression();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Scrumfish.OData.Client.Common
{
    internal static class ParserExtensions
    {
        public static string ParseExpression<TParam, TResult>(this Expression<Func<TParam, TResult>> expression)
        {
            return expression.Body.ParseExpression();
        }

        private static string ParseExpression(this Expression expression)
        {
            if (expression.NodeType.IsUnary())
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
            throw new InvalidExpressionException("Expression is not parsable.");
        }

        private static string AsOperator(this ExpressionType type)
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
                case ExpressionType.AndAlso:
                    return " and ";
                case ExpressionType.OrElse:
                    return " or ";
            }
            throw new InvalidExpressionOperatorException("Unknown operator in the expression.");
        }
        
        private static bool IsUnary(this ExpressionType expressionType)
        {
            return expressionType == ExpressionType.Convert;
        }

        private static bool IsLogicalOperator(this ExpressionType expressionType)
        {
            return expressionType == ExpressionType.Equal
                   || expressionType == ExpressionType.GreaterThan
                   || expressionType == ExpressionType.GreaterThanOrEqual
                   || expressionType == ExpressionType.NotEqual
                   || expressionType == ExpressionType.LessThan
                   || expressionType == ExpressionType.LessThanOrEqual
                   || expressionType == ExpressionType.AndAlso
                   || expressionType == ExpressionType.OrElse;
        }

        private static bool IsMemberAccess(this ExpressionType expressionType)
        {
            return expressionType == ExpressionType.MemberAccess;
        }

        private static bool IsConstant(this ExpressionType expressionType)
        {
            return expressionType == ExpressionType.Constant;
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
            return value == null ? "null" : ((T)value).ToString();
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
                .Append('(')
                .Append(logicalExpression.Left.ParseExpression())
                .Append(logicalExpression.NodeType.AsOperator())
                .Append(logicalExpression.Right.ParseExpression())
                .Append(')')
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

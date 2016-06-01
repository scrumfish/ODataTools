using System;
using System.Collections.Generic;
using System.Linq;
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
            Func<Expression, string> method;
            ExpressionParsers.TryGetValue(expression.NodeType, out method);
            if (method == null)
            {
                throw new InvalidExpressionException("Expression is not parsable.");
            }
            return method(expression);
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

        private static string ParseCallExpression(this Expression expression)
        {
            var callExpression = expression as MethodCallExpression;
            if (callExpression == null)
            {
                throw new InvalidExpressionException("Could not find a call expression to parse.");
            }
            MethodCall method;
            Methods.TryGetValue(callExpression.Method.Name, out method);
            if (method == null)
            {
                throw new InvalidExpressionException($"Unknown method {callExpression.Method.Name}.");
            }
            var target = callExpression.Object.ParseMemberExpression();
            var parameters = callExpression.Arguments.Select(a => a.ParseExpression()).ToList();
            return BuildMethodCall(method.Name, method.Order, target, parameters);
        }

        private static string BuildMethodCall(string method, ParameterOrder order, string target, List<string> parameters)
        {
            return new StringBuilder()
                .Append(method)
                .Append('(')
                .AppendParameters(order, target, parameters)
                .Append(')')
                .ToString();
        }

        private static StringBuilder AppendParameters(this StringBuilder result, ParameterOrder order, string target, List<string> parameters)
        {
            if (order == ParameterOrder.TargetFirst)
            {
                result.Append(target);
                if (parameters.Any())
                {
                    result.Append(',')
                        .Append(string.Join(",", parameters));
                }
            }
            else
            {
                if (parameters.Any())
                {
                    result.Append(string.Join(",", parameters))
                        .Append(',');
                }
                result.Append(target);
            }
            return result;
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

        private static string NumberOrNull<T>(this object value)
        {
            return value == null ? "null" : ((T)value).ToString();
        }

        private static string StringOrNull(this object value)
        {
            return value == null ? "null" : $"'{value}'";
        }

        private static string ParseMemberExpression(this Expression expression)
        {
            var memberExpression = expression as MemberExpression;
            if (memberExpression == null)
            {
                throw new InvalidExpressionException("Could not find a member expression to parse.");
            }
            MethodCall method;
            Methods.TryGetValue(memberExpression.Member.Name, out method);
            if (method?.SupportedTypes != null && method.SupportedTypes.Contains(memberExpression.Expression.Type))
            {
                return memberExpression.ParseCallExpressionWithKnownProperty();
            }
            return memberExpression.Member.Name;
        }

        private static string ParseCallExpressionWithKnownProperty(this MemberExpression expression)
        {
            MethodCall method;
            Methods.TryGetValue(expression.Member.Name, out method);
            if (method == null)
            {
                throw new InvalidExpressionException($"The method {expression.Member.Name} could not be mapped.");
            }

            return new StringBuilder()
                .Append(method.Name)
                .Append('(')
                .Append(expression.Expression.ParseExpression())
                .Append(')')
                .ToString();
        }

        private static string ParseLogicalExpression(this Expression expression)
        {
            var logicalExpression = expression as BinaryExpression;
            if (logicalExpression == null)
            {
                throw new InvalidExpressionException("Could not find logical expression to parse.");
            }

            var rightOverride = logicalExpression.Left.GetRightOperandOverride();

            return new StringBuilder()
                .Append('(')
                .Append(logicalExpression.Left.ParseExpression())
                .Append(logicalExpression.NodeType.AsOperator())
                .Append(rightOverride(logicalExpression.Right.ParseExpression()))
                .Append(')')
                .ToString();
        }

        private static Func<string, string> GetRightOperandOverride(this Expression expression)
        {
            var methodExpression = expression as MemberExpression;
            if (methodExpression == null) return s => s;
            MethodCall method;
            Methods.TryGetValue(methodExpression.Member.Name, out method);
            if (method?.RightOperandConverter != null)
            {
                return method.RightOperandConverter;
            }
            return s => s;
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

        private static string ParseAddExpression(this Expression expression)
        {
            var binaryExpression = expression as BinaryExpression;
            if (binaryExpression == null)
            {
                throw new InvalidExpressionException("Could not find binary expression to parse.");
            }
            MethodCall method;
            Methods.TryGetValue(binaryExpression.Method.Name, out method);
            if (method == null)
            {
                throw new InvalidExpressionException($"The method {binaryExpression.Method.Name} could not be mapped.");
            }
            return new StringBuilder()
                .Append(method.Name)
                .Append('(')
                .Append(binaryExpression.Left.ParseExpression())
                .Append(',')
                .Append(binaryExpression.Right.ParseExpression())
                .Append(')')
                .ToString();
        }

        private static string ConvertToFractionalSeconds(string value)
        {
            int milliseconds;
            if (int.TryParse(value, out milliseconds))
            {
                decimal fractional = milliseconds;
                fractional /= 1000;
                return fractional.ToString().TrimStart('0');
            }
            return value;
        }

        private enum ParameterOrder
        {
            TargetFirst,
            TargetLast
        }

        private class MethodCall
        {
            public string Name { get; set; }
            public ParameterOrder Order { get; set; }
            public IList<Type> SupportedTypes { get; set; }
            public Func<string,string> RightOperandConverter { get; set; } 
        }

        private static readonly Dictionary<ExpressionType, Func<Expression, string>> ExpressionParsers = new Dictionary
            <ExpressionType, Func<Expression, string>>
        {
            {ExpressionType.Convert, ParseUnaryExpression},
            {ExpressionType.Equal, ParseLogicalExpression},
            {ExpressionType.GreaterThan, ParseLogicalExpression},
            {ExpressionType.GreaterThanOrEqual, ParseLogicalExpression},
            {ExpressionType.NotEqual, ParseLogicalExpression},
            {ExpressionType.LessThan, ParseLogicalExpression},
            {ExpressionType.LessThanOrEqual, ParseLogicalExpression},
            {ExpressionType.AndAlso, ParseLogicalExpression},
            {ExpressionType.OrElse, ParseLogicalExpression},
            {ExpressionType.MemberAccess, ParseMemberExpression},
            {ExpressionType.Constant, ParseConstantExpression},
            {ExpressionType.Call, ParseCallExpression},
            {ExpressionType.Add, ParseAddExpression},
        };

        private static readonly Dictionary<Type, Func<object, string>> TypeConverter = new Dictionary
            <Type, Func<object, string>>
        {
            {typeof (int), (o) => o.NumberOrNull<int>()},
            {typeof (short), (o) => o.NumberOrNull<short>()},
            {typeof (long), (o) => o.NumberOrNull<long>()},
            {typeof (int?), (o) => o.NumberOrNull<int?>()},
            {typeof (short?), (o) => o.NumberOrNull<short?>()},
            {typeof (long?), (o) => o.NumberOrNull<long?>()},
            {typeof (string), (o) => o.StringOrNull()},
            {typeof (char), (o) =>  o.StringOrNull() },
            {typeof (char?), (o) =>  o.StringOrNull() }
        };

        private static readonly Dictionary<string, MethodCall> Methods = new Dictionary<string, MethodCall>
        {
            {"EndsWith", new MethodCall {Name = "endswith", Order = ParameterOrder.TargetFirst}},
            {"StartsWith", new MethodCall {Name = "startswith", Order = ParameterOrder.TargetFirst}},
            {"Contains", new MethodCall {Name = "substringof", Order = ParameterOrder.TargetLast}},
            {"Substring", new MethodCall {Name = "substring", Order = ParameterOrder.TargetFirst}},
            {"Length", new MethodCall {Name = "length", Order = ParameterOrder.TargetFirst, SupportedTypes = new List<Type> {typeof(string)}}},
            {"IndexOf", new MethodCall {Name = "indexof", Order = ParameterOrder.TargetFirst}},
            {"Replace", new MethodCall {Name = "replace", Order = ParameterOrder.TargetFirst}},
            {"ToLower", new MethodCall {Name = "tolower", Order = ParameterOrder.TargetFirst}},
            {"ToUpper", new MethodCall {Name = "toupper", Order = ParameterOrder.TargetFirst}},
            {"Trim", new MethodCall {Name = "trim", Order = ParameterOrder.TargetFirst}},
            {"Concat", new MethodCall {Name = "concat", Order = ParameterOrder.TargetFirst}},
            {"Day", new MethodCall {Name = "day", Order = ParameterOrder.TargetFirst, SupportedTypes = new List<Type> {typeof(DateTime),typeof(DateTime?),typeof(DateTimeOffset), typeof(DateTimeOffset?)}}},
            {"Month", new MethodCall {Name = "month", Order = ParameterOrder.TargetFirst, SupportedTypes = new List<Type> {typeof(DateTime),typeof(DateTime?),typeof(DateTimeOffset), typeof(DateTimeOffset?)}}},
            {"Year", new MethodCall {Name = "year", Order = ParameterOrder.TargetFirst, SupportedTypes = new List<Type> {typeof(DateTime),typeof(DateTime?),typeof(DateTimeOffset), typeof(DateTimeOffset?)}}},
            {"Hour", new MethodCall {Name = "hour", Order = ParameterOrder.TargetFirst, SupportedTypes = new List<Type> {typeof(DateTime),typeof(DateTime?),typeof(DateTimeOffset), typeof(DateTimeOffset?)}}},
            {"Minute", new MethodCall {Name = "minute", Order = ParameterOrder.TargetFirst, SupportedTypes = new List<Type> {typeof(DateTime),typeof(DateTime?),typeof(DateTimeOffset), typeof(DateTimeOffset?)}}},
            {"Second", new MethodCall {Name = "second", Order = ParameterOrder.TargetFirst, SupportedTypes = new List<Type> {typeof(DateTime),typeof(DateTime?),typeof(DateTimeOffset), typeof(DateTimeOffset?)}}},
            {"Millisecond", new MethodCall {Name = "fractionalseconds", Order = ParameterOrder.TargetFirst, SupportedTypes = new List<Type> {typeof(DateTime),typeof(DateTime?),typeof(DateTimeOffset), typeof(DateTimeOffset?)}, RightOperandConverter = (s) => ConvertToFractionalSeconds(s)}},
            {"Round", new MethodCall {Name = "round", Order = ParameterOrder.TargetFirst, SupportedTypes = new List<Type> {typeof(decimal),typeof(decimal?)}}},
            {"Floor", new MethodCall {Name = "floor", Order = ParameterOrder.TargetFirst, SupportedTypes = new List<Type> {typeof(decimal),typeof(decimal?)}}},
            {"Ceiling", new MethodCall {Name = "ceiling", Order = ParameterOrder.TargetFirst, SupportedTypes = new List<Type> {typeof(decimal),typeof(decimal?)}}},
        };

    }
}

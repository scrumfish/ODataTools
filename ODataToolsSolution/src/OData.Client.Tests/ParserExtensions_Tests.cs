using System;
using System.Linq.Expressions;
using System.Text;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scrumfish.OData.Client.Tests.TestObjects;

namespace Scrumfish.OData.Client.Tests
{

    [TestClass]
    public class ParserExtensions_Tests
    {
        private MethodInfo _getStartQuery;
        private MethodInfo _getLambdaBody;
        private MethodInfo _asOperator;

        [TestInitialize]
        public void SetUp()
        {
            var type = Type.GetType("Scrumfish.OData.Client.Common.ParserExtensions, Scrumfish.OData.Client");
            Assert.IsNotNull(type);
            _getStartQuery = type.GetMethod("GetStartQuery", BindingFlags.Public | BindingFlags.Static);
            _getLambdaBody = type.GetMethod("GetLambdaBody", BindingFlags.Public | BindingFlags.Static);
            _asOperator = type.GetMethod("AsOperator", BindingFlags.Public | BindingFlags.Static);
        }

        private StringBuilder GetStartQuery(string target)
        {
            return _getStartQuery.Invoke(null, new[] {target}) as StringBuilder;
        }

        private UnaryExpression GetLambdaBody<TParam, TResult>(Expression<Func<TParam, TResult>> expression)
        {
            var method = _getLambdaBody.MakeGenericMethod(typeof (TParam), typeof (TResult));
            return method.Invoke(null, new[] {expression}) as UnaryExpression;
        }

        private string AsOperator(ExpressionType type)
        {
            object o = type;
            return _asOperator.Invoke(null, new[] {o}) as string;
        }

        [TestMethod]
        public void GetStartQuery_ReturnsSameStringIfQuestionMarkEndsString_Test()
        {
            var expected = "yo?";
            var result = GetStartQuery("yo?");
            Assert.AreEqual(expected, result.ToString());
        }

        [TestMethod]
        public void GetStartQuery_ReturnsAmpersandStringIfQuestionMarkEndsString_Test()
        {
            var expected = "yo&";
            var result = GetStartQuery("yo");
            Assert.AreEqual(expected, result.ToString());
        }

        [TestMethod]
        public void GetLambdaBody_ReturnsExpressionBodyForUnaryExpression_Test()
        {
            var result = GetLambdaBody<Person, object>(p => p.Age == 42);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AsOperator_ReturnsEqual_Test()
        {
            Assert.AreEqual(" eq ", AsOperator(ExpressionType.Equal));
        }

        [TestMethod]
        public void AsOperator_ReturnsNotEqual_Test()
        {
            Assert.AreEqual(" ne ", AsOperator(ExpressionType.NotEqual));
        }

        [TestMethod]
        public void AsOperator_ReturnsGreaterThan_Test()
        {
            Assert.AreEqual(" gt ", AsOperator(ExpressionType.GreaterThan));
        }

        [TestMethod]
        public void AsOperator_ReturnsGreaterThanOrEqual_Test()
        {
            Assert.AreEqual(" ge ", AsOperator(ExpressionType.GreaterThanOrEqual));
        }

        [TestMethod]
        public void AsOperator_ReturnsLessThan_Test()
        {
            Assert.AreEqual(" lt ", AsOperator(ExpressionType.LessThan));

        }

        [TestMethod]
        public void AsOperator_ReturnsLessThanOrEqual_Test()
        {
            Assert.AreEqual(" le ", AsOperator(ExpressionType.LessThanOrEqual));
        }
    }
}
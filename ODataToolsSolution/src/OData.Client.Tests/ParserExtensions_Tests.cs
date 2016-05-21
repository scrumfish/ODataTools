using System;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Scrumfish.OData.Client.Tests
{

    [TestClass]
    public class ParserExtensions_Tests
    {
        private MethodInfo _asOperator;

        [TestInitialize]
        public void SetUp()
        {
            var type = Type.GetType("Scrumfish.OData.Client.Common.ParserExtensions, Scrumfish.OData.Client");
            Assert.IsNotNull(type);
            _asOperator = type.GetMethod("AsOperator", BindingFlags.NonPublic | BindingFlags.Static);
        }
        
        private string AsOperator(ExpressionType type)
        {
            object o = type;
            return _asOperator.Invoke(null, new[] {o}) as string;
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

        [TestMethod]
        public void AsOperator_ReturnsAnd_Test()
        {
            Assert.AreEqual(" and ", AsOperator(ExpressionType.AndAlso));
        }
    }
}
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scrumfish.OData.Client.Common;
using Scrumfish.OData.Client.Tests.TestObjects;
using Scrumfish.OData.Client.v4;

namespace Scrumfish.OData.Client.Tests.v4
{
    [TestClass]
    public class Query_OrderBy_Tests
    {
        [TestMethod]
        public void OrderBy_AddsExpectedStringParameterName_Test()
        {
            var expected = "?$orderBy=";
            var result = "?".CreateODataQuery<Person>()
                .OrderBy(p => p.LastName)
                .ToString();
            Assert.IsTrue(result.StartsWith(expected));
        }

        [TestMethod]
        public void OrderBy_AddsExpectedIntParameter_Test()
        {
            var expected = "?$orderBy=";
            var result = "?".CreateODataQuery<Person>()
                .OrderBy(p => p.Age)
                .ToString();
            Assert.IsTrue(result.StartsWith(expected));
        }

        [TestMethod]
        public void OrderBy_AddsExpectedStringParameterNameWithAmpersand_Test()
        {
            var expected = "anyparam&$orderBy=";
            var result = "anyparam".CreateODataQuery<Person>()
                .OrderBy(p => p.LastName)
                .ToString();
            Assert.IsTrue(result.StartsWith(expected));
        }

        [TestMethod]
        public void OrderBy_AddsExpectedIntParameterWithAmpersand_Test()
        {
            var expected = "anyparam&$orderBy=";
            var result = "anyparam".CreateODataQuery<Person>()
                .OrderBy(p => p.Age)
                .ToString();
            Assert.IsTrue(result.StartsWith(expected));
        }

        [TestMethod]
        public void Orderby_Ascending_Test()
        {
            var expected = "?$orderBy=LastName";
            var result = "?".CreateODataQuery<Person>()
                .OrderBy(p => p.LastName)
                .ToString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ThenBy_AppendsToQuery_Test()
        {
            var expected = "?$orderBy=LastName,FirstName";
            var result = "?".CreateODataQuery<Person>()
                .OrderBy(p => p.LastName)
                .ThenBy(p => p.FirstName)
                .ToString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void OrderBy_ThrowsExpectedExceptionWhenCalledMoreThanOnce_Test()
        {
            "?".CreateODataQuery<Person>()
                .OrderBy(p => p.Age)
                .OrderBy(p => p.FirstName);
        }

        [TestMethod]
        public void Orderby_Descending_Test()
        {
            var expected = "?$orderBy=LastName desc";
            var result = "?".CreateODataQuery<Person>()
                .OrderByDesc(p => p.FirstName)
                .ToString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Orderby_AscendingAndDescendingMix_Test()
        {
            var expected = "?$orderBy=LastName,Birthday desc";
            var result = "?".CreateODataQuery<Person>()
                .OrderBy(p => p.FirstName)
                .ThenByDesc(p => p.Birthday)
                .ToString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidExpressionException))]
        public void ThenBy_ThrowsExcpetionIfCallWithoutOrderOperation_Test()
        {
            string.Empty.CreateODataQuery<Person>()
                .ThenBy(p => p.LastName);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidExpressionException))]
        public void ThenBy_ThrowsExcpetionIfCallWithOrderOperationPast_Test()
        {
            "?".CreateODataQuery<Person>()
                .OrderBy(p => p.Birthday)
                .Filter(p => p.Age > 42)
                .ThenBy(p => p.LastName);
        }

    }
}


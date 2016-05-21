using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scrumfish.OData.Client.Common;
using Scrumfish.OData.Client.Tests.TestObjects;
using Scrumfish.OData.Client.v4;

namespace Scrumfish.OData.Client.Tests.v4
{
    [TestClass]
    public class Query_Filter_Tests
    {
        [TestMethod]
        public void Filter_AddsExpectedParameterName_Test()
        {
            var expected = "?$filter=";
            var result = "?".CreateODataQuery<Person>()
                .Filter(p => p.Age > 5)
                .ToString();
            Assert.IsTrue(result.StartsWith(expected));
        }

        [TestMethod]
        public void Filter_AddsExpectedParameterNameWithAmpersand_Test()
        {
            var expected = "hello&$filter=";
            var result = "hello".CreateODataQuery<Person>()
                .Filter(p => p.Age > 5)
                .ToString();
            Assert.IsTrue(result.StartsWith(expected));
        }

        [TestMethod]
        public void Filter_ReturnsIntegerEqualityOperation_Test()
        {
            var expected = "?$filter=(Age eq 5)";
            var result = "?".CreateODataQuery<Person>()
                .Filter(p => p.Age == 5)
                .ToString();
            Assert.AreEqual(expected,result);
        }

        [TestMethod]
        public void Filter_ReturnsNullableLongEqualityOperation_Test()
        {
            var expected = "?$filter=(SomeBigNumber eq 51254411144)";
            var result = "?".CreateODataQuery<Person>()
                .Filter(p => p.SomeBigNumber == 51254411144)
                .ToString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Filter_ReturnsNullableIntegerEqualityOperationNull_Test()
        {
            var expected = "?$filter=(SomeBigNumber eq null)";
            var result = "?".CreateODataQuery<Person>()
                .Filter(p => p.SomeBigNumber == null)
                .ToString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Filter_ReturnsStringEqualityOperation_Test()
        {
            var expected = "?$filter=(FirstName eq 'Steve')";
            var result = "?".CreateODataQuery<Person>()
                .Filter(p => p.FirstName == "Steve")
                .ToString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Filter_ReturnsStringEqualityOperationNull_Test()
        {
            var expected = "?$filter=(FirstName eq null)";
            var result = "?".CreateODataQuery<Person>()
                .Filter(p => p.FirstName == null)
                .ToString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Filter_ReturnsCompundAndExpression_Test()
        {
            var expected = "?$filter=((FirstName eq 'Steve') and (Age gt 18))";
            var result = "?".CreateODataQuery<Person>()
                .Filter(p => p.FirstName == "Steve" && p.Age > 18)
                .ToString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Filter_ReturnsComplexCompundExpression_Test()
        {
            var expected = "?$filter=(((FirstName eq 'Steve') and (Age gt 18)) or (LastName eq 'Smith'))";
            var result = "?".CreateODataQuery<Person>()
                .Filter(p => (p.FirstName == "Steve" && p.Age > 18) || p.LastName == "Smith")
                .ToString();
            Assert.AreEqual(expected, result);
        }
    }
}

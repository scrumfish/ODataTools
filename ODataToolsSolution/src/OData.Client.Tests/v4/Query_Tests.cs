using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scrumfish.OData.Client.Tests.TestObjects;
using Scrumfish.OData.Client.v4;

namespace Scrumfish.OData.Client.Tests.v4
{
    [TestClass]
    public class Query_Tests
    {
        [TestMethod]
        public void Filter_AddsExpectedParameterName_Test()
        {
            var expected = "?$filter=";
            var result = "?".Filter<Person>(p => p.Age > 5);
            Assert.IsTrue(result.StartsWith(expected));
        }

        [TestMethod]
        public void Filter_AddsExpectedParameterNameWithAmpersand_Test()
        {
            var expected = "hello&$filter=";
            var result = "hello".Filter<Person>(p => p.Age > 5);
            Assert.IsTrue(result.StartsWith(expected));
        }

        [TestMethod]
        public void Filter_ReturnsIntegerEqualityOperation_Test()
        {
            var expected = "?$filter=Age eq 5";
            var result = "?".Filter<Person>(p => p.Age == 5);
            Assert.IsTrue(result.StartsWith(expected));
        }
    }
}

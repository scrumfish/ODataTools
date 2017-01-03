using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scrumfish.OData.Client.Common;
using Scrumfish.OData.Client.Tests.TestObjects;
using Scrumfish.OData.Client.v4;

namespace Scrumfish.OData.Client.Tests.v4
{
    [TestClass]
    public class Query_Search_Tests
    {
        [TestMethod]
        public void Search_ReturnsSimpleSearch_Test()
        {
            var expected = "?$search=green";
            var result = "?".CreateODataQuery<Person>()
                .Search(s => "green")
                .ToString();
            Assert.AreEqual(expected,result);
        }

        [TestMethod]
        public void Search_ReturnsSimpleQuotedSearch_Test()
        {
            var expected = "?$search=\"lime green\"";
            var result = "?".CreateODataQuery<Person>()
                .Search(s => "\"lime green\"")
                .ToString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Search_ReturnsSimpleNotSearch_Test()
        {
            var expected = "?$search=NOT green";
            var result = "?".CreateODataQuery<Person>()
                .Search(s => s.Not("green"))
                .ToString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [Ignore]
        public void Search_ReturnsSimpleAndSearchtSearch_Test()
        {
            var expected = "?$search=green AND blue";
            var result = "?".CreateODataQuery<Person>()
                .Search(s => s.Match("green")
                                .And("blue"))
                .ToString();
            Assert.AreEqual(expected, result);
        }
    }
}

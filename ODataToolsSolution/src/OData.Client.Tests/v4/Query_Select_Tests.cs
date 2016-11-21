using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scrumfish.OData.Client.Common;
using Scrumfish.OData.Client.Tests.TestObjects;
using Scrumfish.OData.Client.v4;


namespace Scrumfish.OData.Client.Tests.v4
{
    [TestClass]
    public class Query_Select_Tests
    {
        [TestMethod]
        public void Select_AddsExpectedStringParameterName_Test()
        {
            var expected = "?$select=";
            var result = "?".CreateODataQuery<Person>()
                .Select(p => p.LastName)
                .ToString();
            Assert.IsTrue(result.StartsWith(expected));
        }

        [TestMethod]
        public void Select_All_Test()
        {
            var expected = "?$select=*";
            var result = "?".CreateODataQuery<Person>()
                .SelectAll()
                .ToString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Select_AppendPropertyToQuery_Test()
        {
            var expected = "?$select=LastName,FirstName";
            var result = "?".CreateODataQuery<Person>()
                .Select(p => p.LastName, p => p.FirstName)
                .ToString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Select_AppendOrderByToQuery_Test()
        {
            var expected = "?$select=LastName,FirstName&$orderBy=FirstName";
            var result = "?".CreateODataQuery<Person>()
                .Select(p => p.LastName, p => p.FirstName)
                .OrderBy(p => p.FirstName)
                .ToString();
            Assert.AreEqual(expected, result);
        }

    }
}

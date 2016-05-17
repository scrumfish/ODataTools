using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scrumfish.OData.Client.Tests.TestObjects;
using Scrumfish.OData.Client.v4;

namespace Scrumfish.OData.Client.Tests.v4
{
    [TestClass]
    public class Query_OrderBy_Tests
    {
        [TestMethod]
        public void OrderBy_AddsExpectedParameterName_Test()
        {
            var expected = "?$orderBy=";
            var result = "?".OrderBy<Person>(p => p.LastName);
            Assert.IsTrue(result.StartsWith(expected));
        }

        [TestMethod]
        public void OrderBy_AddsExpectedParameterNameWithAmpersand_Test()
        {
            var expected = "anyparam&$orderBy=";
            var result = "anyparam".OrderBy<Person>(p => p.LastName);
            Assert.IsTrue(result.StartsWith(expected));
        }


        //[TestMethod]
        //public void Orderby_Ascending_Test()
        //{
        //    var expected = "?$orderBy=LastName asc";
        //    var result = "?".OrderBy<Person>(p => p.LastName);
        //    Assert.IsTrue(result.StartsWith(expected));
        //}

        //[TestMethod]
        //public void Orderby_Descending_Test()
        //{
        //    var expected = "?$orderBy=LastName desc";
        //    var result = "?".OrderBy<Person>(p => p.FirstName);
        //    Assert.IsTrue(result.StartsWith(expected));
        //}

    }
}


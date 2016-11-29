using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scrumfish.OData.Client.Common;
using Scrumfish.OData.Client.Tests.TestObjects;
using Scrumfish.OData.Client.v4;

namespace Scrumfish.OData.Client.Tests.v4
{
   
    [TestClass]
    public class Query_Count_Tests
    {
        [TestMethod]
        public void Count_ReturnsCountTrue_Test()
        {
            var expected = "?$count=true";
            var result = "?".CreateODataQuery<Person>()
                .Count(true)
                .ToString(); 
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Count_ReturnsCountFalse_Test()
        {
            var expected = "?$count=false";
            var result = "?".CreateODataQuery<Person>()
                .Count(false)
                .ToString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Count_ReturnsCountInExpand_Test()
        {
            var expected = "?$expand=Cars($count=true)";
            var result = "?".CreateODataQuery<Person>()
                .Expand(p => p.Cars, ODataQueryBuilder.CreateODataQuery<Car>()
                    .Count(true))
                .ToString();
            Assert.AreEqual(expected, result);
        }
    }
}

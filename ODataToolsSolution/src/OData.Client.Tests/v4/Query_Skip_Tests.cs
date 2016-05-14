using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scrumfish.OData.Client.Tests.TestObjects;
using Scrumfish.OData.Client.v4;

namespace Scrumfish.OData.Client.Tests.v4
{
    [TestClass]
    public class Query_Skip_Tests
    {
        [TestMethod]
        public void Skip_AddsExpectedParameterName_Test()
        {
            var expected = "?$skip=";
            var result = "?".Skip(5);
            Assert.IsTrue(result.StartsWith(expected));
        }

        [TestMethod]
        public void Skip_AddsExpectedParameterNameWithAmpersand_Test()
        {
            var expected = "hello&$skip=";
            var result = "hello".Skip(5);
            Assert.IsTrue(result.StartsWith(expected));
        }
    }
}

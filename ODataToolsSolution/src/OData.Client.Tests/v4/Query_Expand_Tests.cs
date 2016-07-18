using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scrumfish.OData.Client.Common;
using Scrumfish.OData.Client.Tests.TestObjects;
using Scrumfish.OData.Client.v4;

namespace Scrumfish.OData.Client.Tests.v4
{
    [TestClass]
    public class Query_Expand_Tests
    {
        [TestMethod]
        public void Expand_AddsExpectedStringParameterName_Test()
        {
            var expected = "?$expand=Cars";
            var result = "?".CreateODataQuery<Person>()
                .Expand(p => p.Cars)
                .ToString();
            Assert.IsTrue(result.StartsWith(expected));
        }
    }
}

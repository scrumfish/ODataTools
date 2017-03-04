using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scrumfish.OData.Client.Common;
using Scrumfish.OData.Client.Tests.TestObjects;
using Scrumfish.OData.Client.v4;

namespace Scrumfish.OData.Client.Tests.v4
{
    [TestClass]
    public class Query_Alias_Tests
    {
        [TestMethod]
        public void Alisa_SetsAliasValue_Test()
        {
            var expected = "?$filter=((FirstName eq @name) and contains(@name,PreferredName))&@name='Steve'";
            var result = "?".CreateODataQuery<Person>()
                .Filter(p => p.FirstName == "@name".AsAlias<string>() && p.PreferredName.Contains("@name".AsAlias<string>()))
                .Alias("@name","Steve")
                .ToString();
            Assert.AreEqual(expected,result);
        }

        [TestMethod]
        public void Alisa_SetsAliasEqualToValue_Test()
        {
            var expected = "?$filter=(Age eq @age)&@age=42";
            var result = "?".CreateODataQuery<Person>()
                .Filter(p => p.Age == "@age".AsAlias<int>())
                .Alias("@age", 42)
                .ToString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Alias_ThrowsExpectedExceptionForInvalidAliasName_Test()
        {
            "?".CreateODataQuery<Person>()
                .Filter(p => p.FirstName == "@name".AsAlias<string>())
                .Alias("name", "Steve");
        }
    }
}

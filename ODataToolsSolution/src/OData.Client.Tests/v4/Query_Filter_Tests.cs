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

        [TestMethod]
        public void Filter_ReturnsEndsWithExpression_Test()
        {
            var expected = "?$filter=endswith(LastName,'eece')";
            var result = "?".CreateODataQuery<Person>()
                .Filter(p => p.LastName.EndsWith("eece"))
                .ToString();
            Assert.AreEqual(expected,result);
        }

        [TestMethod]
        public void Filter_ReturnsStartsWithExpression_Test()
        {
            var expected = "?$filter=startswith(LastName,'Re')";
            var result = "?".CreateODataQuery<Person>()
                .Filter(p => p.LastName.StartsWith("Re"))
                .ToString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Filter_ReturnsSubstringOfExpression_Test()
        {
            var expected = "?$filter=substringof('ece',LastName)";
            var result = "?".CreateODataQuery<Person>()
                .Filter(p => p.LastName.Contains("ece"))
                .ToString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Filter_ReturnsSubstringOneParamExpression_Test()
        {
            var expected = "?$filter=(substring(LastName,2) eq 'eece')";
            var result = "?".CreateODataQuery<Person>()
                .Filter(p => p.LastName.Substring(2) == "eece")
                .ToString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Filter_ReturnsSubstringTwoParamExpression_Test()
        {
            var expected = "?$filter=(substring(LastName,4,3) eq 'eec')";
            var result = "?".CreateODataQuery<Person>()
                .Filter(p => p.LastName.Substring(4, 3) == "eec")
                .ToString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Filter_ReturnsLengthExpression_Test()
        {
            var expected = "?$filter=(length(FirstName) gt 6)";
            var result = "?".CreateODataQuery<Person>()
                .Filter(p => p.FirstName.Length > 6)
                .ToString();
            Assert.AreEqual(expected, result);
        }
    }
}

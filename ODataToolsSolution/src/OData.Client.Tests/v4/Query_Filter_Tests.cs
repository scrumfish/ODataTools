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

        [TestMethod]
        public void Filter_ReturnsIndexOfExpression_Test()
        {
            var expected = "?$filter=(indexof(FirstName,'St') eq 0)";
            var result = "?".CreateODataQuery<Person>()
                .Filter(p => p.FirstName.IndexOf("St") == 0)
                .ToString();
            Assert.AreEqual(expected,result);
        }

        [TestMethod]
        public void Filter_ReturnsReplaceExpressionFromString_Test()
        {
            var expected = "?$filter=(replace(LastName,'ce','th') eq 'Reeth')";
            var result = "?".CreateODataQuery<Person>()
                .Filter(p => p.LastName.Replace("ce", "th") == "Reeth")
                .ToString();
            Assert.AreEqual(expected,result);
        }

        [TestMethod]
        public void Filter_ReturnsReplaceExpressionFromChar_Test()
        {
            var expected = "?$filter=(replace(LastName,'R','S') eq 'Ceece')";
            var result = "?".CreateODataQuery<Person>()
                .Filter(p => p.LastName.Replace('R', 'S') == "Ceece")
                .ToString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Filter_ReturnsToLowerExpression_Test()
        {
            var expected = "?$filter=(tolower(LastName) eq 'reece')";
            var result = "?".CreateODataQuery<Person>()
                .Filter(p => p.LastName.ToLower() == "reece")
                .ToString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Filter_ReturnsToUpperExpression_Test()
        {
            var expected = "?$filter=(toupper(LastName) eq 'REECE')";
            var result = "?".CreateODataQuery<Person>()
                .Filter(p => p.LastName.ToUpper() == "REECE")
                .ToString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Filter_ReturnsTrimExpression_Test()
        {
            var expected = "?$filter=(trim(LastName) eq 'Reece')";
            var result = "?".CreateODataQuery<Person>()
                .Filter(p => p.LastName.Trim() == "Reece")
                .ToString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Filter_ReturnsConcatExpression_Test()
        {
            var expected = "?$filter=(concat(concat(LastName,', '),FirstName) eq 'Reece, Steve')";
            var result = "?".CreateODataQuery<Person>()
                .Filter(p => p.LastName + ", " + p.FirstName == "Reece, Steve")
                .ToString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Filter_ReturnsDayExpressionFromDateTime_Test()
        {
            var expected = "?$filter=(day(Birthday) eq 12)";
            var result = "?".CreateODataQuery<Person>()
                .Filter(p => p.Birthday.Day == 12)
                .ToString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Filter_ReturnsMonthExpressionFromDateTime_Test()
        {
            var expected = "?$filter=(month(Birthday) eq 12)";
            var result = "?".CreateODataQuery<Person>()
                .Filter(p => p.Birthday.Month == 12)
                .ToString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Filter_ReturnsYearExpressionFromDateTime_Test()
        {
            var expected = "?$filter=(year(Birthday) eq 2012)";
            var result = "?".CreateODataQuery<Person>()
                .Filter(p => p.Birthday.Year == 2012)
                .ToString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Filter_ReturnsHourExpressionFromDateTime_Test()
        {
            var expected = "?$filter=(hour(Birthday) eq 11)";
            var result = "?".CreateODataQuery<Person>()
                .Filter(p => p.Birthday.Hour == 11)
                .ToString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Filter_ReturnsMinuteExpressionFromDateTime_Test()
        {
            var expected = "?$filter=(minute(Birthday) eq 58)";
            var result = "?".CreateODataQuery<Person>()
                .Filter(p => p.Birthday.Minute == 58)
                .ToString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Filter_ReturnsSecondExpressionFromDateTime_Test()
        {
            var expected = "?$filter=(second(Birthday) eq 42)";
            var result = "?".CreateODataQuery<Person>()
                .Filter(p => p.Birthday.Second == 42)
                .ToString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Filter_ReturnsDayExpressionFromDateTimeOffset_Test()
        {
            var expected = "?$filter=(day(SomeOffset) eq 12)";
            var result = "?".CreateODataQuery<Person>()
                .Filter(p => p.SomeOffset.Day == 12)
                .ToString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Filter_ReturnsMonthExpressionFromDateTimeOffset_Test()
        {
            var expected = "?$filter=(month(SomeOffset) eq 12)";
            var result = "?".CreateODataQuery<Person>()
                .Filter(p => p.SomeOffset.Month == 12)
                .ToString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Filter_ReturnsYearExpressionFromDateTimeOffset_Test()
        {
            var expected = "?$filter=(year(SomeOffset) eq 2012)";
            var result = "?".CreateODataQuery<Person>()
                .Filter(p => p.SomeOffset.Year == 2012)
                .ToString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Filter_ReturnsHourExpressionFromDateTimeOffset_Test()
        {
            var expected = "?$filter=(hour(SomeOffset) eq 11)";
            var result = "?".CreateODataQuery<Person>()
                .Filter(p => p.SomeOffset.Hour == 11)
                .ToString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Filter_ReturnsMinuteExpressionFromDateTimeOffset_Test()
        {
            var expected = "?$filter=(minute(SomeOffset) eq 58)";
            var result = "?".CreateODataQuery<Person>()
                .Filter(p => p.SomeOffset.Minute == 58)
                .ToString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Filter_ReturnsSecondExpressionFromDateTimeOffset_Test()
        {
            var expected = "?$filter=(second(SomeOffset) eq 42)";
            var result = "?".CreateODataQuery<Person>()
                .Filter(p => p.SomeOffset.Second == 42)
                .ToString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Filter_ReturnsFractionalSecondsExpressionFromDateTime_Test()
        {
            var expected = "?$filter=(fractionalseconds(Birthday) eq .042)";
            var result = "?".CreateODataQuery<Person>()
                .Filter(p => p.Birthday.Millisecond == 42)
                .ToString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Filter_ReturnsFractionalSecondsExpressionFromDateTimeOffset_Test()
        {
            var expected = "?$filter=(fractionalseconds(SomeOffset) eq .042)";
            var result = "?".CreateODataQuery<Person>()
                .Filter(p => p.SomeOffset.Millisecond == 42)
                .ToString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Filter_ReturnsRoundExpressionFromDateTimeOffset_Test()
        {
            var expected = "?$filter=(round(SomeDecimal) eq 42)";
            var result = "?".CreateODataQuery<Person>()
                .Filter(p => decimal.Round(p.SomeDecimal) == 42)
                .ToString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Filter_ReturnsCeilingExpressionFromDateTimeOffset_Test()
        {
            var expected = "?$filter=(ceiling(SomeDecimal) eq 42)";
            var result = "?".CreateODataQuery<Person>()
                .Filter(p => decimal.Ceiling(p.SomeDecimal) == 42)
                .ToString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Filter_ReturnsFloorExpressionFromDateTimeOffset_Test()
        {
            var expected = "?$filter=(floor(SomeDecimal) eq 42)";
            var result = "?".CreateODataQuery<Person>()
                .Filter(p => decimal.Floor(p.SomeDecimal) == 42)
                .ToString();
            Assert.AreEqual(expected, result);
        }
    }
}

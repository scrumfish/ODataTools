﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scrumfish.OData.Client.Common;
using Scrumfish.OData.Client.Tests.TestObjects;
using Scrumfish.OData.Client.v4;

namespace Scrumfish.OData.Client.Tests.v4
{
    [TestClass]
    public class Query_Top_Tests
    {
        [TestMethod]
        public void Top_AddsExpectedParameterName_Test()
        {
            var expected = "?$top=";
            var result = "?".CreateODataQuery<Person>()
                .Top(5)
                .ToString();
            Assert.IsTrue(result.StartsWith(expected));
        }

        [TestMethod]
        public void Top_AddsExpectedParameterNameWithAmpersand_Test()
        {
            var expected = "hello&$top";
            var result = "hello".CreateODataQuery<Person>()
                .Top(5)
                .ToString();
            Assert.IsTrue(result.StartsWith(expected));
        }
    }
}

using System.Collections.Generic;
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
            Assert.AreEqual(result, expected);
        }

        [TestMethod]
        public void Expand_ExpandsComplexObject_Test()
        {
            var expected = "?$expand=Address/City";
            var result = "?".CreateODataQuery<Person>()
                .Expand(p => p.Address.City)
                .ToString();
            Assert.AreEqual(expected, result);

        }

        [TestMethod]
        public void Expand_ExpandsDeepComplexObject_Test()
        {
            var expected = "?$expand=Address/City/Population";
            var result = "?".CreateODataQuery<Person>()
                .Expand(p => p.Address.City.Population)
                .ToString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Expand_ExpandsSubQuery_Test()
        {
            var expected = "?$expand=Cars($filter=(Make eq 'Ford');$select=Make,Model)";
            var result = "?".CreateODataQuery<Person>()
                .Expand(p => p.Cars, ODataQueryBuilder.CreateODataQuery<Car>()
                    .Filter(c => c.Make == "Ford")
                    .Select(c => c.Make, c => c.Model))
                .ToString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Expand_ExpandsDeepSubQuery_Test()
        {
            var expected = "?$expand=Cars/AuthorizedDrivers($filter=(Age gt 21);$select=LastName,LicenseNumber)";
            var result = "?".CreateODataQuery<Person>()
                .Expand(p => p.Cars.WithDependency<Car>(c => c.AuthorizedDrivers), ODataQueryBuilder.CreateODataQuery<Driver>()
                    .Filter(a => a.Age > 21)
                    .Select(a => a.LastName, a => a.LicenseNumber))
                .ToString();
            Assert.AreEqual(expected, result);
        }
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OData.Client.Spatial.Tests.TestObjects;
using Scrumfish.OData.Client.Spatial;

namespace OData.Client.Spatial.Tests
{
    [TestClass]
    public class GeographyPointStringifier_Tests
    {
        private GeographyPointStringifier _target;

        [TestInitialize]
        public void Setup()
        {
            var point = new TestGeographyPoint(1,-1);
            _target = new GeographyPointStringifier(point);
        }

        [TestMethod]
        public void Stringify_ReturnsExpectedResult_Test()
        {
            var result = _target.Stringify();
            Assert.AreEqual("POINT(1 -1)", result);
        }
    }
}

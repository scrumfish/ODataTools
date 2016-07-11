using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scrumfish.OData.Objects;

namespace OData.Objects.Tests
{
    [TestClass]
    public class TypeFactoryConfiguration_Tests
    {
        private TypeFactoryConfiguration _target;

        [TestInitialize]
        public void Setup()
        {
            _target = TypeFactoryConfiguration.GetConfiguration();
        }

        [TestMethod]
        public void TypeFactory_ReturnsElements_Test()
        {
            Assert.AreEqual(2, _target.Stringifiers.Count);
        }

        [TestMethod]
        public void TypeFactory_ReturnsTypeNames_Test()
        {
            Assert.AreEqual("type1", _target.Stringifiers[0].TypeName);
            Assert.AreEqual("type2", _target.Stringifiers[1].TypeName);
        }

        [TestMethod]
        public void TypeFactory_ReturnsStringifierNames_Test()
        {
            Assert.AreEqual("name1", _target.Stringifiers[0].StringifierName);
            Assert.AreEqual("name2", _target.Stringifiers[1].StringifierName);
        }
    }
}

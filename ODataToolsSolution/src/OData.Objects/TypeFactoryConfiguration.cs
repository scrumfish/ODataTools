using System;
using System.Configuration;
using System.Runtime.InteropServices;

namespace Scrumfish.OData.Objects
{
    public class TypeFactoryConfiguration : ConfigurationSection
    {
        protected TypeFactoryConfiguration() { }

        public static TypeFactoryConfiguration GetConfiguration()
        {
            return
                (TypeFactoryConfiguration)
                    System.Configuration.ConfigurationManager.GetSection("scrumfish.Stringifiers");
        }

        [ConfigurationProperty(name: "stringifiers")]
        public TypeFactoryItems Stringifiers
        {
            get { return (TypeFactoryItems) this["stringifiers"]; }
            set { this["stringifiers"] = value; }
        }
    }

    [ConfigurationCollection(typeof (TypeFactoryStringifier))]
    public class TypeFactoryItems : ConfigurationElementCollection
    {
        private const string ItemName = "stringifier";
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMapAlternate;
            }
        }
        protected override string ElementName
        {
            get
            {
                return ItemName;
            }
        }

        protected override bool IsElementName(string elementName)
        {
            return elementName.Equals(ItemName,
              StringComparison.InvariantCultureIgnoreCase);
        }

        public override bool IsReadOnly()
        {
            return false;
        }
        
        protected override ConfigurationElement CreateNewElement()
        {
            return new TypeFactoryStringifier();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((TypeFactoryStringifier) element).TypeName;
        }
        public TypeFactoryStringifier this[int idx] => (TypeFactoryStringifier)BaseGet(idx);
    }

    public class TypeFactoryStringifier : ConfigurationElement
    {
        [ConfigurationProperty(name: "typeName", IsRequired = true, IsKey = true)]
        public string TypeName
        {
            get { return (string) this["typeName"]; }
            set { this["typeName"] = value; }
        }

        [ConfigurationProperty(name: "stringiferName", IsRequired = true)]
        public string StringifierName
        {
            get { return (string) this["stringiferName"]; }
            set { this["stringiferName"] = value; }
        }
    }
}

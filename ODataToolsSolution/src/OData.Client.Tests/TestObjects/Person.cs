using System;
using System.Spatial;

namespace Scrumfish.OData.Client.Tests.TestObjects
{
    internal class Person
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int Age { get; set; }
        public DateTime Birthday { get; set; }
        public long? SomeBigNumber { get; set; }
        public DateTimeOffset SomeOffset { get; set; }
        public decimal SomeDecimal { get; set; }
        public GeographyPoint MyHomePosition => new TestGeographyPoint(33.812511, -117.918976);
    }

    internal class Employee : Person
    {
        public DateTime HireDate { get; set; }
    }

    internal class PersonContainer
    {
        public Person ThePerson { get; set; }
    }
}
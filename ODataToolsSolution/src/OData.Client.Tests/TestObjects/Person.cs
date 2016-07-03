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

    internal class TestGeographyPoint : GeographyPoint
    {
        public TestGeographyPoint(double latitude, double longitude) : base(CoordinateSystem.DefaultGeography, SpatialImplementation.CurrentImplementation)
        {
            IsEmpty = false;
            Latitude = latitude;
            Longitude = longitude;
            Z = 0;
            M = 0;
        }

        public override bool IsEmpty { get;  }
        public override double Latitude { get; }
        public override double Longitude { get; }
        public override double? Z { get; }
        public override double? M { get; }
    }
}
using System.Spatial;

namespace OData.Client.Spatial.Tests.TestObjects
{
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
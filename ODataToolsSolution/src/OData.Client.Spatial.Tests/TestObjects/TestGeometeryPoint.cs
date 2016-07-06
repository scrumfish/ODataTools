using System.Spatial;

namespace OData.Client.Spatial.Tests.TestObjects
{
    internal class TestGeometeryPoint : GeometryPoint
    {
        public TestGeometeryPoint(double x, double y) : base(CoordinateSystem.DefaultGeography, SpatialImplementation.CurrentImplementation)
        {
            IsEmpty = false;
            X = x;
            Y = y;
            Z = 0;
            M = 0;
        }

        public override bool IsEmpty { get; }
        public override double X { get; }
        public override double Y { get; }
        public override double? Z { get; }
        public override double? M { get; }
    }
}
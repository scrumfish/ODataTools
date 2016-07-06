using System;
using System.Spatial;
using Scrumfish.OData.Objects;

namespace Scrumfish.OData.Client.Spatial
{
    public class GeometryPointStringifier : Stringifier
    {
        public GeometryPointStringifier(object item)
            : base(item)
        {

        }

        public override string Stringify()
        {
            var point = Item as GeometryPoint;
            if (point == null)
            {
                throw new InvalidCastException("Could not cast Item to System.Spatial.GeometryPoint");
            }
            return $"POINT({point.X} {point.Y})";
        }
    }
}
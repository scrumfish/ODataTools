using System;
using System.Spatial;
using Scrumfish.OData.Objects;

namespace Scrumfish.OData.Client.Spatial
{
    public class GeographyPointStringifier : Stringifier
    {
        public GeographyPointStringifier(object item)
            : base(item)
        {
            
        }

        public override string Stringify()
        {
            var point = Item as GeographyPoint;
            if (point == null)
            {
                throw new InvalidCastException("Could not cast Item to System.Spatial.GeographyPoint");
            }
            return $"POINT({point.Latitude} {point.Longitude})";
        }
    }
}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scrumfish.OData.Client.Tests.TestObjects
{
    internal class Car
    {
        public int CarId { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }

        [ForeignKey("Person")]
        public int? personId { get; set; }
        public virtual Person Person { get; set; }

        public IEnumerable<Driver> AuthorizedDrivers { get; set; } 
        
    }
}

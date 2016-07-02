using System;

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
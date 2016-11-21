namespace Scrumfish.OData.Client.Tests.TestObjects
{
    internal class Address
    {
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public City City { get; set; }
        public string StateOrProvence { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }

    internal class City
    {
        public string Name { get; set; }
        public int Population { get; set; }
    }
}

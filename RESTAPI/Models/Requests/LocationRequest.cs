namespace Edgias.Inventory.Management.RESTAPI.Models.Requests
{
    public class LocationRequest
    {
        public string Name { get; set; }

        public string Street { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }

        public string Country { get; set; }
    }
}

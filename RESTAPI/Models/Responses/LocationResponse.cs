﻿using System;

namespace Edgias.Inventory.Management.RESTAPI.Models.Responses
{
    public class LocationResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Street { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }

        public string Country { get; set; }
    }
}

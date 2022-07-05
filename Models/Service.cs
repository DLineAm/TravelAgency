using System;
using System.Collections.Generic;

#nullable disable

namespace TravelAgency
{
    public partial class Service
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Price { get; set; }
    }
}

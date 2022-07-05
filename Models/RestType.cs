using System;
using System.Collections.Generic;

#nullable disable

namespace TravelAgency
{
    public partial class RestType
    {
        public RestType()
        {
            Vouchers = new HashSet<Voucher>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Restrictions { get; set; }

        public virtual ICollection<Voucher> Vouchers { get; set; }
    }
}

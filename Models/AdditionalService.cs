using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;

#nullable disable

namespace TravelAgency
{
    public partial class AdditionalService
    {
        public AdditionalService()
        {
            VoucherAdditService1s = new HashSet<Voucher>();
            VoucherAdditService2s = new HashSet<Voucher>();
            VoucherAdditService3s = new HashSet<Voucher>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Price { get; set; }

        public virtual ICollection<Voucher> VoucherAdditService1s { get; set; }
        public virtual ICollection<Voucher> VoucherAdditService2s { get; set; }
        public virtual ICollection<Voucher> VoucherAdditService3s { get; set; }
    }
}

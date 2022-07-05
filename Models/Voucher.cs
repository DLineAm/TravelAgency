using System;
using System.Collections.Generic;

#nullable disable

namespace TravelAgency
{
    public partial class Voucher
    {
        public int Id { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Duration { get; set; }
        public int? HotelId { get; set; }
        public int? RestTypeId { get; set; }
        public int? AdditService1Id { get; set; }
        public int? AdditService2Id { get; set; }
        public int? AdditService3Id { get; set; }
        public int? ClientId { get; set; }
        public int? StuffId { get; set; }
        public string BookingStatus { get; set; }
        public string PaymentStatus { get; set; }

        public virtual AdditionalService AdditService1 { get; set; }
        public virtual AdditionalService AdditService2 { get; set; }
        public virtual AdditionalService AdditService3 { get; set; }
        public virtual Client Client { get; set; }
        public virtual Hotel Hotel { get; set; }
        public virtual RestType RestType { get; set; }
        public virtual Stuff Stuff { get; set; }
    }
}

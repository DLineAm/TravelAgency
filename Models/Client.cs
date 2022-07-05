using System;
using System.Collections.Generic;

#nullable disable

namespace TravelAgency
{
    public partial class Client
    {
        public Client()
        {
            Vouchers = new HashSet<Voucher>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string PassportCode { get; set; }

        public virtual ICollection<Voucher> Vouchers { get; set; }
    }
}

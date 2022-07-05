using System;
using System.Collections.Generic;

#nullable disable

namespace TravelAgency
{
    public partial class Stuff
    {
        public Stuff()
        {
            Vouchers = new HashSet<Voucher>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public int? Age { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public int? PassportCode { get; set; }
        public int? RoleId { get; set; }

        public virtual Role Role { get; set; }
        public virtual ICollection<Voucher> Vouchers { get; set; }
    }
}

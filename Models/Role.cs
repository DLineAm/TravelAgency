using System;
using System.Collections.Generic;

#nullable disable

namespace TravelAgency
{
    public partial class Role
    {
        public Role()
        {
            Stuffs = new HashSet<Stuff>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Salary { get; set; }
        public string Responsibilities { get; set; }
        public string Demands { get; set; }

        public virtual ICollection<Stuff> Stuffs { get; set; }
    }
}

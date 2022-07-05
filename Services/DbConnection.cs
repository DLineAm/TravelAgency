using System;
using System.Collections.Generic;
using System.Text;

namespace TravelAgency.Services
{
    public static class DbConnection
    {
        private static Rieltors_EvsyutinContext db;

        public static Rieltors_EvsyutinContext GetContext() => db ??= new Rieltors_EvsyutinContext();
    }
}

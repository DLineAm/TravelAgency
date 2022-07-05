using System;

namespace TravelAgency.Services
{
    public static class Extensions
    {
        public static T To<T>(this object value) where T: class, new() => (T)value;
    }
}
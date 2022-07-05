using System;
using System.Collections.Generic;

namespace TravelAgency.Services
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AvailablePropertiesAttribute : Attribute
    {
        public int Count { get; set; }

        public List<string> Names { get; set; }

        public AvailablePropertiesAttribute(int count)
        {
            this.Count = count;
        }

        public AvailablePropertiesAttribute(List<string> names)
        {
            this.Names = names;
        }
    }
}
using System;

namespace DHwD.Models
{
    public class Location
    {
        public int ID { get; set; }
        public Place Place { get; set; }
        public Double Latitude { get; set; }
        public Double Longitude { get; set; }
    }
}

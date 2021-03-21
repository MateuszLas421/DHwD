using System.Collections.Generic;

namespace DHwD.Models.REST
{
    public class ActivePlace
    {
        public int ID { get; set; }
        ///Place Place;
        public bool Active { get; set; }
        public ICollection<Status> Status { get; set; }
        public ICollection<Place> Places { get; set; }
    }
}

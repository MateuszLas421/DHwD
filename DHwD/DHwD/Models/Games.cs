using System;
using System.Collections.Generic;

namespace DHwD.Models
{
    public class Games
    {  
        public int Id { get; set; }
        public DateTime DateTimeStart { get; set; }
        public DateTime DateTimeEnd { get; set; }
        public DateTime DateTimeCreate { get; set; }
        public DateTime DateTimeEdit { get; set; }
        public ICollection<Team> Teams { get; set; }
        public ICollection<Place> Place { get; set; }
    }
}

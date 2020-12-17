using System;

namespace DHwD.Models
{
    public class Points
    {
        public int UserId { get; set; }
        public int UserPoints { get; set; }
        public DateTime DataTimeEdit { get; set; }
        public DateTime DataTimeCreate { get; set; }
        public User User { get; set; }
    }
}

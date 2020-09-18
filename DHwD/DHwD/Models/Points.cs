using System;

namespace DHwD.Models
{
    public class Points
    {
        public int Id { get; set; }
        public int UserPoints { get; set; }
        public DateTime DataTimeEdit { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}

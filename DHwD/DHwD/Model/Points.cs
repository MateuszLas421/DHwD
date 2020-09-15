using DHwD.Model;
using System;

namespace DHwD_web.Models
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

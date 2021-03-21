using System;
using System.ComponentModel.DataAnnotations;

namespace DHwD.Models.REST
{
    public class Chats
    {
        public ulong Id { get; set; }

        public bool IsSystem { get; set; }

        public string Text { get; set; }

        public DateTime DateTimeCreate { get; set; }

        public User User { get; set; }

        public Games Game { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace DHwD.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool StatusPassword { get; set; }  //Password Exist
        public string Password { get; set; }
        public string UserNickName { get; set; }
        public string Description { get; set; }
        public bool OnlyOnePlayer { get; set; }
        public string MyteamTEXT { get; set; }
        public bool MyTeam { get; set; }
        public Games Games { get; set; }
        public int StatusRef { get; set; }
        public Status Status { get; set; }
        public Team()
        {
            Games = new Games(); 
        }
    }
}

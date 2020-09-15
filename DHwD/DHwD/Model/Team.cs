using DHwD.Model;
using System;
using System.Collections.Generic;

namespace DHwD_web.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public User Id_Founder { get; set; }// FK
        public DateTime DateTimeCreate { get; set; }  //  User creation date
        public DateTime DateTimeEdit { get; set; }   // User edition date
        public bool StatusPassword { get; set; }  //Password Exist
        public string Password { get; set; }
        public ICollection<TeamMembers> TeamMembers { get; set; }
        public Games Games { get; set; }
    }
}

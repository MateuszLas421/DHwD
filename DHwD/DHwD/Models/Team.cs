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
    }
}

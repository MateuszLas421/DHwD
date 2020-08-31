using System;
using System.Collections.Generic;
using System.Text;

namespace DHwD.Model
{
    public class User
    {
        public Guid Guid { get; set; }
        public string NickName { get; set; }
        public string Token { get; set; }
        public DateTime DateTimeCreate { get; set; }  //  User creation date
        public DateTime DateTimeEdit { get; set; }   // User edition date
    }
}

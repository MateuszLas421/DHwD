using System;
using System.Collections.Generic;
using System.Text;

namespace DHwD.Models.REST
{
    public class User : UserRegistration
    {
        public int Id { get; set; }
        public DateTime DateTimeCreate { get; set; }  //  User creation date
        public DateTime DateTimeEdit { get; set; }   // User edition date
    }
}

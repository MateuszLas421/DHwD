using System;
using System.Collections.Generic;
using System.Text;

namespace DHwD.Models
{
    public class SolutionRequest
    {
        public int idMystery { get; set; }

        public string TextSolution { get; set; }

        public int gameid { get; set; }

        public DateTime DataTimeRequest { get; set; }

    }
}

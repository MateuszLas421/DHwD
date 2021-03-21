using System;
using System.Collections.Generic;
using System.Text;

namespace DHwD.Models
{
    public class SolutionRequest
    {
        public int idMystery { get; private set; }

        public string TextSolution { get; private set; }

        public int gameid { get; private set; }

        public DateTime DataTimeRequest { get; set; }

    }
}

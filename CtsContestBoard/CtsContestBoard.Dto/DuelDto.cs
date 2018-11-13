using System;
using System.Collections.Generic;
using System.Linq;

namespace CtsContestBoard.Dto
{
    public class DuelDto
    {
        public DuelDto()
        {
            Players = new List<string>();
        }

        public int Id { get; set; }
        public List<string> Players { get; set; }
        public DateTime StartTime { get; set; }
        public int Prize { get; set; }
        public string Winner { get; set; }
    }
}
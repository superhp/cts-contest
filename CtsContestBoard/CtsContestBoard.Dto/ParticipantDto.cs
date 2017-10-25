using System;

namespace CtsContestBoard.Dto
{
    public class ParticipantDto
    {
        public string Name { get; set; }
        public string Picture { get; set; }
        public int TotalBalance { get; set; }
        //public int TodayEarnedPoints { get; set; }
        public int TodaysBalance { get; set; }
        public DateTime LastSolved { get; set; }
    }
}
using System;

namespace CtsContestWeb.Db.Entities
{
    public class Solution : IAuditable
    {
        public int SolutionId { get; set; }
        public int UserId { get; set; }
        public int TaskId { get; set; }
        public string Source { get; set; }
        public int Score { get; set; }
        public DateTime Created { get; set; }
    }
}

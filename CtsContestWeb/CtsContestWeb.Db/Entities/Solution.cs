using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CtsContestWeb.Db.Entities
{
    public class Solution : IAuditable
    {
        public int SolutionId { get; set; }
        public string UserEmail { get; set; }
        [ForeignKey("UserEmail")]
        public User User { get; set; }
        public int TaskId { get; set; }
        public string Source { get; set; }
        public int Score { get; set; }
        public DateTime Created { get; set; }
    }
}

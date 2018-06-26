using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CtsContestWeb.Db.Entities
{
    public class CompetitionSolution : IAuditable
    {
        public int CompetitionSolutionId { get; set; }
        public int CompetitionId { get; set; }
        public virtual Competition Competition { get; set; }
        public string UserEmail { get; set; }
        [ForeignKey("UserEmail")]
        public User User { get; set; }
        public string Source { get; set; }
        public bool IsCorrect { get; set; }
        public int Language { get; set; }
        public DateTime Created { get; set; }
    }
}
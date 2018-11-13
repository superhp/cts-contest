using System;
using System.ComponentModel.DataAnnotations.Schema;
using CtsContestBoard.Db.Entities;

namespace CtsContestWeb.Db.Entities
{
    public class DuelSolution : IAuditable
    {
        public int DuelSolutionId { get; set; }
        public int DuelId { get; set; }
        public virtual Duel Duel { get; set; }
        public string UserEmail { get; set; }
        [ForeignKey("UserEmail")]
        public User User { get; set; }
        public string Source { get; set; }
        public bool IsCorrect { get; set; }
        public int Language { get; set; }
        public DateTime Created { get; set; }
    }
}
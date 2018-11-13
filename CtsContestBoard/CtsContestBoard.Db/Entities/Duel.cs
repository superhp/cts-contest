using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CtsContestBoard.Db.Entities
{
    public class Duel : IAuditable
    {
        public int DuelId { get; set; }
        public string FirstPlayerEmail { get; set; }
        [ForeignKey("FirstPlayerEmail")]
        public virtual User FirstPlayer { get; set; }
        public string SecondPlayerEmail { get; set; }
        [ForeignKey("SecondPlayerEmail")]
        public virtual User SecondPlayer { get; set; }
        public string WinnerEmail { get; set; }
        [ForeignKey("WinnerEmail")]
        public User Winner { get; set; }
        public int TaskId { get; set; }
        public int Prize { get; set; }
        public DateTime Created { get; set; }
    }
}
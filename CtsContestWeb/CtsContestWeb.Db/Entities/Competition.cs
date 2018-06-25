using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CtsContestWeb.Db.Entities
{
    public class Competition : IAuditable
    {
        public int CompetitionId { get; set; }
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
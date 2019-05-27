using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CtsContestWeb.Db.Entities
{
    public class TaskOutput : IAuditable
    {
        public int TaskOutputId { get; set; }
        public int TaskInputId { get; set; }
        [ForeignKey("TaskInputId")]
        public virtual TaskInput Input { get; set; }
        public string Value { get; set; }
        public DateTime Created { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CtsContestWeb.Db.Entities
{
    public class TaskInput : IAuditable
    {
        public int TaskInputId { get; set; }
        public int TaskId { get; set; }
        [ForeignKey("TaskId")]
        public virtual Task Task { get; set; }
        public virtual TaskOutput Output { get; set; }
        public string Value { get; set; }
        public bool IsSample { get; set; }
        public DateTime Created { get; set; }
    }
}

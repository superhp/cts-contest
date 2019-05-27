using System;
using System.Collections.Generic;

namespace CtsContestWeb.Db.Entities
{
    public class Task : IAuditable
    {
        public int TaskId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string InputType { get; set; }
        public int Value { get; set; }
        public List<TaskInput> Inputs { get; set; }
        public DateTime Created { get; set; }
    }
}

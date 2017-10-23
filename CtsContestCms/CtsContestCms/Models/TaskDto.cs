using System.Collections.Generic;

namespace CtsContestCms.Models
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Value { get; set; }
        public IEnumerable<string> Inputs { get; set; }
        public IEnumerable<string> Outputs { get; set; }
        public string InputType { get; set; }
    }
}
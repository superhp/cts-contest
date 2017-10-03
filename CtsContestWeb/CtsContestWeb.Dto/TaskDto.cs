using System;
using System.Collections.Generic;
using System.Text;

namespace CtsContestWeb.Dto
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Value { get; set; }
        public List<string> Inputs { get; set; }
        public List<string> Outputs { get; set; }
        public bool IsSolved { get; set; }
    }
}

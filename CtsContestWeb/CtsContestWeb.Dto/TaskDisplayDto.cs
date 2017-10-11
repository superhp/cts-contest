using System;
using System.Collections.Generic;
using System.Text;

namespace CtsContestWeb.Dto
{
    public class TaskDisplayDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Value { get; set; }
        public bool IsSolved { get; set; }
    }
}

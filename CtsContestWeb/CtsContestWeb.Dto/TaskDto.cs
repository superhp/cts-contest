using System;
using System.Collections.Generic;
using System.Text;

namespace CtsContestWeb.Dto
{
    public class TaskDto : TaskDisplayDto
    {
        public List<string> Inputs { get; set; }
        public List<string> Outputs { get; set; }

        public string InputType { get; set; }
    }
}

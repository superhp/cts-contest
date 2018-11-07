using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace CtsContestWeb.Dto
{
    public class TaskDto : TaskDisplayDto
    {
        [JsonIgnore]
        public List<string> Inputs { get; set; }
        [JsonIgnore]
        public List<string> Outputs { get; set; }

        public string InputType { get; set; }
        public bool IsForDuel { get; set; }
    }
}

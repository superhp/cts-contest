using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace CtsContestWeb.Dto
{
    public class DuelDto
    {
        public DuelDto()
        {
            StartTime = DateTime.Now;
            Players = new List<PlayerDto>();
        }

        public int Id { get; set; }
        public List<PlayerDto> Players { get; set; }
        public DateTime StartTime { get; }
        public TaskDto Task { get; set; }
        public int Prize { get; set; }
        [JsonIgnore]
        public string GroupName => string.Join("", Players.Select(p => p.Email));
    }
}
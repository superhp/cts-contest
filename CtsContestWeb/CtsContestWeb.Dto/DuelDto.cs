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
        private int _prize;
        public int Prize {
            get {
                return _prize;
            }

            set {
                _prize = CalculateDuelPrize(value);
            }
        }
        [JsonIgnore]
        public string GroupName => string.Join("", Players.Select(p => p.Email));
        public int Duration { get; set; }

        private int CalculateDuelPrize(int taskValue) {
            switch (taskValue) {
                case 15:
                    return 25;
                case 20:
                    return 40;
                case 40:
                    return 60;
                default:
                    return taskValue;
            }
        }
    }
}
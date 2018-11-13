using Newtonsoft.Json;

namespace CtsContestWeb.Dto
{
    public class PlayerDto
    {
        [JsonIgnore]
        public string Email { get; set; }
        public string Picture { get; set; }
        public string ConnectionId { get; set; }
        public string Name { get; set; }
        public int TotalWins { get; set; }
        public int TotalLooses { get; set; }
    }
}
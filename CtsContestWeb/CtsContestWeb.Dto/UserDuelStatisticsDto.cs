namespace CtsContestWeb.Dto
{
    public class UserDuelStatisticsDto
    {
        public string Email { get; set; }
        public int TotalWins { get; set; }
        public int TotalLooses { get; set; }
        public bool IsInDuel { get; set; }
        public int ActivePlayers { get; set; }
    }
}
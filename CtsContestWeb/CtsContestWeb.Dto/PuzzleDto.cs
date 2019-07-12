namespace CtsContestWeb.Dto
{
    public class PuzzleDto
    {
        public int Id { get; set; }
        public string Identifier { get; set; } // TODO: should both be used? Maybe only one is enough
        public bool IsSolved { get; set; }
        public int Value { get; set; }
        public string BaseUrl { get; set; }
    }
}

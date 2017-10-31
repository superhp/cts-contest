namespace CtsContestWeb.Dto
{
    public class CompileDto
    {
        public bool Compiled { get; set; }

        public bool ResultCorrect { get { return FailedInput == 0; } }

        public int TotalInputs { get; set; }

        public int FailedInput { get; set; }

        public string Message { get; set; }
    }
}

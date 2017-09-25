using Newtonsoft.Json;
using System.Collections.Generic;

namespace CtsContestWeb.Dto
{
    public class CompileDto
    {
        public bool Compiled { get; set; }

        public bool ResultCorrect { get { return FailedInputs == 0; } }

        public int TotalInputs { get; set; }

        public int FailedInputs { get; set; }

        public string Message { get; set; }
    }
}

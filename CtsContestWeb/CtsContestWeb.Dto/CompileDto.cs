using Newtonsoft.Json;
using System.Collections.Generic;

namespace CtsContestWeb.Dto
{
    public class CompileDto
    {
        public bool Success { get { return FailedInputs == 0; } }

        public int TotalInputs { get; set; }

        public int FailedInputs { get; set; }

    }
}

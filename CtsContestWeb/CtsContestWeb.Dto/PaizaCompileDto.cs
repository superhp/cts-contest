using System;
using System.Collections.Generic;
using System.Text;

namespace CtsContestWeb.Dto
{
    public class PaizaCompileDto
    {
        public bool Compiled { get; set; }

        public bool IsOutputCorrect { get; set; }

        public string Message { get; set; }
    }
}

using System.Collections.Generic;

namespace CtsContestWeb.Dto
{
    public class LanguageDto
    {
        public LanguageDto()
        {
            Names = new Dictionary<string, string>();
            Codes = new Dictionary<string, int>();
        }

        public Dictionary<string, string> Names { get; set; }

        public Dictionary<string, int> Codes { get; set; }
    }
}


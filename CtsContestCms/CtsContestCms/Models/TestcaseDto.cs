using Newtonsoft.Json;

namespace CtsContestCms.Models
{
    public class TestcaseDto
    {
        public string Input { get; set; }
        public string Output { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool IsSample { get; set; }
    }
}
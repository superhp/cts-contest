using Newtonsoft.Json;

namespace CtsContestWeb.Dto
{
    public class IdeoneCompilerResultDto
    {
        public int Status { get; set; }
        public IdeoneResultEnum Result { get; set; }
        [JsonProperty("any_cmperr")]
        public bool CompileFailed { get; set; }
        [JsonProperty("stdout")]
        public string Output { get; set; }
        [JsonProperty("stderr")]
        public string Error { get; set; }
        [JsonProperty("cmperr")]
        public string CompileInfo { get; set; }
        public string Time { get; set; }
        public string Memory { get; set; }
        public string Signal { get; set; }
    }

    public enum IdeoneResultEnum
    {
        CompilationError = 11,
        RuntimeError = 12,
        Timeout = 13,
        Success = 15,
        MemoryLimit = 17,
        IllegalSystemCall = 19,
        InternalError = 20
    }
}
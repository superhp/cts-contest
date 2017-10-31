using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CtsContestWeb.Dto
{
    public class PaizaCompilerResultDto
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("language")]
        public string Language { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("build_stdout")]
        public string BuildStdout { get; set; }
        [JsonProperty("build_stderr")]
        public string BuildStderr { get; set; }
        [JsonProperty("build_exit_code")]
        public int? BuildExitCode { get; set; }
        [JsonProperty("build_result")]
        public string BuildResult { get; set; }
        [JsonProperty("stdout")]
        public string Stdout { get; set; }
        [JsonProperty("stderr")]
        public string Stderr { get; set; }
        [JsonProperty("exit_code")]
        public int? ExitCode { get; set; }
        [JsonProperty("result")]
        public string Result { get; set; }
    }
}

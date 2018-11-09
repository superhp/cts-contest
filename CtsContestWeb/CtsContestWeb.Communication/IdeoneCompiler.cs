using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CtsContestWeb.Dto;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;

namespace CtsContestWeb.Communication
{
    public class IdeoneCompiler : CompilerLanguages, ICompiler
    {
        private IConfiguration _configuration;

        public IdeoneCompiler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<CompileDto> Compile(TaskDto task, string source, int language)
        {
            var ideoneApiUrl = _configuration["IdeoneApiUrl"];
            var client = new RestClient(ideoneApiUrl);

            var compileDto = new CompileDto
            {
                Compiled = true,
                FailedInput = 0,
                TotalInputs = task.Inputs.Count
            };

            var languages = GetLanguagesMap();
            var languageCode = languages[language];

            for (int i = 0; i < task.Outputs.Count; i++)
            {
                var request = new RestRequest("/api/submissions", Method.POST);

                request.AddParameter("sourceCode", source);
                request.AddParameter("langId", languageCode);
                request.AddParameter("stdin", task.Inputs[i]);

                TaskCompletionSource<IRestResponse> taskCompletion = new TaskCompletionSource<IRestResponse>();
                client.ExecuteAsync(request, r => taskCompletion.SetResult(r));

                RestResponse response = (RestResponse)await taskCompletion.Task;

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    throw new ArgumentException("Error compiling task");

                dynamic data = JsonConvert.DeserializeObject(response.Content);
                var compileId = data.id;
                var result = await GetCompileResult(compileId.ToString(), task.Outputs[i], client);

                if (result.Compiled)
                {
                    if (!result.IsOutputCorrect)
                    {
                        compileDto.FailedInput = i + 1;
                        compileDto.Message = result.Message;

                        return compileDto;
                    }
                }
                else
                {
                    return new CompileDto
                    {
                        Compiled = false,
                        Message = result.Message
                    };
                }

            }

            return compileDto;
        }

        private async Task<PaizaCompileDto> GetCompileResult(string compileId, string expectedOutput, RestClient client)
        {
            await Task.Delay(200);

            var request = new RestRequest("/api/submissions/{id}", Method.GET);
            request.AddUrlSegment("id", compileId);

            var taskCompletion = new TaskCompletionSource<IRestResponse>();
            client.ExecuteAsync(request, r => taskCompletion.SetResult(r));

            var response = (RestResponse)await taskCompletion.Task;

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new ArgumentException("Error compiling task");

            var data = JsonConvert.DeserializeObject<IdeoneCompilerResultDto>(response.Content);

            if (data.Status != 0)
                return await GetCompileResult(compileId, expectedOutput, client);

            var expected = string.Join('\n', expectedOutput.Split('\n', '\r').Where(s => s.Length != 0).Select(s => s.Trim('\r', '\n', ' ')));
            var actual = string.Join('\n', data.Output.Split('\n', '\r').Where(s => s.Length != 0).Select(s => s.Trim('\r', '\n', ' ')));

            var compileResult = new PaizaCompileDto
            {
                Compiled = !data.CompileFailed,
                Message = (string.IsNullOrEmpty(data.Error)) ? data.Error : data.CompileInfo,
                IsOutputCorrect = data.Output != null && actual.Equals(expected)
            };

            // Temporary check for timeout error. Since HackerRank compiler doesn't complain about timeout error clearly, 
            // Paiza runs as a secondary compiler and checks for timeout. 
            if (data.Result == IdeoneResultEnum.Timeout)
            {
                compileResult.Compiled = true;
                compileResult.Message = "Timeout. Your algorithm is not performant enough.";
                compileResult.IsOutputCorrect = false;
            }

            return compileResult;
        }

        private Dictionary<int, int> GetLanguagesMap()
        {
            var map = new Dictionary<int, int>();

            map.Add(1, 9); // or 10
            map.Add(2, 12);
            map.Add(43, 40); // or 41
            map.Add(15, 71); 
            map.Add(51, 77);
            map.Add(9, 11);
            map.Add(21, 34);
            map.Add(12, 37);
            map.Add(16, 29);
            map.Add(6, 56);
            map.Add(5, 63); // or 64
            map.Add(30, 65);
            map.Add(8, 69);
            map.Add(7, 58);
            map.Add(24, 67);
            map.Add(36, 19);
            map.Add(33, 30);
            map.Add(22, 25); // or 26
            map.Add(13, 18);

            return map;
        }
    }
}

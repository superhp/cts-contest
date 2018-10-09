using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CtsContestWeb.Dto;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;

namespace CtsContestWeb.Communication
{
    public class PaizaCompiler : CompilerLanguages, ICompiler
    {
        readonly IConfiguration _configuration;

        public PaizaCompiler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<CompileDto> Compile(TaskDto task, string source, int language)
        {
            var paizaApiUrl = _configuration["PaizaApiUrl"];
            var client = new RestClient(paizaApiUrl);

            var compileDto = new CompileDto
            {
                Compiled = true,
                FailedInput = 0,
                TotalInputs = task.Inputs.Count
            };

            var languages = GetLanguages();
            var languageCode = languages.Codes.FirstOrDefault(x => x.Value == language).Key;

            for (int i = 0; i < task.Outputs.Count; i++)
            {
                var request = new RestRequest("/runners/create", Method.POST);

                request.AddParameter("source_code", source);
                request.AddParameter("language", languageCode);
                request.AddParameter("input", task.Inputs[i]);
                request.AddParameter("api_key", "guest");

                TaskCompletionSource<IRestResponse> taskCompletion = new TaskCompletionSource<IRestResponse>();
                client.ExecuteAsync(request, r => taskCompletion.SetResult(r));

                RestResponse response = (RestResponse)await taskCompletion.Task;

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    throw new ArgumentException("Error compiling task");

                dynamic data = JsonConvert.DeserializeObject(response.Content);

                var compileId = data.id;

                PaizaCompileDto result = await GetCompileResult(compileId.ToString(), task.Outputs[i], client);

                if (result.Compiled)
                {
                    if (!result.IsOutputCorrect)
                    {
                        compileDto.FailedInput = i+1;
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

            var request = new RestRequest("/runners/get_details", Method.GET);
            request.AddParameter("id", compileId);
            request.AddParameter("api_key", "guest");

            var taskCompletion = new TaskCompletionSource<IRestResponse>();
            client.ExecuteAsync(request, r => taskCompletion.SetResult(r));

            var response = (RestResponse)await taskCompletion.Task;

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new ArgumentException("Error compiling task");

            var data = JsonConvert.DeserializeObject<PaizaCompilerResultDto>(response.Content);

            if (data.Status.Equals("running"))
                return await GetCompileResult(compileId, expectedOutput, client);

            var expected = string.Join('\n', expectedOutput.Split('\n', '\r').Where(s => s.Length != 0).Select(s => s.Trim('\r', '\n', ' ')));
            var actual = string.Join('\n', data.Stdout.Split('\n', '\r').Where(s => s.Length != 0).Select(s => s.Trim('\r', '\n', ' ')));

            var compileResult = new PaizaCompileDto
            {
                Compiled = data.Result != null && data.Result.Equals("success"),
                Message = (string.IsNullOrEmpty(data.BuildStderr)) ? data.Stderr : data.BuildStderr,
                IsOutputCorrect = data.Stdout != null && actual.Equals(expected)
            };

            return compileResult;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CtsContestWeb.Dto;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;

namespace CtsContestWeb.Communication
{
    public class HackerRankCompiler : CompilerLanguages, ICompiler
    {
        readonly IConfiguration _configuration;

        public HackerRankCompiler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<CompileDto> Compile(TaskDto task, string source, int language)
        {
            var hackerRankUrl = _configuration["HackerRankUrl"];

            var client = new RestClient(hackerRankUrl);

            var request = new RestRequest("/checker/submission.json", Method.POST);

            var keysList = _configuration.GetSection("HackerRankApiKeys").GetChildren().Select(c => c.Value).ToList();
            var random = new Random();
            var element = random.Next(keysList.Count);

            request.AddParameter("source", source);
            request.AddParameter("lang", language);
            request.AddParameter("testcases", JsonConvert.SerializeObject(task.Inputs));
            request.AddParameter("api_key", keysList[element]);
            request.AddParameter("wait", "true");
            request.AddParameter("format", "json");

            TaskCompletionSource<IRestResponse> taskCompletion = new TaskCompletionSource<IRestResponse>();
            client.ExecuteAsync(request, r => taskCompletion.SetResult(r));

            RestResponse response = (RestResponse)await taskCompletion.Task;

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new ArgumentException("Error compiling task");

            dynamic data = JsonConvert.DeserializeObject(response.Content);

            var compileResult = new CompileDto
            {
                Compiled = data.result.result.Value.ToString() == "0",
                TotalInputs = task.Inputs.Count
            };

            if (compileResult.Compiled)
            {
                for (int i = 0; i < task.Outputs.Count; i++)
                {
                    if (task.Outputs[i].TrimEnd('\r', '\n', ' ') != data.result.stdout[i].Value.ToString().TrimEnd('\r', '\n', ' '))
                    {
                        compileResult.FailedInput = i+1; 
                        break; 
                    }
                }
            }
            else
            {
                var message = data.result.compilemessage.Value.ToString();
                compileResult.Message = Regex.Replace(message, @"[^\u0000-\u007F]+", string.Empty);
            }

            return compileResult;
        }

        /*public async Task<LanguageDto> GetLanguages()
        {
            var hackerRankUrl = _configuration["HackerRankUrl"];
            var client = new RestClient(hackerRankUrl);

            var request = new RestRequest("checker/languages.json", Method.GET);

            TaskCompletionSource<IRestResponse> taskCompletion = new TaskCompletionSource<IRestResponse>();
            client.ExecuteAsync(request, r => taskCompletion.SetResult(r));

            RestResponse response = (RestResponse)await taskCompletion.Task;

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new ArgumentException("Error getting languages");

            dynamic data = JsonConvert.DeserializeObject(response.Content);

            var languages = new LanguageDto();

            foreach (var item in data.languages.names)
            {
                languages.Names.Add(item.Name, item.Value.ToString());
            }

            foreach (var item in data.languages.codes)
            {
                languages.Codes.Add(item.Name, Int32.Parse(item.Value.ToString()));
            }

            return languages;
        }*/
    }
}

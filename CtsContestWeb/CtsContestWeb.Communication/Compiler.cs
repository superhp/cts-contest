using System;
using System.Threading.Tasks;
using CtsContestWeb.Dto;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;

namespace CtsContestWeb.Communication
{
    public class Compiler : ICompiler
    {
        IConfiguration _configuration;
        private ITask _taskManager;
        public Compiler(IConfiguration configuration, ITask taskManger)
        {
            _configuration = configuration;
            _taskManager = taskManger;
        }

        public async Task<CompileDto> Compile(int taskId, string source, int language)
        {
            var task = await _taskManager.GetTaskById(taskId);

            var hackerRankUrl = _configuration["HackerRankUrl"];

            var client = new RestClient(hackerRankUrl);

            var request = new RestRequest("/checker/submission.json", Method.POST);

            request.AddParameter("source", source);
            request.AddParameter("lang", language);
            request.AddParameter("testcases", JsonConvert.SerializeObject(task.Inputs));
            request.AddParameter("api_key", _configuration["HackerRankApiKey"]);
            request.AddParameter("wait", "true");
            request.AddParameter("format", "json");

            TaskCompletionSource<IRestResponse> taskCompletion = new TaskCompletionSource<IRestResponse>();
            client.ExecuteAsync(request, r => taskCompletion.SetResult(r));

            RestResponse response = (RestResponse)await taskCompletion.Task;

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new ArgumentException("Error compiling task");

            dynamic data = JsonConvert.DeserializeObject(response.Content);

            var compileResult = new CompileDto();

            compileResult.TotalInputs = task.Inputs.Count;

            for (int i = 0; i < task.Inputs.Count; i++)
            {
                if (task.Outputs[i] != data.result.stdout[i].Value.ToString())
                    compileResult.FailedInputs++;
            }

            return compileResult;
        }

        public async Task<LanguageDto> GetLanguages()
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
        }
    }
}

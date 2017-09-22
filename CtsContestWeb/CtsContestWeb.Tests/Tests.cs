using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CtsContestWeb.Dto;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RestSharp;

namespace CtsContestWeb.Tests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public async Task ShouldDeserializeLanguages()
        {
            var client = new RestClient("http://api.hackerrank.com");

            var request = new RestRequest("checker/languages.json", Method.GET);

            TaskCompletionSource<IRestResponse> taskCompletion = new TaskCompletionSource<IRestResponse>();
            client.ExecuteAsync(request, r => taskCompletion.SetResult(r));
            
            RestResponse response = (RestResponse)await taskCompletion.Task;

            
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new ArgumentException("Error getting languages");

            dynamic data = JsonConvert.DeserializeObject(response.Content);

            var result = new LanguageDto();

            foreach (var item in data.languages.names)
            {
                result.Names.Add(item.Name, item.Value.ToString());
            }

            foreach (var item in data.languages.codes)
            {
                result.Codes.Add(item.Name, Int32.Parse(item.Value.ToString()));
            }
        }

        [TestMethod]
        public async Task ShouldSolveProblem()
        {
            Console.WriteLine("Started");

            var client = new RestClient("http://api.hackerrank.com");

            var request = new RestRequest("/checker/submission.json", Method.POST);

            var task = new TaskDto();
            task.Inputs = new List<string>();
            task.Inputs.Add("1");
            task.Inputs.Add("2");
            task.Inputs.Add("3");

            task.Outputs = new List<string>();
            task.Outputs.Add("2\n");
            task.Outputs.Add("2\n");
            task.Outputs.Add("3\n");

            request.AddParameter("source", "print 2");
            request.AddParameter("lang", 5);
            request.AddParameter("testcases", JsonConvert.SerializeObject(task.Inputs));
            request.AddParameter("api_key", "hackerrank|2980862-1817|3684f35257abaa03dda0cc7a6564ea2d6bde13e3");
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
                Console.WriteLine(data.result.stdout[i].Value.ToString());
                if (task.Outputs[i] != data.result.stdout[i].Value.ToString())
                    compileResult.FailedInputs++;
            }
        }
    }
}

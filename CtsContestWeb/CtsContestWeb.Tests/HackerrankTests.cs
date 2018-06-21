using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CtsContestWeb.Communication;
using CtsContestWeb.Dto;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RestSharp;

namespace CtsContestWeb.Tests
{
    [TestClass]
    public class HackerrankTests
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
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var configuration = builder.Build();

            var compiler = new HackerRankCompiler(configuration);

            var result = await compiler.Compile(TaskData.GetTestTask(), "print 2", 5);

            Assert.AreEqual(true, result.Compiled);
            Assert.AreEqual(true, result.ResultCorrect);
        }

        [TestMethod]
        public async Task ShouldNotSolveProblem()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var configuration = builder.Build();

            var compiler = new HackerRankCompiler(configuration);

            var result = await compiler.Compile(TaskData.GetTestTask(), "print 5", 5);

            Assert.AreEqual(true, result.Compiled);
            Assert.AreEqual(false, result.ResultCorrect);
        }
    }
}

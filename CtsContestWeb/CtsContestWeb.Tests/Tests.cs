using System;
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
    }
}

﻿using System;
using System.Threading.Tasks;
using CtsContestWeb.Dto;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;

namespace CtsContestWeb.Communication
{
    public class Compiler : ICompiler
    {
        IConfiguration _iconfiguration;

        public Compiler(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;
        }
        public void Compile(string source, string[] inputs)
        {
            throw new NotImplementedException();
        }

        public async Task<LanguageDto> GetLanguages()
        {
            var hackerRankUrl = _iconfiguration["HackerRankUrl"];
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

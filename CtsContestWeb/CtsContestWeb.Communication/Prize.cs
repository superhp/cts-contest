using CtsContestWeb.Dto;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;

namespace CtsContestWeb.Communication
{
    public class Prize : IPrize
    {
        IConfiguration _iconfiguration;
        public Prize(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;
        }

        public List<PrizeDto> GetAllPrizes()
        {
            var umbracoApiUrl = _iconfiguration["UmbracoApiUrl"];
            var client = new RestClient(umbracoApiUrl);

            var request = new RestRequest("task/getAll", Method.GET);

            var response = client.Execute<List<PrizeDto>>(request);

            return response.Data;
        }

        public PrizeDto GetPrizeById(int id)
        {
            var umbracoApiUrl = _iconfiguration["UmbracoApiUrl"];
            var client = new RestClient(umbracoApiUrl);

            var request = new RestRequest("task/get/{id}", Method.GET);
            request.AddUrlSegment("id", id.ToString());

            var response = client.Execute<PrizeDto>(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new ArgumentException("No task with given ID");

            return response.Data;
        }
    }
}

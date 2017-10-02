using CtsContestWeb.Dto;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CtsContestWeb.Communication
{
    public class PrizeManager : IPrizeManager
    {
        IConfiguration _iconfiguration;
        public PrizeManager(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;
        }

        public async Task<List<PrizeDto>> GetAllPrizes()
        {
            var umbracoApiUrl = _iconfiguration["UmbracoApiUrl"];
            var pictureUrl = _iconfiguration["UmbracoPictureUrl"];
            var client = new RestClient(umbracoApiUrl);

            var request = new RestRequest("prize/getAll", Method.GET);

            TaskCompletionSource<List<PrizeDto>> taskCompletion = new TaskCompletionSource<List<PrizeDto>>();
            client.ExecuteAsync<List<PrizeDto>>(request, response =>
            {
                taskCompletion.SetResult(response.Data);
            });
            var prizes = await taskCompletion.Task;
            foreach (var item in prizes)
            {
                item.Picture = pictureUrl + item.Picture;
            }
            return prizes;
        }

        public async Task<PrizeDto> GetPrizeById(int id)
        {
            var umbracoApiUrl = _iconfiguration["UmbracoApiUrl"];
            var pictureUrl = _iconfiguration["UmbracoPictureUrl"];
            var client = new RestClient(umbracoApiUrl);

            var request = new RestRequest("prize/get/{id}", Method.GET);
            request.AddUrlSegment("id", id.ToString());

            TaskCompletionSource<PrizeDto> taskCompletion = new TaskCompletionSource<PrizeDto>();
            client.ExecuteAsync<PrizeDto>(request, response =>
            {
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    throw new ArgumentException("No prize with given ID");
                taskCompletion.SetResult(response.Data);
            });

            var prize = await taskCompletion.Task;
            prize.Picture = pictureUrl + prize.Picture;

            return prize;
        }
    }
}

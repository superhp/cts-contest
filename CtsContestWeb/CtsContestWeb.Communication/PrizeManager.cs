using CtsContestWeb.Dto;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CtsContestWeb.Db.Repository;

namespace CtsContestWeb.Communication
{
    public class PrizeManager : IPrizeManager
    {
        private readonly IConfiguration _configuration;
        private readonly IPurchaseRepository _purchaseRepository;

        public PrizeManager(IConfiguration configuration, IPurchaseRepository purchaseRepository)
        {
            _configuration = configuration;
            _purchaseRepository = purchaseRepository;
        }

        public async Task<List<PrizeDto>> GetAllPrizesForPoints()
        {
            var umbracoApiUrl = _configuration["UmbracoApiUrl"];
            var pictureUrl = _configuration["UmbracoPictureUrl"];
            var client = new RestClient(umbracoApiUrl);

            var request = new RestRequest("prize/getAll", Method.GET);

            TaskCompletionSource<List<PrizeDto>> taskCompletion = new TaskCompletionSource<List<PrizeDto>>();
            client.ExecuteAsync<List<PrizeDto>>(request, response =>
            {
                taskCompletion.SetResult(response.Data);
            });

            var prizes = await taskCompletion.Task;
            var prizesForPoints = prizes.Where(p => p.Category.Equals("Prize for points")).ToList();
            var purchases = _purchaseRepository.GetAll().ToList();

            foreach (var item in prizesForPoints)
            {
                item.Picture = pictureUrl + item.Picture;
                item.Quantity -= purchases.Count(np => np.PrizeId == item.Id);
            }

            return prizesForPoints;
        }

        public async Task<PrizeDto> GetPrizeById(int id)
        {
            var umbracoApiUrl = _configuration["UmbracoApiUrl"];
            var pictureUrl = _configuration["UmbracoPictureUrl"];
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
            var purchases = _purchaseRepository.GetAll();
            var purchasesCount = purchases.Count(p => p.PrizeId == prize.Id);

            prize.Quantity -= purchasesCount;
            prize.Picture = pictureUrl + prize.Picture;

            return prize;
        }
    }
}

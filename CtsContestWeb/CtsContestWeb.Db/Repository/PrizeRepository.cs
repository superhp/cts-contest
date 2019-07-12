using CtsContestWeb.Db.Entities;
using CtsContestWeb.Dto;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CtsContestWeb.Db.Repository
{
    public class PrizeRepository : IPrizeRepository
    {
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IMemoryCache _cache;
        private readonly ApplicationDbContext _dbContext;

        public PrizeRepository(IPurchaseRepository purchaseRepository, IMemoryCache cache, ApplicationDbContext dbContext)
        {
            _purchaseRepository = purchaseRepository;
            _cache = cache;
            _dbContext = dbContext;
        }
        public List<PrizeDto> GetAllWinnablePrizes()
        {
            List<PrizeDto> winnablePrizes;
            if (!_cache.TryGetValue<List<PrizeDto>>("winnablePrizes", out winnablePrizes))
            {
                winnablePrizes = CacheWinnablePrizes();
            }

            return winnablePrizes;
        }

        public List<PrizeDto> CacheWinnablePrizes()
        {
            var winnablePrizes = GetAllWinnablePrizesFromDb();

            MemoryCacheEntryOptions cacheExpirationOptions = new MemoryCacheEntryOptions();
            cacheExpirationOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(60);
            cacheExpirationOptions.Priority = CacheItemPriority.Normal;
            _cache.Set<List<PrizeDto>>("winnablePrizes", winnablePrizes, cacheExpirationOptions);

            return winnablePrizes;
        }

        private List<PrizeDto> GetAllWinnablePrizesFromDb()
        {

            var prizes = _dbContext.Prizes.ToList();
            var winnablePrizes = prizes.Where(p => !p.Category.Equals("Prize for points")).ToList();
            var purchases = _purchaseRepository.GetAll().ToList();
            var winnableDtoPrizes = new List<PrizeDto>();

            foreach (var item in winnablePrizes)
            {
                var dtoItem = MapToDto(item);
                dtoItem.Quantity = purchases.Count(np => np.PrizeId == item.Id);
                winnableDtoPrizes.Add((dtoItem));
            }
            return winnableDtoPrizes;
        }

        public List<PrizeDto> GetAllPrizesForPoints()
        {
            var prizes = _dbContext.Prizes.ToList();
            var prizesForPoints = prizes.Where(p => p.Category.Equals("Prize for points")).ToList();
            var purchases = _purchaseRepository.GetAll().ToList();
            var winnableDtoPrizes = new List<PrizeDto>();

            foreach (var item in prizesForPoints)
            {
                var dtoItem = MapToDto(item);
                var purchaseCount = purchases.Count(np => np.PrizeId == item.Id);
                dtoItem.Quantity = dtoItem.Quantity - purchaseCount;
                winnableDtoPrizes.Add(dtoItem);
            }
            return winnableDtoPrizes;
        }

        public PrizeDto GetPrizeById(int id)
        {
            var prize = _dbContext.Prizes.Find(id);
            if (prize == null)
            {
                throw new ArgumentException("No prize with given ID");
            }

            var purchases = _purchaseRepository.GetAll();
            var purchasesCount = purchases.Count(p => p.PrizeId == prize.Id);
            var dtoPrize = MapToDto(prize);
            dtoPrize.Quantity -= purchasesCount;

            return dtoPrize;
        }

        private PrizeDto MapToDto(Prize prize)
        {
            return new PrizeDto
            {
                Name = prize.Name,
                Id = prize.Id,
                Category = prize.Category,
                Description = prize.Description,
                Picture = prize.Picture,
                Price = prize.Price,
                Quantity = prize.Quantity,
            };
        }
    }
}

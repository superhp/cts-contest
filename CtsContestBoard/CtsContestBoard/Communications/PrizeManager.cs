using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CtsContestBoard.Db;
using CtsContestBoard.Db.Entities;
using CtsContestBoard.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RestSharp;

namespace CtsContestBoard.Communications
{
    public class PrizeManager : IPrizeManager
    {
        private readonly ApplicationDbContext _dbContext;

        public PrizeManager(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<PrizeDto> GetAllPrizes()
        {
            var prizes = _dbContext.Prizes.ToList();

            var prizesDto = new List<PrizeDto>();

            foreach (var item in prizes)
            {
                var dtoItem = MapToDto(item);
                prizesDto.Add((dtoItem));
            }

            return prizesDto;
        }

        public PrizeDto GetPrizeById(int id)
        {

            var prize = _dbContext.Prizes.Find(id);
            if (prize == null)
            {
                throw new ArgumentException("No prize with given ID");
            }

            var dtoPrize = MapToDto(prize);
            return dtoPrize;
        }
        private PrizeDto MapToDto(Prize prize)
        {
            return new PrizeDto
            {
                Name = prize.Name,
                Id = prize.Id,
                Category = prize.Category,
                Picture = prize.Picture,
                Price = prize.Price,
                Quantity = prize.Quantity,
            };
        }
    }
}

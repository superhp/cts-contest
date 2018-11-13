using System;
using System.Collections.Generic;
using System.Linq;
using CtsContestBoard.Db.Entities;
using CtsContestBoard.Dto;
using CtsContestWeb.Db.Entities;
using Microsoft.EntityFrameworkCore;

namespace CtsContestBoard.Db.Repository
{
    public class DuelRepository : IDuelRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DuelRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public DuelSolution GetSolution(int competitionId, string userEmail)
        {
            return _dbContext.DuelSolutions.SingleOrDefault(cs =>
                cs.DuelId == competitionId && cs.UserEmail == userEmail);
        }

        public IEnumerable<DuelDto> GetWonDuelsByEmail(string userEmail)
        {
            return _dbContext.Duels
                .Where(c => c.WinnerEmail.Equals(userEmail)).AsEnumerable()
                .Select(DuelToDuelDto);
        }

        public IEnumerable<DuelDto> GetDuelsByEmail(string email)
        {
            return _dbContext.Duels
                .Where(c => c.FirstPlayerEmail.Equals(email) || c.SecondPlayerEmail.Equals(email)).AsEnumerable()
                .Select(DuelToDuelDto);
        }

        public IEnumerable<DuelDto> GetLostDuelsByEmail(string userEmail)
        {
            return _dbContext.Duels
                .Where(c => (c.FirstPlayerEmail.Equals(userEmail) || c.SecondPlayerEmail.Equals(userEmail)) &&
                            c.WinnerEmail != null && !c.WinnerEmail.Equals(userEmail)).AsEnumerable()
                .Select(DuelToDuelDto);
        }

        public IEnumerable<DuelDto> GetAllDuelsWithWinner()
        {
            return _dbContext.Duels.Where(d => !string.IsNullOrEmpty(d.WinnerEmail)).AsEnumerable().Select(DuelToDuelDto);
        }

        private DuelDto DuelToDuelDto(Duel c)
        {
            return new DuelDto
            {
                Prize = c.Prize,
                Id = c.DuelId,
                Players = new List<string>
                {
                    c.FirstPlayerEmail,
                    c.SecondPlayerEmail
                },
                Winner = c.WinnerEmail,
                StartTime = c.Created
            };
        }
    }
}
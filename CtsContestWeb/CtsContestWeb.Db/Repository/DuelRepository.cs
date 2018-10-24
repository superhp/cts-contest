using System;
using System.Collections.Generic;
using System.Linq;
using CtsContestWeb.Db.Entities;
using CtsContestWeb.Dto;
using Microsoft.EntityFrameworkCore;

namespace CtsContestWeb.Db.Repository
{
    public class DuelRepository : IDuelRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DuelRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int CreateDuel(DuelDto competitionDto)
        {
            var competition = new Duel
            {
                FirstPlayerEmail = competitionDto.Players[0].Email,
                SecondPlayerEmail = competitionDto.Players[1].Email,
                Prize = competitionDto.Prize,
                TaskId = competitionDto.Task.Id
            };

            _dbContext.Duels.Add(competition);
            _dbContext.SaveChanges();

            return competition.DuelId;
        }

        public void SetWinner(DuelDto competitionDto, PlayerDto winner)
        {
            var competition = _dbContext.Duels.Find(competitionDto.Id);

            if (string.IsNullOrEmpty(competition.WinnerEmail))
                competition.WinnerEmail = winner.Email;
            else
                throw new ArgumentException($"Duel {competitionDto.Id} already has a winner");

            _dbContext.SaveChanges();
        }

        public DuelSolution GetSolution(int competitionId, string userEmail)
        {
            return _dbContext.DuelSolutions.SingleOrDefault(cs =>
                cs.DuelId == competitionId && cs.UserEmail == userEmail);
        }

        public void UpsertSolution(DuelSolution solution)
        {
            _dbContext.Entry(solution).State = solution.DuelSolutionId == 0 ?
                EntityState.Added :
                EntityState.Modified;
            _dbContext.SaveChanges();
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

        private DuelDto DuelToDuelDto(Duel c)
        {
            return new DuelDto
            {
                Prize = c.Prize,
                Players = new List<PlayerDto>
                {
                    new PlayerDto
                    {
                        Email = c.FirstPlayerEmail
                    },
                    new PlayerDto
                    {
                        Email = c.SecondPlayerEmail
                    }
                },
                Task = new TaskDto
                {
                    Id = c.TaskId
                },
                Id = c.DuelId
            };
        }
    }
}
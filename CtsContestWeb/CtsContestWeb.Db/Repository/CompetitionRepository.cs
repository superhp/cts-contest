using System;
using System.Collections.Generic;
using System.Linq;
using CtsContestWeb.Db.Entities;
using CtsContestWeb.Dto;
using Microsoft.EntityFrameworkCore;

namespace CtsContestWeb.Db.Repository
{
    public class CompetitionRepository : ICompetitionRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CompetitionRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int CreateCompetition(CompetitionDto competitionDto)
        {
            var competition = new Competition
            {
                FirstPlayerEmail = competitionDto.Players[0].Email,
                SecondPlayerEmail = competitionDto.Players[1].Email,
                Prize = competitionDto.Prize,
                TaskId = competitionDto.Task.Id
            };

            _dbContext.Competitions.Add(competition);
            _dbContext.SaveChanges();

            return competition.CompetitionId;
        }

        public void SetWinner(CompetitionDto competitionDto, PlayerDto winner)
        {
            var competition = _dbContext.Competitions.Find(competitionDto.Id);

            if (string.IsNullOrEmpty(competition.WinnerEmail))
                competition.WinnerEmail = winner.Email;
            else
                throw new ArgumentException($"Competition {competitionDto.Id} already has a winner");

            _dbContext.SaveChanges();
        }

        public CompetitionSolution GetSolution(int competitionId, string userEmail)
        {
            return _dbContext.CompetitionSolutions.SingleOrDefault(cs =>
                cs.CompetitionId == competitionId && cs.UserEmail == userEmail);
        }

        public void UpsertSolution(CompetitionSolution solution)
        {
            _dbContext.Entry(solution).State = solution.CompetitionSolutionId == 0 ?
                EntityState.Added :
                EntityState.Modified;
            _dbContext.SaveChanges();
        }

        public IEnumerable<CompetitionDto> GetWonCompetitionsByEmail(string userEmail)
        {
            return _dbContext.Competitions
                .Where(c => c.WinnerEmail.Equals(userEmail)).AsEnumerable()
                .Select(CompetitionToCompetitionDto);
        }

        public IEnumerable<CompetitionDto> GetCompetitionsByEmail(string email)
        {
            return _dbContext.Competitions
                .Where(c => c.FirstPlayerEmail.Equals(email) || c.SecondPlayerEmail.Equals(email)).AsEnumerable()
                .Select(CompetitionToCompetitionDto);
        }

        private CompetitionDto CompetitionToCompetitionDto(Competition c)
        {
            return new CompetitionDto
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
                Id = c.CompetitionId
            };
        }
    }
}
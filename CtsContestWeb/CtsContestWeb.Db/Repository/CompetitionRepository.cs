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

            competition.WinnerEmail = winner.Email;

            _dbContext.SaveChanges();
        }

        public CompetitionSolution GetSolution(int competitionId, string userEmail)
        {
            return _dbContext.CompetitionSolutions.Single(cs =>
                cs.CompetitionId == competitionId && cs.UserEmail == userEmail);
        }

        public void UpsertSolution(CompetitionSolution solution)
        {
            _dbContext.Entry(solution).State = solution.CompetitionId == 0 ?
                EntityState.Added :
                EntityState.Modified;
            _dbContext.SaveChanges();
        }
    }
}
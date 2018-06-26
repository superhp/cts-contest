using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.Threading.Tasks;
using CtsContestWeb.Communication;
using CtsContestWeb.Db.Repository;
using CtsContestWeb.Dto;
using CtsContestWeb.Logic;

namespace CtsContestWeb.Competition
{
    public class CompetitionHub : Hub
    {
        private readonly ITaskManager _taskManager;
        private readonly ICompetitionRepository _competitionRepository;
        private readonly ISolutionLogic _solutionLogic;

        public CompetitionHub(ITaskManager taskManager, ICompetitionRepository competitionRepository, ISolutionLogic solutionLogic)
        {
            _taskManager = taskManager;
            _competitionRepository = competitionRepository;
            _solutionLogic = solutionLogic;
        }

        public override async Task OnConnectedAsync()
        {
            var firstPlayer = new PlayerDto
            {
                ConnectionId = Context.ConnectionId,
                Email = Context.User.FindFirst(ClaimTypes.Email).Value,
                Name = Context.User.FindFirst(ClaimTypes.GivenName).Value
            };

            if (UserHandler.WaitingPlayers.Count > 0)
            {
                var secondPlayer = UserHandler.WaitingPlayers.First();
                UserHandler.WaitingPlayers.Remove(secondPlayer);

                // TODO: prize amount calculation
                var competition = new CompetitionDto
                {
                    Prize = 100
                };
                competition.Players.Add(firstPlayer);
                competition.Players.Add(secondPlayer);
                competition.Task = await _taskManager.GetTaskForCompetition(competition.Players.Select(p => p.Email));
                competition.Id = _competitionRepository.CreateCompetition(competition);

                UserHandler.ActiveCompetitions.Add(competition);

                await Groups.AddToGroupAsync(firstPlayer.ConnectionId, competition.GroupName);
                await Groups.AddToGroupAsync(secondPlayer.ConnectionId, competition.GroupName);
                await Clients.Group(competition.GroupName).SendAsync("competitionStarts", competition);
            }
            else
            {
                UserHandler.WaitingPlayers.Add(firstPlayer);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var competition = GetCurrentCompetition();

            if (competition != null)
            {
                UserHandler.ActiveCompetitions.Remove(competition);

                var winner = competition.Players.Single(p => !p.ConnectionId.Equals(Context.ConnectionId));
                await Clients.User(winner.ConnectionId).SendAsync("opponentDisconnected");

                await Groups.RemoveFromGroupAsync(Context.ConnectionId, competition.GroupName);
                await Groups.RemoveFromGroupAsync(winner.ConnectionId, competition.GroupName);

                _competitionRepository.SetWinner(competition, winner);
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task CheckSolution(string source, int language)
        {
            var competition = GetCurrentCompetition();
            var player = competition.Players.First(p => p.ConnectionId.Equals(Context.ConnectionId));

            var compileResult = await _solutionLogic.CheckSolution(competition.Task.Id, source, language);

            _solutionLogic.SaveCompetitionSolution(competition.Id, source, player.Email, language, compileResult.ResultCorrect);
            if (compileResult.ResultCorrect)
            {
                // send result to users
                // mark competition as ended
            }
            else
            {
                await Clients.User(Context.ConnectionId).SendAsync("checkedSolution", compileResult);
            }
        }

        private CompetitionDto GetCurrentCompetition()
        {
            return UserHandler.ActiveCompetitions.FirstOrDefault(c =>
                c.Players.Select(p => p.ConnectionId).Contains(Context.ConnectionId));
        }
    }
}
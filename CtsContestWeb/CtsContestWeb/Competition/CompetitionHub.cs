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
            if (IsAlreadyConnected())
            {
                await Clients.Caller.SendAsync("closeThisWindow");
                return;
            }

            var firstPlayer = new PlayerDto
            {
                ConnectionId = Context.ConnectionId,
                Email = Context.User.FindFirst(ClaimTypes.Email).Value,
                Name = Context.User.FindFirst(ClaimTypes.GivenName).Value
            };

            if (UserHandler.WaitingPlayers.Count > 0)
            {
                PlayerDto secondPlayer = null;
                TaskDto task = null;
                for (int i = 0; i < UserHandler.WaitingPlayers.Count; i++)
                {
                    secondPlayer = UserHandler.WaitingPlayers.Skip(i).First();

                    task = await _taskManager.GetTaskForCompetition(new List<string> { firstPlayer.Email, secondPlayer.Email });
                    if (task != null)
                        break;
                }

                if (task == null)
                {
                    UserHandler.WaitingPlayers.Add(firstPlayer);
                    return;
                }
              
                UserHandler.WaitingPlayers.Remove(secondPlayer);

                var competition = new CompetitionDto
                {
                    Prize = task.Value,
                    Players = new List<PlayerDto>
                    {
                        firstPlayer,
                        secondPlayer
                    },
                    Task = task
                };
                
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

        private bool IsAlreadyConnected()
        {
            var email = Context.User.FindFirst(ClaimTypes.Email).Value;

            if (UserHandler.WaitingPlayers.Select(wp => wp.Email).Contains(email))
                return true;

            if (UserHandler.ActiveCompetitions.SelectMany(ac => ac.Players).Select(p => p.Email).Contains(email))
                return true;

            return false;
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var competition = GetCurrentCompetition();

            if (competition != null)
            {
                UserHandler.ActiveCompetitions.Remove(competition);

                var winner = competition.Players.Single(p => !p.ConnectionId.Equals(Context.ConnectionId));

                await Groups.RemoveFromGroupAsync(Context.ConnectionId, competition.GroupName);
                //Currently calling Group (as the other person is removed). Should be changed to call just the winner
                await Clients.Group(competition.GroupName).SendAsync("opponentDisconnected", winner);
                await Clients.Group(competition.GroupName).SendAsync("scoreAdded");
                await Groups.RemoveFromGroupAsync(winner.ConnectionId, competition.GroupName);

                _competitionRepository.SetWinner(competition, winner);
            }

            await base.OnDisconnectedAsync(exception);
        }

        [HubMethodName("CheckSolution")]
        public async Task CheckSolution(string source, int language)
        {
            var competition = GetCurrentCompetition();
            var player = competition.Players.First(p => p.ConnectionId.Equals(Context.ConnectionId));

            var compileResult = await _solutionLogic.CheckSolution(competition.Task.Id, source, language);

            _solutionLogic.SaveCompetitionSolution(competition.Id, source, player.Email, language, compileResult.Compiled && compileResult.ResultCorrect);
            if (compileResult.Compiled && compileResult.ResultCorrect)
            {
                await Clients.Group(competition.GroupName).SendAsync("competitionHasWinner", player);
                await Clients.Caller.SendAsync("scoreAdded");

                await Groups.RemoveFromGroupAsync(competition.Players[0].ConnectionId, competition.GroupName);
                await Groups.RemoveFromGroupAsync(competition.Players[1].ConnectionId, competition.GroupName);

                _competitionRepository.SetWinner(competition, player);
            }
            else
            {
                await Clients.Caller.SendAsync("solutionChecked", compileResult);
            }
        }

        private CompetitionDto GetCurrentCompetition()
        {
            return UserHandler.ActiveCompetitions.FirstOrDefault(c =>
                c.Players.Select(p => p.ConnectionId).Contains(Context.ConnectionId));
        }
    }
}
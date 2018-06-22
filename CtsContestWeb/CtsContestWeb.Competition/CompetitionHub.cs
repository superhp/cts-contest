using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.Threading.Tasks;
using CtsContestWeb.Communication;
using CtsContestWeb.Dto;

namespace CtsContestWeb.Competition
{
    public class CompetitionHub : Hub
    {
        private readonly ITaskManager _taskManager;

        public CompetitionHub(ITaskManager taskManager)
        {
            _taskManager = taskManager;
        }

        public override async Task OnConnectedAsync()
        {
            var firstPlayer = new Player
            {
                ConnectionId = Context.ConnectionId,
                Email = Context.User.FindFirst(ClaimTypes.Email).Value,
                Name = Context.User.FindFirst(ClaimTypes.GivenName).Value
            };

            if (UserHandler.WaitingPlayers.Count > 0)
            {
                var secondPlayer = UserHandler.WaitingPlayers.First();
                UserHandler.WaitingPlayers.Remove(secondPlayer);

                var competition = new Competition();
                competition.Players.Add(firstPlayer);
                competition.Players.Add(secondPlayer);

                UserHandler.ActiveCompetitions.Add(competition);

                await Groups.AddToGroupAsync(firstPlayer.ConnectionId, competition.GroupName);
                await Groups.AddToGroupAsync(secondPlayer.ConnectionId, competition.GroupName);

                competition.Task = await _taskManager.GetTaskForCompetition(competition.Players.Select(p => p.Email));

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
            var competition = UserHandler.ActiveCompetitions.FirstOrDefault(c =>
                c.Players.Select(p => p.ConnectionId).Contains(Context.ConnectionId));
            if (competition != null)
            {
                UserHandler.ActiveCompetitions.Remove(competition);

                var winner = competition.Players.Single(p => !p.ConnectionId.Equals(Context.ConnectionId));
                await Clients.User(winner.ConnectionId).SendAsync("opponentDisconnected");

                await Groups.RemoveFromGroupAsync(Context.ConnectionId, competition.GroupName);
                await Groups.RemoveFromGroupAsync(winner.ConnectionId, competition.GroupName);

                // TODO: SET COMPETITION WINNER
            }

            await base.OnDisconnectedAsync(exception);
        }

    }
}
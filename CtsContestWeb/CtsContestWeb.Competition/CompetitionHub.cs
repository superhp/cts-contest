using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CtsContestWeb.Competition
{
    public class CompetitionHub : Hub
    {
        public CompetitionHub()
        {
            
        }

        private string UserEmail => Context.User.FindFirst(ClaimTypes.Email).Value;

        public override async Task OnConnectedAsync()
        {
            var firstPlayer = new Player
            {
                ConnectionId = Context.ConnectionId,
                Email = UserEmail
            };
            
            if (UserHandler.WaitingPlayers.Count > 0)
            {
                var secondPlayer = UserHandler.WaitingPlayers.First();
                UserHandler.WaitingPlayers.Remove(secondPlayer);

                var competition = new Competition
                {
                    FirstPlayer = firstPlayer,
                    SecondPlayer = secondPlayer
                };

                await Groups.AddToGroupAsync(firstPlayer.ConnectionId, competition.GroupName);
                await Groups.AddToGroupAsync(secondPlayer.ConnectionId, competition.GroupName);

                await Clients.Group(competition.GroupName).SendAsync("competitionStarts");
            }
            else
            {
                UserHandler.WaitingPlayers.Add(firstPlayer);
            }
            
            await base.OnConnectedAsync();
        }



    }

    public static class UserHandler
    {
        public static HashSet<Player> WaitingPlayers = new HashSet<Player>();
        public static List<Competition> ActiveCompetitions = new List<Competition>();
    }

    public class Player
    {
        public string Email { get; set; }
        public string ConnectionId { get; set; }
    }

    public class Competition
    {
        public Competition()
        {
            StartTime = DateTime.Now;
        }

        public Player FirstPlayer { get; set; }
        public Player SecondPlayer { get; set; }
        public DateTime StartTime { get; }

        public string GroupName => FirstPlayer.Email + SecondPlayer.Email;
    }
}
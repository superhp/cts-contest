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

namespace CtsContestWeb.Duel
{
    public class DuelHub : Hub
    {
        private const string WaitingPlayersGroup = "WaitingPlayerGroups"; 
        private readonly ITaskManager _taskManager;
        private readonly IDuelRepository _duelRepository;
        private readonly ISolutionLogic _solutionLogic;

        public DuelHub(ITaskManager taskManager, IDuelRepository duelRepository, ISolutionLogic solutionLogic)
        {
            _taskManager = taskManager;
            _duelRepository = duelRepository;
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
                Name = Context.User.FindFirst(ClaimTypes.GivenName).Value
            };

            if (UserHandler.WaitingPlayers.Count > 0)
            {
                PlayerDto secondPlayer = null;
                TaskDto task = null;
                for (int i = 0; i < UserHandler.WaitingPlayers.Count; i++)
                {
                    secondPlayer = UserHandler.WaitingPlayers.Skip(i).First();

                    task = await _taskManager.GetTaskForDuel(new List<string> { firstPlayer.Email, secondPlayer.Email });
                    if (task != null)
                        break;
                }

                if (task == null)
                {
                    AddWaitingPlayer(firstPlayer);
                    return;
                }
              
                RemoveWaitingPlayer(secondPlayer);

                var duel = new DuelDto
                {
                    Prize = task.Value,
                    Players = new List<PlayerDto>
                    {
                        firstPlayer,
                        secondPlayer
                    },
                    Task = task
                };

                Duel.Players.ForEach(player =>
                {
                    var totalWins = _DuelRepository.GetWonDuelsByEmail(player.Email).Count();
                    var totalLooses = _DuelRepository.GetDuelsByEmail(player.Email).Count() - totalWins;
                    player.TotalWins = totalWins;
                    player.TotalLooses = totalLooses;
                });
                
                duel.Id = _duelRepository.CreateDuel(duel);
                UserHandler.ActiveDuels.Add(duel);

                await Groups.AddToGroupAsync(firstPlayer.ConnectionId, duel.GroupName);
                await Groups.AddToGroupAsync(secondPlayer.ConnectionId, duel.GroupName);
                await Clients.Group(duel.GroupName).SendAsync("DuelStarts", duel);
            }
            else
            {
                AddWaitingPlayer(firstPlayer);
            }

            await base.OnConnectedAsync();
        }

        private bool IsAlreadyConnected()
        {
            var email = Context.User.FindFirst(ClaimTypes.Email).Value;

            if (UserHandler.WaitingPlayers.Select(wp => wp.Email).Contains(email))
                return true;

            if (UserHandler.ActiveDuels.SelectMany(ac => ac.Players).Select(p => p.Email).Contains(email))
                return true;

            return false;
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var duel = GetCurrentDuel();

            if (duel != null)
            {
                UserHandler.ActiveDuels.Remove(duel);

                var winner = duel.Players.Single(p => !p.ConnectionId.Equals(Context.ConnectionId));

                await Groups.RemoveFromGroupAsync(Context.ConnectionId, duel.GroupName);
                //Currently calling Group (as the other person is removed). Should be changed to call just the winner
                await Clients.Group(duel.GroupName).SendAsync("opponentDisconnected", winner);
                await Clients.Group(duel.GroupName).SendAsync("scoreAdded");
                await Groups.RemoveFromGroupAsync(winner.ConnectionId, duel.GroupName);

                _duelRepository.SetWinner(duel, winner);
                UserHandler.ActiveDuels.Remove(duel);
            }
            else
            {
                var player =
                    UserHandler.WaitingPlayers.FirstOrDefault(wp => wp.ConnectionId.Equals(Context.ConnectionId));

                if (player != null)
                    RemoveWaitingPlayer(player);
            }

            await base.OnDisconnectedAsync(exception);
        }

        [HubMethodName("CheckSolution")]
        public async Task CheckSolution(string source, int language)
        {
            var duel = GetCurrentDuel();
            var player = duel.Players.First(p => p.ConnectionId.Equals(Context.ConnectionId));

            var compileResult = await _solutionLogic.CheckSolution(duel.Task.Id, source, language);

            _solutionLogic.SaveDuelSolution(duel.Id, source, player.Email, language, compileResult.Compiled && compileResult.ResultCorrect);
            if (compileResult.Compiled && compileResult.ResultCorrect)
            {
                await Clients.Group(duel.GroupName).SendAsync("DuelHasWinner", player);
                await Clients.Caller.SendAsync("scoreAdded");

                await Groups.RemoveFromGroupAsync(duel.Players[0].ConnectionId, duel.GroupName);
                await Groups.RemoveFromGroupAsync(duel.Players[1].ConnectionId, duel.GroupName);

                _duelRepository.SetWinner(duel, player);
                UserHandler.ActiveDuels.Remove(duel);
            }
            else
            {
                await Clients.Caller.SendAsync("solutionChecked", compileResult);
            }
        }

        private DuelDto GetCurrentDuel()
        {
            return UserHandler.ActiveDuels.FirstOrDefault(c =>
                c.Players.Select(p => p.ConnectionId).Contains(Context.ConnectionId));
        }

        private async void AddWaitingPlayer(PlayerDto playerDto)
        {
            UserHandler.WaitingPlayers.Add(playerDto);
            await Groups.AddToGroupAsync(playerDto.ConnectionId, WaitingPlayersGroup);
            await Clients.Group(WaitingPlayersGroup).SendAsync("waitingPlayers", UserHandler.WaitingPlayers.Count);
        }

        private async void RemoveWaitingPlayer(PlayerDto playerDto)
        {
            UserHandler.WaitingPlayers.Remove(playerDto);
            await Groups.RemoveFromGroupAsync(playerDto.ConnectionId, WaitingPlayersGroup);
            await Clients.Group(WaitingPlayersGroup).SendAsync("waitingPlayers", UserHandler.WaitingPlayers.Count);
        }
    }
}
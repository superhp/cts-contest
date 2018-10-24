using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Timers;
using CtsContestWeb.Communication;
using CtsContestWeb.Db.Repository;
using CtsContestWeb.Dto;
using CtsContestWeb.Logic;
using Microsoft.Extensions.Configuration;

namespace CtsContestWeb.Duel
{
    public class DuelHub : Hub
    {
        private const string WaitingPlayersGroup = "WaitingPlayerGroups"; 
        private readonly ITaskManager _taskManager;
        private readonly IDuelRepository _duelRepository;
        private readonly ISolutionLogic _solutionLogic;
        private readonly IConfiguration _configuration;

        public DuelHub(ITaskManager taskManager, IDuelRepository duelRepository, ISolutionLogic solutionLogic, IConfiguration configuration)
        {
            _taskManager = taskManager;
            _duelRepository = duelRepository;
            _solutionLogic = solutionLogic;
            _configuration = configuration;
        }

        public override async Task OnConnectedAsync()
        {
            if (IsAlreadyConnected())
            {
                var duel = GetCurrentDuel();
                var player = new PlayerDto
                {
                    ConnectionId = Context.ConnectionId,
                    Name = Context.User.FindFirstValue(ClaimTypes.GivenName),
                    Email = Context.User.FindFirstValue(ClaimTypes.Email)
                };
                await Groups.AddToGroupAsync(player.ConnectionId, duel.GroupName);
                await Clients.Caller.SendAsync("DuelStarts", duel);
                return;
            }
           
            var firstPlayer = new PlayerDto
            {
                ConnectionId = Context.ConnectionId,
                Name = Context.User.FindFirstValue(ClaimTypes.GivenName),
                Email = Context.User.FindFirstValue(ClaimTypes.Email)
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

                duel.Players.ForEach(player =>
                {
                    player.TotalWins = _duelRepository.GetWonDuelsByEmail(player.Email).Count();
                    player.TotalLooses = _duelRepository.GetLostDuelsByEmail(player.Email).Count();
                });
                
                duel.Id = _duelRepository.CreateDuel(duel);
                UserHandler.ActiveDuels.Add(duel);

                var duration = _configuration.GetValue<int>("DuelDurationInMinutes");
                var timer = new Timer
                {
                    Interval = duration * 60 * 1000
                };
                timer.Elapsed += (sender, e) => DuelTimeElapsed(duel, timer);
                timer.Start();

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

        private void DuelTimeElapsed(DuelDto duel, Timer timer)
        {
            timer.Dispose();
            UserHandler.ActiveDuels.Remove(duel);
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
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, duel.GroupName);
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
            var player = duel.Players.First(p => p.Email.Equals(Context.User.FindFirstValue(ClaimTypes.Email)));

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
                c.Players.Select(p => p.Email).Contains(Context.User.FindFirstValue(ClaimTypes.Email)));
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
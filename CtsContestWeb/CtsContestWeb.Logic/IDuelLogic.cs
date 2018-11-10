using CtsContestWeb.Dto;

namespace CtsContestWeb.Logic
{
    public interface IDuelLogic
    {
        DuelDto CreateDuel(TaskDto task, PlayerDto firstPlayer, PlayerDto secondPlayer, int duration);
        int CalculateDuelDuration(string environment, int taskValue);
    }
}

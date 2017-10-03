﻿using System.Threading.Tasks;

namespace CtsContestWeb.Logic
{
    public interface IBalanceLogic
    {
        Task<bool> IsBalanceEnough(string userEmail, int prizeId);
        Task<int> GetCurrentBalance(string userEmail);
    }
}
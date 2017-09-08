using System;

namespace CtsContestWeb.Logic
{
    public class BalanceLogic
    {
        private int _amount;

        public BalanceLogic(/* ... */)
        {
            _amount = 0;
        }

        public int Amount
        {
            get { return _amount; }
        }

        public bool IsBalanceEnough()
        {
            throw new NotImplementedException();
        }
    }
}

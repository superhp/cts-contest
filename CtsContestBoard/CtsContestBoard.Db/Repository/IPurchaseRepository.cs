using System;
using System.Collections.Generic;
using CtsContestBoard.Db.Entities;
using CtsContestBoard.Dto;

namespace CtsContestBoard.Db.Repository
{
    public interface IPurchaseRepository
    {
        IEnumerable<Purchase> GetAll();
    }
}
using System;

namespace CtsContestBoard.Db.Entities
{
    public interface IAuditable
    {
        DateTime Created { get; set; }
    }
}
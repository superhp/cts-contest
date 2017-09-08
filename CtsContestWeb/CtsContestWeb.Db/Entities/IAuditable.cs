using System;

namespace CtsContestWeb.Db.Entities
{
    public interface IAuditable
    {
        DateTime Created { get; set; }
    }
}
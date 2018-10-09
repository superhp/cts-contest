using CtsContestWeb.Db.Entities;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace CtsContestWeb.Db.Repository
{
    public interface IContactInfoRepository
    {
        void InsertIfNotExists(ContactInfo contactInfo);
    }
}

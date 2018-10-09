using CtsContestWeb.Db.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace CtsContestWeb.Db.Repository
{
    public class ContactInfoRepository : IContactInfoRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ContactInfoRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void InsertIfNotExists(ContactInfo contactInfo)
        {
            var email = contactInfo.Email;
            var exists = _dbContext.ContactInformation.Any(u => u.Email.Equals(email));

            if (!exists)
            {
                _dbContext.ContactInformation.Add(contactInfo);
                _dbContext.SaveChanges();
            }
        }
    }
}

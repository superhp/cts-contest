using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CtsContestWeb.Db.Entities
{
    public class User
    {
        [Key]
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Picture { get; set; }
        public ICollection<Solution> Solutions { get; set; }
        public ICollection<Purchase> Purchases { get; set; }
    }
}

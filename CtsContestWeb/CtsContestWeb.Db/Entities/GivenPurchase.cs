using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CtsContestWeb.Db.Entities
{
    public class GivenPurchase : IAuditable
    {
        [ForeignKey("Purchase")]
        public Guid GivenPurchaseId { get; set; }
        public virtual Purchase Purchase { get; set; }
        public DateTime Created { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CtsContestWeb.Db.Entities
{
    public class Purchase : IAuditable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid PurchaseId { get; set; }
        public string UserEmail { get; set; }
        public int PrizeId { get; set; }
        public int Cost { get; set; }
        public virtual GivenPurchase GivenPurchase { get; set; }
        public DateTime Created { get; set; }
    }
}

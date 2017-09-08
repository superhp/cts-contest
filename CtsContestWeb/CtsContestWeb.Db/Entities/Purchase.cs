using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CtsContestWeb.Db.Entities
{
    public class Purchase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid PurchaseId { get; set; }
        public int UserId { get; set; }
        public int PrizeId { get; set; }
        public int Cost { get; set; }
        public bool Given { get; set; }
    }
}

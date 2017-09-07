namespace CtsContestWeb.Db.Entities
{
    public class Purchase
    {
        public int PurchaseId { get; set; }
        public int UserId { get; set; }
        public int PrizeId { get; set; }
        public int Cost { get; set; }
    }
}

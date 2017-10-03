using System;

namespace CtsContestBoard.Dto
{
    public class PurchaseDto
    {
        public Guid PurchaseId { get; set; }
        public string UserEmail { get; set; }
        public int PrizeId { get; set; }
        public bool IsGivenAway { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public int Price { get; set; }
    }
}

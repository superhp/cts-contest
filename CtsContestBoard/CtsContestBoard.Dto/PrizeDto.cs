using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CtsContestBoard.Dto
{
    public class PrizeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public string Picture { get; set; }
        public string Category { get; set; }
    }
}

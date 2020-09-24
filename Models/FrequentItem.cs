using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NorthwindAPI.Models
{
    public class FrequentItem
    {
        public long ProductID { get; set; }
        public int Quantity { get; set; }

        public FrequentItem(long productID, int quantity)
        {
            ProductID = productID;
            Quantity = quantity;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NorthwindAPI.Models
{
    public class ReceivedOrder
    {
        public long RepeatOrder { get; set; }
        public Customer ShipAddress { get; set; }
        public List<OrderDetail> Products { get;set;}

    }
}

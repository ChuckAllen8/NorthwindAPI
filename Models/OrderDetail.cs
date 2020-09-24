using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;

namespace NorthwindAPI.Models
{
    [Table("Order Details")]
    public class OrderDetail
    {
        [ExplicitKey]
        public long OrderID { get; set; }
        [ExplicitKey]
        public long ProductID { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Discount { get; set; }

    }
}

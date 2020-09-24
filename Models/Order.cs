using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;

namespace NorthwindAPI.Models
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        public long OrderID { get; set; }
        public string CustomerID { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime RequiredDate { get; set; }
        [Write(false)]
        public DateTime ShippedDate { get; set; }
        [Write(false)]
        public List<OrderDetail> Details { get; set; }
    }
}

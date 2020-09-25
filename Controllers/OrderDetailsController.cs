using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NorthwindAPI.Models;
using NorthwindAPI.Services;

namespace NorthwindAPI.Controllers
{

    [Route("OrderDetails")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {
        IDataAccessor db;
        public OrderDetailsController(IDataAccessor _db)
        {
            db = _db;
        }

        [HttpGet("{CustomerID}/Frequent")]
        public List<FrequentItem> Frequent(string CustomerID)
        {
            var AllOrders = db.GetCustomerOrders(CustomerID);
            Dictionary<long, int> tally = new Dictionary<long, int>();
            foreach(Order o in AllOrders)
            {
                foreach (OrderDetail od in o.Details)
                {
                    if (tally.ContainsKey(od.ProductID))
                    {
                        tally[od.ProductID] += od.Quantity;
                    }
                    else
                    {
                        tally.Add(od.ProductID, od.Quantity);
                    }
                }
            }
            List<int> quants = tally.Values.ToList();
            quants.Sort();
            quants.Reverse();
            Dictionary<long, int> results = new Dictionary<long, int>();

            /*
            int countingTarget;
            if(quants.Count < 3)
            {    countingTarget = quants.Count;  }
            else
            {   countingTarget = 3;    }
            */

            for (int i = 0; i < (quants.Count>=3? 3: quants.Count); i++)
            {
                foreach (KeyValuePair<long, int> item in tally)
                {
                    if (item.Value == quants[i])
                    {
                        results[item.Key] = item.Value;
                    }
                }
            }

            List<FrequentItem> FI = new List<FrequentItem>();

            foreach (KeyValuePair<long, int> item in results)
            {
                FI.Add(new FrequentItem(item.Key, item.Value));
            }

            return FI;
        }

        [HttpGet("{CustomerID}/{OrderID}")]
        public JsonResult GetOrderDetails(string CustomerID, long OrderID)
        {
            IEnumerable<Order> orders = db.GetCustomerOrders(CustomerID);
            if (orders.Where(item => item.OrderID == OrderID).ToList().Count == 0)
            {
                return new JsonResult(new { Status = "No Order found matching Order ID" });
            }

            Order toPresent = (Order)(orders.Where(item => item.OrderID == OrderID).First());
            return new JsonResult(toPresent);
        }

        //return db.GetCustomerOrders(CustomerID);


    }
}



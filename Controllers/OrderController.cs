using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NorthwindAPI.Models;
using NorthwindAPI.Services;

namespace NorthwindAPI.Controllers
{
    [Route("Orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        IDataAccessor db;
        public OrderController(IDataAccessor _db)
        {
            db = _db;
        }

        [HttpGet]
        [Route("{CustomerID}")]
        public IEnumerable<Order> GetMyOrders(string CustomerID)
        {
            return db.GetCustomerOrders(CustomerID);
        }

        [HttpGet]
        [Route("{CustomerID}/NotShipped")]
        public IEnumerable<Order> GetNotShippedOrders(string CustomerID)
        {
            return from order in db.GetCustomerOrders(CustomerID) where order.ShippedDate == DateTime.Parse("0001-01-01T00:00:00") select order;
        }



        [HttpPost]
        [Route("{CustomerID}")]
        public async Task<JsonResult> Order(string CustomerID, [FromBody] ReceivedOrder order)   // [FromBody] string order
        {
            //string order = await ReadString();
            //return new JsonResult(order);
            //try
            //{
                //var stuff = JsonSerializer.Deserialize(order, typeof(ReceivedOrder));
                return new JsonResult(new { Staus = "Order Details Received" });
            //}
            //catch
            //{
                //return new JsonResult(new { Status = "No Order Details Received" });
            //}
        }

        public async Task<string> ReadString()
        {
            using(StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}


//String data = new System.IO.StreamReader(context.Request.InputStream).ReadToEnd();
//ReceivedOrder user = JsonConvert.DeserializeObject<ReceivedOrder>(data);

/*
     * Orders
     * done GET all my previous orders by my ID (My order history) return OrderID, Order Date, Ship Date, Freight (Total Shipping Cost)
     * done GET NotShipped Orders ShippedDate is Null return All previous for Unshipped Orders, return message for no results
     * DELETE Unshipped Order by OrderID
     * in progress POST Create New Order - Accept Shipped To, Product ID, Qty if no Address default to customer record-> Post Order Detail
     * POST Create Repeat Order - Duplicate all items with new OrderID
     * 
     * 
     * CustomerID not vlid returns empty list
     * 
     * */

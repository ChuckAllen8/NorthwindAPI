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


    }
}

/*
     * Orders
     * GET all my previous orders by my ID (My order history) return OrderID, Order Date, Ship Date, Freight (Total Shipping Cost)
     * GET NotShipped Orders ShippedDate is Null return All previous for Unshipped Orders, return message for no results
     * DELETE Unshipped Order by OrderID
     * POST Create New Order - Accept Shipped To, Product ID, Qty -> Post Order Detail
     * POST Create Repeat Order - Duplicate all items with new OrderID
     * 
     * 
     * CustomerID not vlid returns empty list
     * 
     * */

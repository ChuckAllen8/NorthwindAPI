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

        [HttpDelete]
        [Route("{CustomerID}/Delete/{OrderID}")]
        public JsonResult Delete(string CustomerID, long OrderID)
        {
            IEnumerable<Order> customerOrders = db.GetCustomerOrders(CustomerID);
            if (customerOrders.Where(item => item.OrderID == OrderID).ToList().Count == 0)
            {
                return new JsonResult(new { Status = "No Order found matching Order ID" });
            }

            Order toDelete = (Order)(customerOrders.Where(item => item.OrderID == OrderID).First());

            if(toDelete.ShippedDate > DateTime.Parse("0001-01-01T00:00:00"))
            {
                return new JsonResult(new { Status = "Cannot Delete, Order has already shipped" });
            }
            else
            {
                db.DeleteOrder(OrderID);
                return new JsonResult(new { Status = $"Order {OrderID} is Deleted" });
            }
        }

        [HttpPost]
        [Route("{CustomerID}")]
        [Consumes("application/json")]
        public JsonResult Order(string CustomerID, [FromBody] ReceivedOrder order)   // [FromBody] string order
        {
            IEnumerable<Order> myOrders = db.GetCustomerOrders(CustomerID);
            IEnumerable<Product> products = db.GetAllProducts();
            Order toPost = null;

            if(myOrders.Any(item => item.OrderID == order.RepeatOrder))
            {
                //repeat order
                toPost = (Order)(myOrders.Where(item => item.OrderID == order.RepeatOrder).First());
                toPost.OrderID = 0;
                toPost.ShippedDate = DateTime.Parse("0001-01-01T00:00:00");

                if(toPost.Details.Any(detail => products.Where(prod => prod.ProductID == detail.ProductID).First().Discontinued))
                {
                    return new JsonResult(new { Status = "Discontinued Products in order, unable to repeat" });
                }

                db.SaveOrder(toPost);

                foreach(OrderDetail orderDetail in toPost.Details)
                {
                    orderDetail.OrderID = toPost.OrderID;
                    db.SaveDetail(orderDetail);
                }
            }
            else
            {
                //new order
                toPost = new Order();
                toPost.CustomerID = CustomerID;
                toPost.ShippedDate = DateTime.Parse("0001-01-01T00:00:00");
                toPost.OrderID = 0;
                toPost.OrderDate = DateTime.Now;
                toPost.RequiredDate = DateTime.Now.AddDays(28);
                db.SaveOrder(toPost);
                foreach(OrderDetail orderDetail in order.Products)
                {
                    orderDetail.OrderID = toPost.OrderID;
                    orderDetail.UnitPrice = products.Where(prod => prod.ProductID == orderDetail.ProductID).First().UnitPrice;
                    if (orderDetail.Quantity > 5)
                    {
                        orderDetail.Discount = 0.05m;
                    }
                    else
                    {
                        orderDetail.Discount = 0;
                    }
                    db.SaveDetail(orderDetail);
                }
            }
            return new JsonResult(db.GetCustomerOrders(CustomerID).Where(item => item.OrderID == toPost.OrderID).First());
        }
    }
}
/*
     * Orders
     * done GET all my previous orders by my ID (My order history) return OrderID, Order Date, Ship Date, Freight (Total Shipping Cost)
     * done GET NotShipped Orders ShippedDate is Null return All previous for Unshipped Orders, return message for no results
     * DELETE Unshipped Order by OrderID
     * POST Create New Order - Accept Shipped To, Product ID, Qty if no Address default to customer record-> Post Order Detail
     * POST Create Repeat Order - Duplicate all items with new OrderID
     * 
     * */

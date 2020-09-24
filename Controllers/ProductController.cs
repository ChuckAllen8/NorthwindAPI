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
    [Route("Products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        IDataAccessor db;
        public ProductController(IDataAccessor _db)
        {
            db = _db;
        }

        [HttpGet]
        [Route("")]
        public IEnumerable<Product> GetCurrentProducts(string Price="")
        {
            if (Price != "" && decimal.TryParse(Price, out decimal dollaz))
            {
                return from item in db.GetAllProducts() where item.UnitsInStock > 0 && !item.Discontinued && item.UnitPrice <= dollaz select item;
            }
            else
            {
                return from item in db.GetAllProducts() where item.UnitsInStock > 0 && !item.Discontinued select item;   
            }
        }

        [HttpGet]
        [Route("Clearance")]
        public IEnumerable<Product> GetClearanceProducts()
        {
            return from item in db.GetAllProducts() where item.UnitsInStock > 0 && item.Discontinued select item;
        }
    }

    /*
     * Orders
     * GET all my previous orders by my ID (My order history) return OrderID, Order Date, Ship Date, Freight (Total Shipping Cost)
     * GET Unshipped Orders ShippedDate is Null return All previous for Unshipped Orders, return message for no results
     * DELETE Unshipped Order by OrderID
     * POST Create New Order - Accept Shipped To, Product ID, Qty -> Post Order Detail
     * POST Create Repeat Order - Duplicate all items with new OrderID
     * 
     * Order Details
     * GET Frequent item orders return OrderID, Product ID, and Quantity (Optional limit number of items (Top 3 or 5))
     * GET Line item price for Product ID for each item in an order return OrderID, JOIN for ProductName, Quantity, Total Price (Unit Price * Qty[discount])
     * POST Add item to Unshipped Order - Accept Product ID, Qty [Modify existing Qty as PUT]
     * 
     * CustomerID not vlid returns empty list
     * 
     * */
}

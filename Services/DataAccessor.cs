using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper.Contrib.Extensions;
using System.Linq;
using System.Threading.Tasks;
using NorthwindAPI.Models;
using Microsoft.AspNetCore.Http.Features;
using Dapper;

namespace NorthwindAPI.Services
{
    public class DataAccessor : IDataAccessor
    {
        IDbConnection db;

        public DataAccessor(IConfiguration config)
        {
            db = new SqlConnection(config.GetConnectionString("DbServer"));
        }

        public bool DeleteOrder(long OrderID)
        {
            IEnumerable<OrderDetail> orderDetails = from detail in db.Query<OrderDetail>("SELECT * FROM [Order Details]") where detail.OrderID == OrderID select detail;
            foreach(OrderDetail detail in orderDetails)
            {
                db.Execute("DELETE FROM [Order Details] WHERE OrderID = @OrderID AND ProductID = @ProductID", detail);
            }
            db.Delete<Order>(new Order() { OrderID = OrderID });
            return true;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return db.GetAll<Product>();
        }

        public IEnumerable<Category> GetCategories()
        {
            return db.GetAll<Category>();
        }

        public Customer GetCustomer(string CustomerID)
        {
            return db.Get<Customer>(CustomerID);
        }

        public IEnumerable<Order> GetCustomerOrders(string CustomerID)
        {
            IEnumerable<Order> AllOrders = db.GetAll<Order>();
            IEnumerable<OrderDetail> AllDetails = db.Query<OrderDetail>("SELECT * FROM [Order Details]");
            IEnumerable<Order> customerOrders = from order in AllOrders where order.CustomerID == CustomerID select order;

            foreach (Order order in customerOrders)
            {
                order.Details = (from detail in AllDetails where detail.OrderID == order.OrderID select detail).ToList();
            }
            return customerOrders;
        }

        public bool SaveDetail(OrderDetail detail)
        {
            db.Execute("INSERT INTO [Order Details] (OrderID, ProductID, UnitPrice, Quantity, Discount) VALUES (@OrderID, @ProductID, @UnitPrice, @Quantity, @Discount)", detail);
            return true;
        }

        public bool SaveOrder(Order o)
        {
            if(o.OrderID == 0)
            {
                o.OrderID = db.Insert(o);
                return true;
            }
            return false;
        }

        public bool SaveProduct(Product product)
        {
            if (product.ProductID == 0)
            {
                product.ProductID = db.Insert(product);
                return true;
            }
            return false;
        }
    }


}

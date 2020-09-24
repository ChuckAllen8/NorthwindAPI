using NorthwindAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NorthwindAPI.Services
{
    public interface IDataAccessor
    {
        public IEnumerable<Product> GetAllProducts();

        public IEnumerable<Order> GetCustomerOrders(string CustomerID);

        public Customer GetCustomer(string CustomerID);

        public bool SaveOrder(Order order);
        public bool SaveDetail(OrderDetail detail);

        public bool DeleteOrder(long OrderID);
    }

   
}

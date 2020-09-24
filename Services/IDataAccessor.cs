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

    }

   
}

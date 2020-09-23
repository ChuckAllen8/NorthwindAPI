using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper.Contrib.Extensions;
using System.Linq;
using System.Threading.Tasks;
using NorthwindAPI.Models;

namespace NorthwindAPI.Services
{
    public class DataAccessor : IDataAccessor
    {
        IDbConnection db;

        public DataAccessor(IConfiguration config)
        {
            db = new SqlConnection(config.GetConnectionString("DbServer"));
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return db.GetAll<Product>();
        }
    }
}

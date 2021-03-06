﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;

namespace NorthwindAPI.Models
{
    [Table("Customers")]
    public class Customer
    {
        [ExplicitKey]
        public string CustomerID { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }

    }
}

﻿using ECommerceAPI.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Domain.Entities
{
    public class Order : BaseEntity
    {
        
        public string Description { get; set; }
        public string Address { get; set; }
        //public ICollection<Product> Products { get; set; }
        //public Guid CustomerId { get; set; }
        //public Customer Customer { get; set; }
        public Basket Basket { get; set; }
        public string OrderCode { get; set; }
        public CompletedOrder CompletedOrder { get; set; }
    }
}

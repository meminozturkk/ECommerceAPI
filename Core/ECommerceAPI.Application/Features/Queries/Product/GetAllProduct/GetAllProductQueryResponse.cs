﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Queries.Product.GetAllProduct
{
    public class GetAllProductQueryResponse
    {

        public int TotalCount { get; set; }
        public int TotalProductCount { get; set; }
        public object Products { get; set; }
        
    }
}


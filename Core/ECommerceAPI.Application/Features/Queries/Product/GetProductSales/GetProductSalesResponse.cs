using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Queries.Product.GetProductSales
{
    public class GetProductSalesResponse
    {
        public Dictionary<string, int> ProductSales { get; set; } // Ürün adı ve satış miktarını tutan sözlük

    }
}

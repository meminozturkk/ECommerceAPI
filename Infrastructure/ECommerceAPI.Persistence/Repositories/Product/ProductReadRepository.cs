using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Persistence.Repositories
{
    public class ProductReadRepository : ReadRepository<Product>, IProductReadRepository
    {
        private readonly ECommerceAPIDbContext _context;
        public ProductReadRepository(ECommerceAPIDbContext context) : base(context)
        {
            _context = context;
        }
        public new IQueryable<Product> Table => _context.Products.Include(p => p.ProductImageFile);

    }
}

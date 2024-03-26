using ECommerceAPI.Application.Repositories.Menu;
using ECommerceAPI.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Persistence.Repositories.Menu
{
    public class MenuWriteRepository : WriteRepository<ECommerceAPI.Domain.Entities.Menu>, IMenuWriteRepository
    {
        public MenuWriteRepository(ECommerceAPIDbContext context) : base(context)
        {
        }
    }
}

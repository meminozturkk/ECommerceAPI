using ECommerceAPI.Application.Repositories.EndPoint;
using ECommerceAPI.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Persistence.Repositories.Endpoint
{
    public class EndpointWriteRepository : WriteRepository<ECommerceAPI.Domain.Entities.Endpoint>, IEndpointWriteRepository
    {
        public EndpointWriteRepository(ECommerceAPIDbContext context) : base(context)
        {
        }
    }
}

﻿using ECommerceAPI.Application.Repositories;
using P = ECommerceAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Repositories
{
    public interface IProductImageFileWriteRepository : IWriteRepository<P.ProductImageFile>
    {
    }
}

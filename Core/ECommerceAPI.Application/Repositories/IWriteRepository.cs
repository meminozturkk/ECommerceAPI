﻿using ECommerceAPI.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Repositories
{
    public interface IWriteRepository<T> : IRepository<T> where T : BaseEntity
    {
        Task<bool> AddAsync(T entity);
        Task<bool> AddRangeAsync(List<T> entities);
        bool Update(T entity);
        Task<bool> UpdateRangeAsync(List<T> entities);
        bool Delete(T entity);
        Task<bool> DeleteAsync(string id);
        bool DeleteRange(List<T> entities);
        Task<int> SaveAsync();
    }
}

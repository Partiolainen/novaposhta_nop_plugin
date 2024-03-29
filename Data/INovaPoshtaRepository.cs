﻿using System;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Data;
using Nop.Plugin.Shipping.NovaPoshta.Domain;

namespace Nop.Plugin.Shipping.NovaPoshta.Data
{
    public interface INovaPoshtaRepository<T> : IRepository<T> where T : BaseEntity, INovaPoshtaEntity
    {
        Task<T> GetByRefAsync(string @ref, Func<IStaticCacheManager, CacheKey> getCacheKey = null, bool includeDeleted = true);
        
    }
}
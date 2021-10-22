using System;
using System.Linq;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Events;
using Nop.Data;
using Nop.Plugin.Shipping.NovaPoshta.Domain;

namespace Nop.Plugin.Shipping.NovaPoshta.Data
{
    public class NovaPoshtaRepository<T> : EntityRepository<T>, INovaPoshtaRepository<T> where T : BaseEntity, INovaPoshtaEntity
    {
        private readonly IStaticCacheManager _staticCacheManager;

        public NovaPoshtaRepository(
            IEventPublisher eventPublisher, 
            INopDataProvider dataProvider, 
            IStaticCacheManager staticCacheManager) : base(eventPublisher, dataProvider, staticCacheManager)
        {
            _staticCacheManager = staticCacheManager;
        }

        public async Task<T> GetByRefAsync(string @ref, Func<IStaticCacheManager, CacheKey> getCacheKey = null, bool includeDeleted = true)
        {
            async Task<T> GetEntityAsync()
            {
                return await AddDeletedFilter(Table, includeDeleted).FirstOrDefaultAsync(entity => entity.Ref == Convert.ToString(@ref));
            }
            
            if (getCacheKey == null)
                return await GetEntityAsync();
            
            //caching
            var cacheKey = getCacheKey(_staticCacheManager)
                           ?? _staticCacheManager.PrepareKeyForDefaultCache(NopEntityCacheDefaults<T>.ByIdCacheKey, @ref);

            return await _staticCacheManager.GetAsync(cacheKey, GetEntityAsync);
        }
    }
}
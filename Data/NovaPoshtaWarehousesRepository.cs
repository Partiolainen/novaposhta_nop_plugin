using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nop.Core.Caching;
using Nop.Core.Events;
using Nop.Data;
using Nop.Plugin.Shipping.NovaPoshta.Domain;

namespace Nop.Plugin.Shipping.NovaPoshta.Data
{
    public class NovaPoshtaWarehousesRepository : NovaPoshtaRepository<NovaPoshtaWarehouse>
    {
        private readonly IRepository<Dimensions> _dimensionsRepository;

        public NovaPoshtaWarehousesRepository(
            IEventPublisher eventPublisher,
            INopDataProvider dataProvider,
            IStaticCacheManager staticCacheManager,
            IRepository<Dimensions> dimensionsRepository)
            : base(eventPublisher, dataProvider, staticCacheManager)
        {
            _dimensionsRepository = dimensionsRepository;
        }

        public override async Task<IList<NovaPoshtaWarehouse>> GetAllAsync(
            Func<IQueryable<NovaPoshtaWarehouse>, IQueryable<NovaPoshtaWarehouse>> func = null,
            Func<IStaticCacheManager, CacheKey> getCacheKey = null, bool includeDeleted = true)
        {
            var novaPoshtaWarehouses = await base.GetAllAsync(func, getCacheKey, includeDeleted);

            foreach (var novaPoshtaWarehouse in novaPoshtaWarehouses)
            {
                novaPoshtaWarehouse.ReceivingLimitationsOnDimensions =
                    await _dimensionsRepository.GetByIdAsync(novaPoshtaWarehouse.ReceivingLimitationsOnDimensionsId);
                novaPoshtaWarehouse.SendingLimitationsOnDimensions =
                    await _dimensionsRepository.GetByIdAsync(novaPoshtaWarehouse.SendingLimitationsOnDimensionsId);
            }

            return novaPoshtaWarehouses;
        }

        public override async Task<IList<NovaPoshtaWarehouse>> GetAllAsync(
            Func<IQueryable<NovaPoshtaWarehouse>, Task<IQueryable<NovaPoshtaWarehouse>>> func = null,
            Func<IStaticCacheManager, CacheKey> getCacheKey = null, bool includeDeleted = true)
        {
            var novaPoshtaWarehouses = await base.GetAllAsync(func, getCacheKey, includeDeleted);

            foreach (var novaPoshtaWarehouse in novaPoshtaWarehouses)
            {
                novaPoshtaWarehouse.ReceivingLimitationsOnDimensions =
                    await _dimensionsRepository.GetByIdAsync(novaPoshtaWarehouse.ReceivingLimitationsOnDimensionsId);
                novaPoshtaWarehouse.SendingLimitationsOnDimensions =
                    await _dimensionsRepository.GetByIdAsync(novaPoshtaWarehouse.SendingLimitationsOnDimensionsId);
            }

            return novaPoshtaWarehouses;
        }

        public override async Task<IList<NovaPoshtaWarehouse>> GetAllAsync(
            Func<IQueryable<NovaPoshtaWarehouse>, Task<IQueryable<NovaPoshtaWarehouse>>> func = null,
            Func<IStaticCacheManager, Task<CacheKey>> getCacheKey = null, bool includeDeleted = true)
        {
            var novaPoshtaWarehouses = await base.GetAllAsync(func, getCacheKey, includeDeleted);

            foreach (var novaPoshtaWarehouse in novaPoshtaWarehouses)
            {
                novaPoshtaWarehouse.ReceivingLimitationsOnDimensions =
                    await _dimensionsRepository.GetByIdAsync(novaPoshtaWarehouse.ReceivingLimitationsOnDimensionsId);
                novaPoshtaWarehouse.SendingLimitationsOnDimensions =
                    await _dimensionsRepository.GetByIdAsync(novaPoshtaWarehouse.SendingLimitationsOnDimensionsId);
            }

            return novaPoshtaWarehouses;
        }

        public override async Task InsertAsync(NovaPoshtaWarehouse warehouseEntity, bool publishEvent = true)
        {
            await _dimensionsRepository.InsertAsync(warehouseEntity.SendingLimitationsOnDimensions);
            await _dimensionsRepository.InsertAsync(warehouseEntity.ReceivingLimitationsOnDimensions);

            warehouseEntity.SendingLimitationsOnDimensionsId = warehouseEntity.SendingLimitationsOnDimensions.Id;
            warehouseEntity.ReceivingLimitationsOnDimensionsId = warehouseEntity.ReceivingLimitationsOnDimensions.Id;

            await base.InsertAsync(warehouseEntity, publishEvent);
        }

        public override async Task InsertAsync(IList<NovaPoshtaWarehouse> warehouseEntities, bool publishEvent = true)
        {
            foreach (var novaPoshtaWarehouse in warehouseEntities)
            {
                await _dimensionsRepository.InsertAsync(novaPoshtaWarehouse.SendingLimitationsOnDimensions);
                await _dimensionsRepository.InsertAsync(novaPoshtaWarehouse.ReceivingLimitationsOnDimensions);

                novaPoshtaWarehouse.SendingLimitationsOnDimensionsId =
                    novaPoshtaWarehouse.SendingLimitationsOnDimensions.Id;
                novaPoshtaWarehouse.ReceivingLimitationsOnDimensionsId =
                    novaPoshtaWarehouse.ReceivingLimitationsOnDimensions.Id;
            }

            await base.InsertAsync(warehouseEntities, publishEvent);
        }
    }
}
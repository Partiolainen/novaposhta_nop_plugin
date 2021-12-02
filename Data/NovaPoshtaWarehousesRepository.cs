using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nop.Core.Caching;
using Nop.Core.Events;
using Nop.Data;
using Nop.Plugin.Shipping.NovaPoshta.Domain;
using Nop.Services.Configuration;
using NUglify.Helpers;

namespace Nop.Plugin.Shipping.NovaPoshta.Data
{
    public class NovaPoshtaWarehousesRepository : NovaPoshtaRepository<NovaPoshtaWarehouse>
    {
        private readonly IRepository<Dimensions> _dimensionsRepository;
        private readonly NovaPoshtaSettings _novaPoshtaSettings;
        private readonly ISettingService _settingService;

        public NovaPoshtaWarehousesRepository(
            IEventPublisher eventPublisher,
            INopDataProvider dataProvider,
            IStaticCacheManager staticCacheManager,
            IRepository<Dimensions> dimensionsRepository,
            NovaPoshtaSettings novaPoshtaSettings,
            ISettingService settingService)
            : base(eventPublisher, dataProvider, staticCacheManager)
        {
            _dimensionsRepository = dimensionsRepository;
            _novaPoshtaSettings = novaPoshtaSettings;
            _settingService = settingService;
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
            var maxDimension = new Dimensions();
            var maxWeight = 0;
            
            await _dimensionsRepository.InsertAsync(
                warehouseEntities
                    .Select(warehouse => warehouse.ReceivingLimitationsOnDimensions)
                    .ToList(), false);
            
            await _dimensionsRepository.InsertAsync(
                warehouseEntities
                    .Select(warehouse => warehouse.SendingLimitationsOnDimensions)
                    .ToList(), false);
            
            warehouseEntities.ForEach(warehouse =>
            {
                if (maxDimension <= warehouse.ReceivingLimitationsOnDimensions)
                    maxDimension = warehouse.ReceivingLimitationsOnDimensions;

                if (maxWeight < int.Parse(warehouse.PlaceMaxWeightAllowed))
                    maxWeight = int.Parse(warehouse.PlaceMaxWeightAllowed);
                
                if (maxWeight < int.Parse(warehouse.TotalMaxWeightAllowed))
                    maxWeight = int.Parse(warehouse.TotalMaxWeightAllowed);

                warehouse.ReceivingLimitationsOnDimensionsId = warehouse.ReceivingLimitationsOnDimensions.Id;
                warehouse.SendingLimitationsOnDimensionsId = warehouse.SendingLimitationsOnDimensions.Id;
            });

            _novaPoshtaSettings.MaxAllowedHeightCm = maxDimension.Height;
            _novaPoshtaSettings.MaxAllowedWidthCm = maxDimension.Width;
            _novaPoshtaSettings.MaxAllowedLengthCm = maxDimension.Length;
            _novaPoshtaSettings.MaxAllowedWeightKg = maxWeight;
            await _settingService.SaveSettingAsync(_novaPoshtaSettings);

            await base.InsertAsync(warehouseEntities, publishEvent);
        }
    }
}
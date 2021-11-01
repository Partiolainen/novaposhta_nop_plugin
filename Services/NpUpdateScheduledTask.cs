using System.Linq;
using Nop.Plugin.Shipping.NovaPoshta.Data;
using Nop.Plugin.Shipping.NovaPoshta.Domain;
using Nop.Services.Logging;
using Nop.Services.Tasks;
using Task = System.Threading.Tasks.Task;

namespace Nop.Plugin.Shipping.NovaPoshta.Services
{
    public class NpUpdateScheduledTask : IScheduleTask
    {
        private readonly INovaPoshtaRepository<NovaPoshtaSettlement> _settlementsRepository;
        private readonly INovaPoshtaRepository<NovaPoshtaWarehouse> _warehousesRepository;
        private readonly INovaPoshtaRepository<NovaPoshtaArea> _areasRepository;
        private readonly INpApiService _npApiService;
        private readonly ILogger _logger;

        public NpUpdateScheduledTask(
            INovaPoshtaRepository<NovaPoshtaSettlement> settlementsRepository,
            INovaPoshtaRepository<NovaPoshtaWarehouse> warehousesRepository,
            INovaPoshtaRepository<NovaPoshtaArea> areasRepository,
            INpApiService npApiService,
            ILogger logger
        )
        {
            _settlementsRepository = settlementsRepository;
            _warehousesRepository = warehousesRepository;
            _areasRepository = areasRepository;
            _npApiService = npApiService;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            await _logger.InformationAsync("Start Nova Poshta plugin data update scheduled task");

            await UpdateAreas();
            await UpdateSettlements();
            await UpdateWarehouses();
            
            await _logger.InformationAsync("End Nova Poshta plugin data update scheduled task");
        }
        
        private async Task UpdateAreas()
        {
            var areas = await _npApiService.GetAllAreas();
            
            await _logger.InformationAsync($"Received from API-server {areas.Count} area objects");
            
            if (areas.Any())
            {
                var deleted = await _areasRepository.DeleteAsync(_ => true);
                await _logger.InformationAsync($"Deleted from database {deleted} area entities");
                
                await _areasRepository.InsertAsync(areas, false);
            }
        }

        private async Task UpdateSettlements()
        {
            var allSettlements = await _npApiService.GetAllSettlements();
            
            await _logger.InformationAsync($"Received from API-server {allSettlements.Count} settlement objects");

            if (allSettlements.Any())
            {
                var deleted = await _settlementsRepository.DeleteAsync(_ => true);
                await _logger.InformationAsync($"Deleted from database {deleted} settlement entities");
                
                await _settlementsRepository.InsertAsync(allSettlements, false);
            }
        }

        private async Task UpdateWarehouses()
        {
            var allWarehouses = await _npApiService.GetAllWarehouses();
            
            await _logger.InformationAsync($"Received from API-server {allWarehouses.Count} warehouse objects");

            if (allWarehouses.Any())
            {
                var deleted = await _warehousesRepository.DeleteAsync(_ => true);
                await _logger.InformationAsync($"Deleted from database {deleted} warehouse entities");
                
                await _warehousesRepository.InsertAsync(allWarehouses, false);
            }
        }
    }
}
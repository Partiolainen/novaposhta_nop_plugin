using System.Linq;
using System.Threading.Tasks;
using Nop.Data;
using Nop.Plugin.Shipping.NovaPoshta.Domain;

namespace Nop.Plugin.Shipping.NovaPoshta.Services
{
    public class NovaPoshtaWarehouseForOrderService : INovaPoshtaWarehouseForOrderService
    {
        private readonly IRepository<NovaPoshtaWarehouseForOrder> _repository;

        public NovaPoshtaWarehouseForOrderService(IRepository<NovaPoshtaWarehouseForOrder> repository)
        {
            _repository = repository;
        }

        public async Task<NovaPoshtaWarehouseForOrder> AddRecord(NovaPoshtaWarehouseForOrder record)
        {
            await _repository.InsertAsync(record);

            return record;
        }

        public async Task<NovaPoshtaWarehouseForOrder> GetByOrderId(int orderId)
        {
            var novaPoshtaWarehouseForOrderRecords = await _repository.GetAllAsync(query =>
            {
                return query.Where(record => record.OrderId == orderId);
            });

            return novaPoshtaWarehouseForOrderRecords.FirstOrDefault();
        }
    }
}
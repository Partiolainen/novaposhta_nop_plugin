using System.Linq;
using System.Threading.Tasks;
using Nop.Data;
using Nop.Plugin.Shipping.NovaPoshta.Domain;

namespace Nop.Plugin.Shipping.NovaPoshta.Services
{
    public class NpOrderDataService : INpOrderDataService
    {
        private readonly IRepository<NpOrderShippingData> _repository;

        public NpOrderDataService(IRepository<NpOrderShippingData> repository)
        {
            _repository = repository;
        }

        public async Task<NpOrderShippingData> AddRecord(NpOrderShippingData record)
        {
            await _repository.InsertAsync(record);

            return record;
        }

        public async Task<NpOrderShippingData> GetByOrderId(int orderId)
        {
            var novaPoshtaWarehouseForOrderRecords = await _repository.GetAllAsync(query =>
            {
                return query.Where(record => record.OrderId == orderId);
            });

            return novaPoshtaWarehouseForOrderRecords.FirstOrDefault();
        }
    }
}
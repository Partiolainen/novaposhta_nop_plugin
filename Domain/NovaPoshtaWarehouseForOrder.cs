using Nop.Core;

namespace Nop.Plugin.Shipping.NovaPoshta.Domain
{
    public class NovaPoshtaWarehouseForOrder : BaseEntity
    {
        public int OrderId { get; set; }
        public string NovaPoshtaWarehouseRef { get; set; }
    }
}
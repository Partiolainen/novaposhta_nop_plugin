using Nop.Core;

namespace Nop.Plugin.Shipping.NovaPoshta.Domain
{
    public class NovaPoshtaAddress : BaseEntity, INovaPoshtaEntity
    {
        public string Present { get; set; }
        public int Warehouses { get; set; }
        public string MainDescription { get; set; }
        public string Area { get; set; }
        public string Region { get; set; }
        public string SettlementTypeCode { get; set; }
        public string DeliveryCity { get; set; }
        public bool AddressDeliveryAllowed { get; set; }
        public bool StreetsAvailability { get; set; }
        public string ParentRegionTypes { get; set; }
        public string ParentRegionCode { get; set; }
        public string RegionTypes { get; set; }
        public string RegionTypesCode { get; set; }
        public string Ref { get; set; }
    }
}
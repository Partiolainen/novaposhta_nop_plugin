using Nop.Core;

namespace Nop.Plugin.Shipping.NovaPoshta.Domain
{
    public class NovaPoshtaWarehouse : BaseEntity, INovaPoshtaEntity
    {
        public string Ref { get; set; }
        public string SiteKey { get; set; }
        public string Description { get; set; }
        public string DescriptionRu { get; set; }
        public string ShortAddress { get; set; }
        public string ShortAddressRu { get; set; }
        public string Phone { get; set; }
        public string TypeOfWarehouse { get; set; }
        public string Number { get; set; }
        public string CityRef { get; set; }
        public string CityDescription { get; set; }
        public string CityDescriptionRu { get; set; }
        public string SettlementRef { get; set; }
        public string SettlementDescription { get; set; }
        public string SettlementAreaDescription { get; set; }
        public string SettlementRegionsDescription { get; set; }
        public string SettlementTypeDescription { get; set; }
        public string SettlementTypeDescriptionRu { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string MaxDeclaredCost { get; set; }
        public string TotalMaxWeightAllowed { get; set; }
        public string PlaceMaxWeightAllowed { get; set; }
        public string DistrictCode { get; set; }
        public string WarehouseStatus { get; set; }
        public string WarehouseStatusDate { get; set; }
        public string CategoryOfWarehouse { get; set; }
        public string Direct { get; set; }
        public string RegionCity { get; set; }
        public string WarehouseForAgent { get; set; }
        public string WorkInMobileAwis { get; set; }
        
        public Dimensions SendingLimitationsOnDimensions { get; set; }
        public int SendingLimitationsOnDimensionsId { get; set; }
        
        public Dimensions ReceivingLimitationsOnDimensions { get; set; }
        public int ReceivingLimitationsOnDimensionsId { get; set; }
        
        public WeekWorkTimes Reception { get; set; }
        public int ReceptionWeekWorkTimesId { get; set; }
        
        public WeekWorkTimes Delivery { get; set; }
        public int DeliveryWeekWorkTimesId { get; set; }
        
        public WeekWorkTimes Schedule { get; set; }
        public int ScheduleWeekWorkTimesId { get; set; }
    }
}
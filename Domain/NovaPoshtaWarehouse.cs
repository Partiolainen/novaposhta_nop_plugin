using Nop.Core;

namespace Nop.Plugin.Shipping.NovaPoshta.Domain
{
    public class NovaPoshtaWarehouse : BaseEntity
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
    }
}
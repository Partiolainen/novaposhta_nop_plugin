using Nop.Core;

namespace Nop.Plugin.Shipping.NovaPoshta.Domain
{
    public class NovaPoshtaArea : BaseEntity, INovaPoshtaEntity
    {
        public string Ref { get; set; }
        public string AreasCenter { get; set; }
        public string DescriptionRu { get; set; }
        public string Description { get; set; }
    }
}
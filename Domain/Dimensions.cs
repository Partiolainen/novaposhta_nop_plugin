using Nop.Core;

namespace Nop.Plugin.Shipping.NovaPoshta.Domain
{
    public class Dimensions : BaseEntity
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int Length { get; set; }
    }
}
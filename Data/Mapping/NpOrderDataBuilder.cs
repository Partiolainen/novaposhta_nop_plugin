using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Orders;
using Nop.Data.Extensions;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Shipping.NovaPoshta.Domain;

namespace Nop.Plugin.Shipping.NovaPoshta.Data.Mapping
{
    public class NpOrderDataBuilder : NopEntityBuilder<NpOrderShippingData>
    {
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(NpOrderShippingData.OrderId))
                .AsInt32()
                .ForeignKey<Order>();
            
            table
                .WithColumn(nameof(NpOrderShippingData.NovaPoshtaCustomerAddressId))
                .AsInt32()
                .Nullable()
                .ForeignKey<NpCustomerAddressForOrder>();
        }
    }
}
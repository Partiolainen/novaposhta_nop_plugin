using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Orders;
using Nop.Data.Extensions;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Shipping.NovaPoshta.Domain;

namespace Nop.Plugin.Shipping.NovaPoshta.Data.Mapping
{
    public class NpCustomerAddressForOrderBuilder : NopEntityBuilder<NpCustomerAddressForOrder>
    {
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(NpCustomerAddressForOrder.OrderId))
                .AsInt32()
                .ForeignKey<Order>();
        }
    }
}
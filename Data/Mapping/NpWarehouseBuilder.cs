using FluentMigrator.Builders.Create.Table;
using Nop.Data.Extensions;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Shipping.NovaPoshta.Domain;

namespace Nop.Plugin.Shipping.NovaPoshta.Data.Mapping
{
    public class NpWarehouseBuilder : NopEntityBuilder<NovaPoshtaWarehouse>
    {
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            // table
            //     .WithColumn(nameof(NovaPoshtaWarehouse.SendingLimitationsOnDimensionsId))
            //     .AsInt32()
            //     .ForeignKey<Dimensions>();
            //
            // table
            //     .WithColumn(nameof(NovaPoshtaWarehouse.ReceivingLimitationsOnDimensionsId))
            //     .AsInt32()
            //     .ForeignKey<Dimensions>();
        }
    }
}
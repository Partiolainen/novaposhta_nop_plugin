using FluentMigrator;
using Nop.Data.Migrations;
using Nop.Plugin.Shipping.NovaPoshta.Domain;

namespace Nop.Plugin.Shipping.NovaPoshta.Data.Migrations
{
    [NopMigration("2021/10/20 12:17:00:0000000", "Create NovaPoshtaAddress, NovaPoshtaSettlement, NovaPoshtaWarehouse")]
    public class InitialMigration : AutoReversingMigration
    {
        private readonly IMigrationManager _migrationManager;

        public InitialMigration(IMigrationManager migrationManager)
        {
            _migrationManager = migrationManager;
        }

        public override void Up()
        {
            _migrationManager.BuildTable<NovaPoshtaSettlement>(Create);
            _migrationManager.BuildTable<NovaPoshtaWarehouse>(Create);
        }
    }
}
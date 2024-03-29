﻿using FluentMigrator;
using Nop.Core.Domain.Orders;
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
            _migrationManager.BuildTable<Dimensions>(Create);
            _migrationManager.BuildTable<WeekWorkTimes>(Create);
            _migrationManager.BuildTable<NovaPoshtaArea>(Create);
            _migrationManager.BuildTable<NovaPoshtaSettlement>(Create);
            _migrationManager.BuildTable<NovaPoshtaWarehouse>(Create);
            _migrationManager.BuildTable<NpCustomerAddressForOrder>(Create);
            _migrationManager.BuildTable<NpOrderShippingData>(Create);
        }
    }
}
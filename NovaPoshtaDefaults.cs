namespace Nop.Plugin.Shipping.NovaPoshta
{
    public static class NovaPoshtaDefaults
    {
        public static string UpdateDataTaskType => "Nop.Plugin.Shipping.NovaPoshta.Services.NpUpdateScheduledTask";
        public static int DefaultSynchronizationPeriod => 24;
        public static string SynchronizationTaskName => "Update data (Nova Poshta plugin)";
        public static string CustomerAddressForOrder => "CustomerAddressForOrder";
    }
}
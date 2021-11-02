using System;
using Nop.Services.Logging;
using Nop.Services.Tasks;
using Task = System.Threading.Tasks.Task;

namespace Nop.Plugin.Shipping.NovaPoshta.Services
{
    public class NpScheduleTasksService : INpScheduleTasksService
    {
        private readonly IScheduleTaskService _scheduleTaskService;
        private readonly ILogger _logger;

        public NpScheduleTasksService(
            IScheduleTaskService scheduleTaskService,
            ILogger logger)
        {
            _scheduleTaskService = scheduleTaskService;
            _logger = logger;
        }

        public async Task UpdateDatabase(bool force = false)
        {
            try
            {
                var scheduleTask = await _scheduleTaskService.GetTaskByTypeAsync(NovaPoshtaDefaults.UpdateDataTaskType)
                                   ?? throw new ArgumentException("Schedule task cannot be loaded",
                                       NovaPoshtaDefaults.UpdateDataTaskType);

                if (!force)
                {
                    var lastSuccessUtc = scheduleTask.LastSuccessUtc;
                    if (lastSuccessUtc != null)
                    {
                        var afterSuccess = DateTime.Now - lastSuccessUtc;
                        if (new TimeSpan(0, 0, scheduleTask.Seconds) > afterSuccess)
                        {
                            return;
                        }
                    }
                }

                var task = new Nop.Services.Tasks.Task(scheduleTask) {Enabled = true};
                await task.ExecuteAsync(true, false);
            }
            catch (Exception e)
            {
                await _logger.ErrorAsync($"Exception in : {NovaPoshtaDefaults.SynchronizationTaskName}", e);
            }
        }
    }
}
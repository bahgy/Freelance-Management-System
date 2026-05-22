using Freelance_Bot.Domain.IRepository;
using Freelance_Bot.Infrastruction.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TaskStatusEnum = Freelance_Bot.Domain.Enum.TaskStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance_Bot.Infrastruction.BackgroundServices
{
    public class OverdueTaskDetector(
    IServiceScopeFactory scopeFactory,
    ILogger<OverdueTaskDetector> logger) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await DetectOverdueTasksAsync();
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }

        private async Task DetectOverdueTasksAsync()
        {
            using var scope = scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<FreelancerDbContext>();
            var eventRepo = scope.ServiceProvider.GetRequiredService<IEventRepository>();

            try
            {
                var now = DateTime.UtcNow;
                var cutoff = now.AddHours(-1); // only detect tasks that became overdue in the last hour

                var newlyOverdue = await db.tasks
                    .Include(t => t.Project)
                    .Where(t => t.DueDate >= cutoff
                             && t.DueDate < now
                             && t.Status != TaskStatusEnum.Done
                             && t.Status != TaskStatusEnum.Cancelled)
                    .ToListAsync();

                foreach (var task in newlyOverdue)
                {
                    // Check we haven't already emitted this event
                    var alreadyEmitted = await db.events.AnyAsync(e =>
                        e.EventName == "TaskOverdue"
                        && e.EntityId == task.Id
                        && e.CreatedAt >= cutoff);

                    if (!alreadyEmitted)
                    {
                        await eventRepo.EmitAsync(
                            task.Project.UserId, "Task", task.Id, "TaskOverdue",
                            new { task.Id, task.Title, task.ProjectId, task.DueDate }
                        );
                        logger.LogInformation("TaskOverdue event emitted for Task {TaskId}", task.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in OverdueTaskDetector");
            }
        }
    }
}

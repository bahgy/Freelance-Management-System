using Freelance_Bot.Domain.Enum;
using Freelance_Bot.Domain.IRepository;
using Freelance_Bot.Infrastruction.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance_Bot.Infrastruction.BackgroundServices
{
    public class DeadlineApproachingDetector(
    IServiceScopeFactory scopeFactory,
    ILogger<DeadlineApproachingDetector> logger) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Wait until next 7am UTC
                var now = DateTime.UtcNow;
                var nextRun = now.Date.AddHours(7);
                if (now >= nextRun) nextRun = nextRun.AddDays(1);
                await Task.Delay(nextRun - now, stoppingToken);

                await DetectApproachingDeadlinesAsync();
            }
        }

        private async Task DetectApproachingDeadlinesAsync()
        {
            using var scope = scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<FreelancerDbContext>();
            var eventRepo = scope.ServiceProvider.GetRequiredService<IEventRepository>();

            try
            {
                var now = DateTime.UtcNow;
                var threshold = now.AddHours(48);

                var projects = await db.projects
                    .Include(p => p.Client)
                    .Where(p => p.Status == ProjectStatus.Active
                             && p.Deadline != null
                             && p.Deadline > now
                             && p.Deadline <= threshold)
                    .ToListAsync();

                foreach (var project in projects)
                {
                    await eventRepo.EmitAsync(
                        project.UserId, "Project", project.Id, "DeadlineApproaching",
                        new
                        {
                            project.Id,
                            project.Title,
                            project.Deadline,
                            HoursRemaining = (int)(project.Deadline!.Value - now).TotalHours,
                            ClientName = project.Client?.Name
                        }
                    );
                    logger.LogInformation("DeadlineApproaching event emitted for Project {Id}", project.Id);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in DeadlineApproachingDetector");
            }
        }
    }

}

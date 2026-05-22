using Freelance_Bot.Domain.Entity;
using Freelance_Bot.Domain.Enum;
using Freelance_Bot.Domain.IRepository;
using Freelance_Bot.Infrastruction.DB;
using Microsoft.EntityFrameworkCore;
using System;
using TaskStatusEnum = Freelance_Bot.Domain.Enum.TaskStatus;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance_Bot.Infrastruction.Repos
{
    public class ProjectRepository(FreelancerDbContext db)
     : Repository<Project>(db), IProjectRepository
    {
        public async Task<IEnumerable<Project>> GetByUserIdAsync(Guid userId)
            => await _db.projects
                .Include(p => p.Client)
                .Include(p => p.Tasks)
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.UpdatedAt)
                .ToListAsync();

        public async Task<Project?> GetWithDetailsAsync(Guid id)
            => await _db.projects
                .Include(p => p.Client)
                .Include(p => p.Tasks.Where(t => t.ParentTaskId == null))
                    .ThenInclude(t => t.SubTasks)
                .FirstOrDefaultAsync(p => p.Id == id);

        public async Task<IEnumerable<Project>> GetActiveProjectsAsync(Guid userId)
            => await _db.projects
                .Include(p => p.Client)
                .Include(p => p.Tasks)
                .Where(p => p.UserId == userId && p.Status == ProjectStatus.Active)
                .ToListAsync();

        public async Task<IEnumerable<Project>> GetProjectsWithDeadlinesAsync(Guid userId, int daysAhead)
        {
            var cutoff = DateTime.UtcNow.AddDays(daysAhead);
            return await _db.projects
                .Include(p => p.Client)
                .Where(p => p.UserId == userId
                         && p.Status == ProjectStatus.Active
                         && p.Deadline != null
                         && p.Deadline <= cutoff)
                .ToListAsync();
        }

        public async Task RecalculateProgressAsync(Guid projectId)
        {
            var project = await _db.projects
                .Include(p => p.Tasks)
                .FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null) return;

            var total = project.Tasks.Count(t => t.ParentTaskId == null);
            if (total == 0) { project.ProgressPct = 0; }
            else
            {
                var done = project.Tasks.Count(t =>
                    t.ParentTaskId == null && t.Status == TaskStatusEnum.Done);
                project.ProgressPct = (int)Math.Round((double)done / total * 100);
            }
            await _db.SaveChangesAsync();
        }

        public async Task<object> GetAnalyticsDataAsync(Guid userId)
        {
            var projects = await _db.projects
                .Include(p => p.Client)
                .Include(p => p.Tasks)
                .Where(p => p.UserId == userId && p.Status == ProjectStatus.Active)
                .ToListAsync();

            var now = DateTime.UtcNow;
            return projects.Select(p => new
            {
                p.Id,
                p.Title,
                ClientName = p.Client?.Name,
                p.Status,
                p.ProgressPct,
                p.Deadline,
                DaysUntilDeadline = p.Deadline.HasValue
                    ? (int)(p.Deadline.Value - now).TotalDays : 999,
                TotalTasks = p.Tasks.Count,
                CompletedTasks = p.Tasks.Count(t => t.Status == TaskStatusEnum.Done),
                OverdueTasks = p.Tasks.Count(t => t.DueDate < now && t.Status != TaskStatusEnum.Done),
                LastActivity = p.UpdatedAt
            });
        }
    }
}

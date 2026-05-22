using Freelance_Bot.Domain.Entity;
using Freelance_Bot.Domain.IRepository;
using Freelance_Bot.Infrastruction.DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance_Bot.Infrastruction.Repos
{
    public class InsightRepository(FreelancerDbContext db)
    : Repository<Insight>(db), IInsightRepository
    {
        public async Task<IEnumerable<Insight>> GetActiveInsightsAsync(Guid userId)
            => await _db.insights
                .Include(i => i.Project)
                .Where(i => i.UserId == userId
                         && !i.IsDismissed
                         && (i.ExpiresAt == null || i.ExpiresAt > DateTime.UtcNow))
                .OrderByDescending(i => i.Severity)
                .ThenByDescending(i => i.CreatedAt)
                .ToListAsync();

        public async Task<IEnumerable<Insight>> GetByProjectAsync(Guid projectId)
            => await _db.insights
                .Where(i => i.ProjectId == projectId && !i.IsDismissed)
                .ToListAsync();

        public async Task AddRangeAsync(IEnumerable<Insight> insights)
        {
            await _db.insights.AddRangeAsync(insights);
            await _db.SaveChangesAsync();
        }

        public async Task DismissAsync(Guid insightId)
        {
            var insight = await _db.insights.FindAsync(insightId);
            if (insight != null) { insight.IsDismissed = true; await _db.SaveChangesAsync(); }
        }

        public async Task DismissAllForUserAsync(Guid userId)
        {
            await _db.insights
                .Where(i => i.UserId == userId && !i.IsDismissed)
                .ExecuteUpdateAsync(s => s.SetProperty(i => i.IsDismissed, true));
        }
    }
}

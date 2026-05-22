using Freelance_Bot.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance_Bot.Domain.IRepository
{
    public interface IInsightRepository : IRepository<Insight>
    {
        Task<IEnumerable<Insight>> GetActiveInsightsAsync(Guid userId);
        Task<IEnumerable<Insight>> GetByProjectAsync(Guid projectId);
        Task AddRangeAsync(IEnumerable<Insight> insights);
        Task DismissAsync(Guid insightId);
        Task DismissAllForUserAsync(Guid userId);
    }
}

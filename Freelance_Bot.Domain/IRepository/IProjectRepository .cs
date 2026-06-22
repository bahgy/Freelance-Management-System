using Freelance_Bot.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance_Bot.Domain.IRepository
{
    public interface IProjectRepository : IRepository<Project>
    {
        Task<IEnumerable<Project>> GetByUserIdAsync(Guid userId);
        Task<Project?> GetWithDetailsAsync(Guid id); // includes Tasks + Client
        Task<IEnumerable<Project>> GetActiveProjectsAsync(Guid userId);
        Task<IEnumerable<Project>> GetProjectsWithDeadlinesAsync(Guid userId, int daysAhead);
        Task RecalculateProgressAsync(Guid projectId);
        
        Task<object> GetAnalyticsDataAsync(Guid userId); // returns structured data for AI
    }
}

using Freelance_Bot.Domain.Entity;
using TaskStatusEnum = Freelance_Bot.Domain.Enum.TaskStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance_Bot.Domain.IRepository
{
    public interface ITaskRepository : IRepository<TaskItem>
    {
        Task<IEnumerable<TaskItem>> GetByProjectIdAsync(Guid projectId);
        Task<IEnumerable<TaskItem>> GetOverdueTasksAsync(Guid userId);
        Task<IEnumerable<TaskItem>> GetOverdueByProjectAsync(Guid projectId);
        Task AddRangeAsync(IEnumerable<TaskItem> tasks);
        Task<IEnumerable<TaskItem>> GetByStatusAsync(Guid projectId, TaskStatusEnum status);
    }
}

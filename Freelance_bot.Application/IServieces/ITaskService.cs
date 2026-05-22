using Freelance_bot.Application.Feature.Tasks.Request;
using Freelance_bot.Application.Feature.Tasks.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance_bot.Application.IServieces
{

    public interface ITaskService
    {
        Task<TaskResponse> CreateAsync(Guid userId, CreateTaskRequest request);
        Task<IEnumerable<TaskResponse>> CreateBulkAsync(Guid userId, BulkCreateTasksRequest request);
        Task<IEnumerable<TaskResponse>> GetByProjectAsync(Guid projectId, Guid userId);
        Task<TaskResponse> UpdateAsync(Guid id, Guid userId, UpdateTaskRequest request);
        Task DeleteAsync(Guid id, Guid userId);
        Task<IEnumerable<OverdueTaskResponse>> GetOverdueAsync(Guid userId);
    }
}

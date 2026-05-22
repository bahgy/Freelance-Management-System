using Freelance_Bot.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance_Bot.Domain.IRepository
{
    public interface IEventRepository : IRepository<AppEvent>
    {
        Task<IEnumerable<AppEvent>> GetUnprocessedEventsAsync();
        Task<IEnumerable<AppEvent>> GetUnprocessedByNameAsync(string eventName);
        Task MarkAsProcessedAsync(Guid eventId, string processedBy);
        Task MarkManyProcessedAsync(IEnumerable<Guid> eventIds, string processedBy);
        Task<AppEvent> EmitAsync(Guid userId, string entityType, Guid? entityId, string eventName, object? payload);
    }
}

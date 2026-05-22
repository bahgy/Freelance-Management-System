using Freelance_bot.Application.Feature.Event.Requests;
using Freelance_bot.Application.Feature.Event.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance_bot.Application.IServieces
{
    public interface IEventService
    {
        Task<UnprocessedEventsResponse> GetUnprocessedAsync();
        Task MarkProcessedAsync(MarkEventProcessedRequest request);
        Task EmitAsync(Guid userId, string entityType, Guid? entityId, string eventName, object? payload = null);
    }
}

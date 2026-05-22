using Freelance_bot.Application.Feature.Event.Requests;
using Freelance_bot.Application.Feature.Event.Responses;
using Freelance_bot.Application.IServieces;
using Freelance_Bot.Domain.IRepository;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance_bot.Application.Servieces
{
    public class EventService(IEventRepository eventRepo) : IEventService
    {
        public async Task<UnprocessedEventsResponse> GetUnprocessedAsync()
        {
            var events = await eventRepo.GetUnprocessedEventsAsync();
            return new UnprocessedEventsResponse(
                events.Select(e => new EventDto(
                    e.Id, e.EventName, e.EntityType, e.EntityId, e.Payload, e.CreatedAt
                )).ToList()
            );
        }

        public async Task MarkProcessedAsync(MarkEventProcessedRequest request)
            => await eventRepo.MarkAsProcessedAsync(request.EventId, request.ProcessedBy);

        public async Task EmitAsync(Guid userId, string entityType,
            Guid? entityId, string eventName, object? payload = null)
            => await eventRepo.EmitAsync(userId, entityType, entityId, eventName, payload);
    }
}

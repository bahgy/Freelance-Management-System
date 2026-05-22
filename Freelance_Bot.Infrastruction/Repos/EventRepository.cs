using Freelance_Bot.Domain.Entity;
using Freelance_Bot.Domain.IRepository;
using Freelance_Bot.Infrastruction.DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Freelance_Bot.Infrastruction.Repos
{
    public class EventRepository(FreelancerDbContext db)
    : Repository<AppEvent>(db), IEventRepository
    {
        public async Task<IEnumerable<AppEvent>> GetUnprocessedEventsAsync()
            => await _db.events
                .Where(e => !e.IsProcessed)
                .OrderBy(e => e.CreatedAt)
                .Take(100) // process in batches
                .ToListAsync();

        public async Task<IEnumerable<AppEvent>> GetUnprocessedByNameAsync(string eventName)
            => await _db.events
                .Where(e => !e.IsProcessed && e.EventName == eventName)
                .ToListAsync();

        public async Task MarkAsProcessedAsync(Guid eventId, string processedBy)
        {
            var ev = await _db.events.FindAsync(eventId);
            if (ev == null) return;
            ev.IsProcessed = true;
            ev.ProcessedBy = processedBy;
            ev.ProcessedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
        }

        public async Task MarkManyProcessedAsync(IEnumerable<Guid> eventIds, string processedBy)
        {
            var ids = eventIds.ToList();
            await _db.events
                .Where(e => ids.Contains(e.Id))
                .ExecuteUpdateAsync(s => s
                    .SetProperty(e => e.IsProcessed, true)
                    .SetProperty(e => e.ProcessedBy, processedBy)
                    .SetProperty(e => e.ProcessedAt, DateTime.UtcNow));
        }

        public async Task<AppEvent> EmitAsync(Guid userId, string entityType,
            Guid? entityId, string eventName, object? payload)
        {
            var ev = new AppEvent
            {
                UserId = userId,
                EntityType = entityType,
                EntityId = entityId,
                EventName = eventName,
                Payload = payload != null ? JsonSerializer.Serialize(payload) : null
            };
            await _db.events.AddAsync(ev);
            await _db.SaveChangesAsync();
            return ev;
        }
    }
}

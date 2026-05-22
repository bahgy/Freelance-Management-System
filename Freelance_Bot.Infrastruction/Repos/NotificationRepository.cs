using Freelance_Bot.Domain.Entity;
using Freelance_Bot.Domain.Enum;
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
    public class NotificationRepository(FreelancerDbContext db)
     : Repository<Notification>(db), INotificationRepository
    {
        public async Task<IEnumerable<Notification>> GetByUserIdAsync(Guid userId)
            => await _db.notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .Take(50)
                .ToListAsync();

        public async Task<IEnumerable<Notification>> GetUnreadAsync(Guid userId)
            => await _db.notifications
                .Where(n => n.UserId == userId && n.ReadAt == null)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

        public async Task MarkAsReadAsync(Guid notificationId)
        {
            var n = await _db.notifications.FindAsync(notificationId);
            if (n != null) { n.ReadAt = DateTime.UtcNow; await _db.SaveChangesAsync(); }
        }

        public async Task MarkAllReadAsync(Guid userId)
        {
            await _db.notifications
                .Where(n => n.UserId == userId && n.ReadAt == null)
                .ExecuteUpdateAsync(s => s.SetProperty(n => n.ReadAt, DateTime.UtcNow));
        }

        public async Task UpdateStatusAsync(Guid notificationId, NotificationStatus status)
        {
            var n = await _db.notifications.FindAsync(notificationId);
            if (n != null) { n.Status = status; await _db.SaveChangesAsync(); }
        }
    }
}

using Freelance_Bot.Domain.Entity;
using Freelance_Bot.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance_Bot.Domain.IRepository
{
    public interface INotificationRepository : IRepository<Notification>
    {
        Task<IEnumerable<Notification>> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<Notification>> GetUnreadAsync(Guid userId);
        Task MarkAsReadAsync(Guid notificationId);
        Task MarkAllReadAsync(Guid userId);
        Task UpdateStatusAsync(Guid notificationId, NotificationStatus status);
    }
}

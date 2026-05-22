using Freelance_bot.Application.Feature.Notifications.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance_bot.Application.IServieces
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationResponse>> GetByUserAsync(Guid userId);
        Task MarkReadAsync(Guid notificationId, Guid userId);
        Task MarkAllReadAsync(Guid userId);
    }
}

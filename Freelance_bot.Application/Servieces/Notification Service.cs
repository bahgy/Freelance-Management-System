using Freelance_bot.Application.Feature.Notifications.Response;
using Freelance_bot.Application.IServieces;
using Freelance_Bot.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance_bot.Application.Servieces
{

    public class NotificationService(INotificationRepository notifRepo) : INotificationService
    {
        public async Task<IEnumerable<NotificationResponse>> GetByUserAsync(Guid userId)
        {
            var notifs = await notifRepo.GetByUserIdAsync(userId);
            return notifs.Select(n => new NotificationResponse(
                n.Id, n.Title, n.Body, n.Channel, n.Status, n.SentAt, n.CreatedAt));
        }

        public async Task MarkReadAsync(Guid notificationId, Guid userId)
        {
            var n = await notifRepo.GetByIdAsync(notificationId)
                ?? throw new KeyNotFoundException();
            if (n.UserId != userId) throw new UnauthorizedAccessException();
            await notifRepo.MarkAsReadAsync(notificationId);
        }

        public async Task MarkAllReadAsync(Guid userId)
            => await notifRepo.MarkAllReadAsync(userId);
    }
}

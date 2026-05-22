using Freelance_Bot.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance_bot.Application.Feature.Notifications.Request
{
    public record CreateNotificationRequest(
        Guid UserId,
        string Title,
        string Body,
        NotificationChannel Channel
    );
}

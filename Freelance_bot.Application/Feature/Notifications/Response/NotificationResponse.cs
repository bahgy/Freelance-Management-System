using Freelance_Bot.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance_bot.Application.Feature.Notifications.Response
{
    public record NotificationResponse(
     Guid Id,
     string Title,
     string Body,
     NotificationChannel Channel,
     NotificationStatus Status,
     DateTime? SentAt,
     DateTime CreatedAt
 );
}

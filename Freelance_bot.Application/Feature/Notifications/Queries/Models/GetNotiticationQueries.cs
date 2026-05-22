using Freelance_bot.Application.Feature.Event.Responses;
using Freelance_bot.Application.Feature.Notifications.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance_bot.Application.Feature.Notifications.Queries.Models
{
    public class GetNotificationQueries
          : IRequest<IEnumerable<NotificationResponse>>
    {
    }
}

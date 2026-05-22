using AutoMapper;
using Freelance_bot.Application.Feature.Event.Queries.Models;
using Freelance_bot.Application.Feature.Event.Responses;
using Freelance_bot.Application.Feature.Notifications.Queries.Models;
using Freelance_bot.Application.Feature.Notifications.Response;
using Freelance_Bot.Domain.IRepository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance_bot.Application.Feature.Notifications.Queries.Handler
{
    public class NotificationsHandler
       : IRequestHandler<GetNotificationQueries, IEnumerable<NotificationResponse>>
    {
        #region Fields

        private readonly INotificationRepository _notificationRepo;
        private readonly IMapper _mapper;

        #endregion

        #region Ctor

        public NotificationsHandler(
            INotificationRepository notificationRepo,
            IMapper mapper)
        {
            _notificationRepo = notificationRepo;
            _mapper = mapper;
        }

        #endregion

        #region Handle

        public async Task<IEnumerable<NotificationResponse>> Handle(
            GetNotificationQueries request,
            CancellationToken cancellationToken)
        {
            var notificationList = await _notificationRepo.GetAllAsync();

            var notificationDtoList =
                _mapper.Map<IEnumerable<NotificationResponse>>(notificationList);

            return notificationDtoList;
        }

        #endregion
    }
}

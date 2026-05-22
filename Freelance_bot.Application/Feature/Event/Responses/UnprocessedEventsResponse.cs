using Freelance_Bot.Domain.Enum;
using Freelance_bot.Application.Feature.Event.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance_bot.Application.Feature.Event.Responses
{
    public record UnprocessedEventsResponse(IEnumerable<EventDto> Events);

    
}

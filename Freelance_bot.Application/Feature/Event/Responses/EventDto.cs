using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance_bot.Application.Feature.Event.Responses
{
    public record EventDto(
     Guid Id,
     string EventName,
     string EntityType,
     Guid? EntityId,
     string? Payload,
     DateTime CreatedAt
 );
}

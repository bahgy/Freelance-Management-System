using Freelance_Bot.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance_bot.Application.Feature.Event.Requests
{
    public record MarkEventProcessedRequest(  Guid EventId, string ProcessedBy = "n8n");
}

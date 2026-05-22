using Freelance_Bot.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance_bot.Application.Feature.Insight.Response
{
    public record InsightResponse(
    Guid Id,
    InsightCategory Category,
    InsightSeverity Severity,
    string Message,
    string? ActionHint,
    string? ProjectTitle,
    bool IsDismissed,
    DateTime CreatedAt
);
}

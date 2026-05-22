using Freelance_Bot.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance_bot.Application.Feature.Projects.Responses
{
    public record ProjectSummaryResponse(
     Guid Id,
     string Title,
     ProjectStatus Status,
     int ProgressPct,
     DateTime? Deadline,
     int DaysUntilDeadline
 );
}

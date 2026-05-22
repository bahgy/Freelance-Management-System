using Freelance_Bot.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance_bot.Application.Feature.Projects.Requests
{
    public record UpdateProjectRequest(
     string? Title,
     string? Description,
     ProjectStatus? Status,
     decimal? Budget,
     DateTime? Deadline,
     int? ProgressPct
 );
}

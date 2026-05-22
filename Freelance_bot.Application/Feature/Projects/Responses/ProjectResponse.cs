using Freelance_Bot.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance_bot.Application.Feature.Projects.Responses
{
    public record ProjectResponse(
    Guid Id,
    string Title,
    string? Description,
    ProjectStatus Status,
    decimal? Budget,
    string Currency,
    DateTime? StartDate,
    DateTime? Deadline,
    int ProgressPct,
    string? ClientName,
    int TotalTasks,
    int CompletedTasks,
    int OverdueTasks,
    DateTime CreatedAt
);
}

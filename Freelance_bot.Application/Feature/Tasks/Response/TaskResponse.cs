using Freelance_Bot.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance_bot.Application.Feature.Tasks.Response
{
    public record TaskResponse(
    Guid Id,
    Guid ProjectId,
    string ProjectTitle,
    string Title,
    string? Notes,
    Freelance_Bot.Domain.Enum.TaskStatus Status,
    TaskPriority Priority,
    DateTime? DueDate,
    bool IsOverdue,
    bool IsDefault,
    List<TaskResponse> SubTasks,
    DateTime CreatedAt
);
}

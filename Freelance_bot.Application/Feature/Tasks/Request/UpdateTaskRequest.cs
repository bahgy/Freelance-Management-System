using Freelance_Bot.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance_bot.Application.Feature.Tasks.Request
{
    public record UpdateTaskRequest(
    string? Title,
    string? Notes,
    Freelance_Bot.Domain.Enum.TaskStatus? Status,
    TaskPriority? Priority,
    DateTime? DueDate
);
}

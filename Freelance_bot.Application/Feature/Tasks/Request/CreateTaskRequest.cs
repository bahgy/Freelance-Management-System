using Freelance_Bot.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance_bot.Application.Feature.Tasks.Request
{
    public record CreateTaskRequest(
     Guid ProjectId,
     string Title,
     string? Notes,
     TaskPriority Priority,
     DateTime? DueDate,
     Guid? ParentTaskId
 );
}

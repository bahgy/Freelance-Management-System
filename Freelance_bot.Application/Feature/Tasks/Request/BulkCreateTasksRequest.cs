using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance_bot.Application.Feature.Tasks.Request
{
    public record BulkCreateTasksRequest(
    Guid ProjectId,
    List<CreateTaskRequest> Tasks
);
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance_bot.Application.Feature.Tasks.Response
{
    public record OverdueTaskResponse(
    Guid Id,
    string Title,
    string ProjectTitle,
    Guid ProjectId,
    string? ClientName,
    DateTime DueDate,
    int DaysOverdue
);
}

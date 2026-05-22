using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance_bot.Application.Feature.Projects.Requests
{
    public record CreateProjectRequest(
    string Title,
    string? Description,
    Guid? ClientId,
    decimal? Budget,
    string Currency,
    DateTime? StartDate,
    DateTime? Deadline
);
}

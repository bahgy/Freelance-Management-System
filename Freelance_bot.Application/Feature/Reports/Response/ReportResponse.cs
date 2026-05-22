using Freelance_Bot.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance_bot.Application.Feature.Reports.Response
{
    public record ReportResponse(
    Guid Id,
    Guid ProjectId,
    string ProjectTitle,
    ReportType Type,
    //ReportStatus Status,
    string? AiSummary,
    string? AiSuggestions,
    string? SentVia,
    DateTime? SentAt,
    DateTime? GeneratedAt,
    DateTime CreatedAt
);
}

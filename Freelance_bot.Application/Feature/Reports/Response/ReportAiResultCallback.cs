using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance_bot.Application.Feature.Reports.Response
{
    // n8n callback — n8n بيبعت الـ AI result هنا
    public record ReportAiResultCallback(
        Guid ReportId,
        string AiSummary,
        string AiSuggestions,
        string Status = "completed"
    );
}

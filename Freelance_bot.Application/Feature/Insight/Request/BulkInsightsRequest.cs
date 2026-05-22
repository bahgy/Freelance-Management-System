using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance_bot.Application.Feature.Insight.Request
{
    public record BulkInsightsRequest(
      Guid UserId,
      List<CreateInsightRequest> Insights
  );
}

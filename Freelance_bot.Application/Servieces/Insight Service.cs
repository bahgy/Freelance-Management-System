using Freelance_bot.Application.Feature.Insight.Request;
using Freelance_bot.Application.Feature.Insight.Response;
using Freelance_bot.Application.IServieces;
using Freelance_Bot.Domain.Entity;
using Freelance_Bot.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance_bot.Application.Servieces
{

    public class InsightService(IInsightRepository insightRepo) : IInsightService
    {
        public async Task<IEnumerable<InsightResponse>> GetActiveAsync(Guid userId)
        {
            var insights = await insightRepo.GetActiveInsightsAsync(userId);
            return insights.Select(MapToResponse);
        }

        public async Task BulkCreateAsync(BulkInsightsRequest request)
        {
            var insights = request.Insights.Select(i => new Insight
            {
                UserId = request.UserId,
                ProjectId = i.ProjectId,
                Category = i.Category,
                Severity = i.Severity,
                Message = i.Message,
                ActionHint = i.ActionHint,
                ExpiresAt = i.ExpiresAt ?? DateTime.UtcNow.AddDays(7),
                TriggerSource = "cron"
            }).ToList();

            await insightRepo.AddRangeAsync(insights);
        }

        public async Task DismissAsync(Guid insightId, Guid userId)
        {
            var insight = await insightRepo.GetByIdAsync(insightId)
                ?? throw new KeyNotFoundException("Insight not found");
            if (insight.UserId != userId) throw new UnauthorizedAccessException();
            await insightRepo.DismissAsync(insightId);
        }

        private static InsightResponse MapToResponse(Insight i)
            => new(i.Id, i.Category, i.Severity, i.Message, i.ActionHint,
                   i.Project?.Title, i.IsDismissed, i.CreatedAt);
    }

}

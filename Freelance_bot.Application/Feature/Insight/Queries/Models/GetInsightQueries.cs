using Freelance_bot.Application.Feature.Event.Responses;
using Freelance_bot.Application.Feature.Insight.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance_bot.Application.Feature.Insight.Queries.Models
{
    public class GetInsightQueries
        : IRequest<IEnumerable<InsightResponse>>
    {
    }
}

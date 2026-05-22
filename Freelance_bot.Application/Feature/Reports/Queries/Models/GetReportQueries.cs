using Freelance_bot.Application.Feature.Event.Responses;
using Freelance_bot.Application.Feature.Reports.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance_bot.Application.Feature.Reports.Queries.Models
{
    public class GetReportQueries : IRequest<IEnumerable<ReportResponse>> /// dto 
    {
    }
}

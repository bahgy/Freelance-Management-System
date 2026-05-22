using Freelance_bot.Application.Feature.Reports.Request;
using Freelance_bot.Application.Feature.Reports.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance_bot.Application.IServieces
{

    public interface IReportService
    {
        Task<ReportResponse> GenerateAsync(Guid userId, GenerateReportRequest request);
        Task<ReportResponse> SendAsync(Guid userId, SendReportRequest request);
        Task<ReportResponse> AutoSendAsync(Guid userId, AutoSendRequest request);
        Task<IEnumerable<ReportResponse>> GetByProjectAsync(Guid projectId, Guid userId);
        Task HandleAiCallbackAsync(ReportAiResultCallback callback); // called by n8n
    }
}

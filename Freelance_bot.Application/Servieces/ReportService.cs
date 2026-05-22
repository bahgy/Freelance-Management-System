//using Freelance_bot.Application.Feature.Reports.Request;
//using Freelance_bot.Application.Feature.Reports.Response;
//using Freelance_bot.Application.IServieces;
//using Freelance_Bot.Domain.Entity;
//using Freelance_Bot.Domain.Enum;
//using Freelance_Bot.Domain.IRepository;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Text.Json;
//using System.Threading.Tasks;

//namespace Freelance_bot.Application.Servieces
//{

//    public class ReportService(
//        IReportRepository reportRepo,
//        IProjectRepository projectRepo,
//        IEventService eventService) : IReportService
//    {
//        public async Task<ReportResponse> GenerateAsync(Guid userId, GenerateReportRequest request)
//        {
//            var project = await projectRepo.GetWithDetailsAsync(request.ProjectId)
//                ?? throw new KeyNotFoundException("Project not found");
//            if (project.UserId != userId) throw new UnauthorizedAccessException();

//            // Collect raw data for AI
//            var rawData = new
//            {
//                project.Id,
//                project.Title,
//                project.Status,
//                project.ProgressPct,
//                project.Deadline,
//                ClientName = project.Client?.Name,
//                Tasks = project.Tasks.Select(t => new { t.Title, t.Status, t.DueDate, t.Priority }),
//                GeneratedAt = DateTime.UtcNow
//            };

//            var report = new Report
//            {
//                ProjectId = project.Id,
//                GeneratedBy = userId,
//                Type = request.Type,
//                Status = ReportStatus.Generating,
//                RawData = JsonSerializer.Serialize(rawData)
//            };

//            await reportRepo.AddAsync(report);

//            // Emit event — n8n will send raw_data to Claude AI
//            await eventService.EmitAsync(userId, "Report", report.Id, "ReportRequested", new
//            {
//                ReportId = report.Id,
//                RawData = rawData
//            });

//            return MapToResponse(report, project.Title);
//        }

//        public async Task<ReportResponse> SendAsync(Guid userId, SendReportRequest request)
//        {
//            var report = await reportRepo.GetByIdAsync(request.ReportId)
//                ?? throw new KeyNotFoundException("Report not found");
//            var project = await projectRepo.GetByIdAsync(report.ProjectId)!;
//            if (project!.UserId != userId) throw new UnauthorizedAccessException();

//            await eventService.EmitAsync(userId, "Report", report.Id, "ReportSendRequested", new
//            {
//                ReportId = report.Id,
//                report.AiSummary,
//                report.AiSuggestions,
//                Channel = request.Channel,
//                ProjectTitle = project.Title,
//                ClientId = project.ClientId
//            });

//            return MapToResponse(report, project.Title);
//        }

//        public async Task<ReportResponse> AutoSendAsync(Guid userId, AutoSendRequest request)
//        {
//            // Single endpoint: generate + send in one shot via n8n
//            var project = await projectRepo.GetWithDetailsAsync(request.ProjectId)
//                ?? throw new KeyNotFoundException("Project not found");
//            if (project.UserId != userId) throw new UnauthorizedAccessException();

//            var report = new Report
//            {
//                ProjectId = project.Id,
//                GeneratedBy = userId,
//                Type = ReportType.ClientFacing,
//                Status = ReportStatus.Pending,
//            };
//            await reportRepo.AddAsync(report);

//            await eventService.EmitAsync(userId, "Report", report.Id, "AutoSendRequested", new
//            {
//                ReportId = report.Id,
//                ProjectId = project.Id,
//                ProjectTitle = project.Title,
//                ClientId = project.ClientId,
//                Channel = request.Channel,
//                Tasks = project.Tasks.Select(t => new { t.Title, t.Status, t.Priority, t.DueDate })
//            });

//            return MapToResponse(report, project.Title);
//        }

//        public async Task<IEnumerable<ReportResponse>> GetByProjectAsync(Guid projectId, Guid userId)
//        {
//            var project = await projectRepo.GetByIdAsync(projectId)
//                ?? throw new KeyNotFoundException("Project not found");
//            if (project.UserId != userId) throw new UnauthorizedAccessException();

//            var reports = await reportRepo.GetByProjectIdAsync(projectId);
//            return reports.Select(r => MapToResponse(r, project.Title));
//        }

//        // Called by n8n webhook after Claude AI finishes
//        public async Task HandleAiCallbackAsync(ReportAiResultCallback callback)
//        {
//            var status = Enum.Parse<ReportStatus>(callback.Status, true);
//            await reportRepo.UpdateStatusAsync(
//                callback.ReportId, status,
//                callback.AiSummary, callback.AiSuggestions);
//        }

//        private static ReportResponse MapToResponse(Report report, string projectTitle)
//            => new(report.Id, report.ProjectId, projectTitle, report.Type,
//                   report.Status, report.AiSummary, report.AiSuggestions,
//                   report.SentVia, report.SentAt, report.GeneratedAt, report.CreatedAt);
//    }
//}

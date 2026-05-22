using Freelance_bot.Application.Feature.Projects.Requests;
using Freelance_bot.Application.Feature.Projects.Responses;
using TaskStatusEnum = Freelance_Bot.Domain.Enum.TaskStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Freelance_Bot.Domain.IRepository;
using Freelance_bot.Application.IServieces;
using Freelance_Bot.Domain.Entity;
using Freelance_Bot.Domain.Enum;

namespace Freelance_bot.Application.Servieces
{
    public class ProjectService(
    IProjectRepository projectRepo,
    IEventService eventService,
    ITaskRepository taskRepo) : IProjectService
    {
        public async Task<ProjectResponse> CreateAsync(Guid userId, CreateProjectRequest request)
        {
            var project = new Project
            {
                UserId = userId,
                Title = request.Title,
                Description = request.Description,
                ClientId = request.ClientId,
                Budget = request.Budget,
                Currency = request.Currency,
                StartDate = request.StartDate ?? DateTime.UtcNow,
                Deadline = request.Deadline,
                Status = ProjectStatus.Active
            };

            await projectRepo.AddAsync(project);

            // Emit ProjectCreated event — n8n will pick this up and create default tasks
            await eventService.EmitAsync(userId, "Project", project.Id, "ProjectCreated", new
            {
                project.Id,
                project.Title,
                project.Deadline,
                project.ClientId
            });

            return await MapToResponseAsync(project);
        }

        public async Task<ProjectResponse?> GetByIdAsync(Guid id, Guid userId)
        {
            var project = await projectRepo.GetWithDetailsAsync(id);
            if (project == null || project.UserId != userId) return null;
            return await MapToResponseAsync(project);
        }

        public async Task<IEnumerable<ProjectResponse>> GetAllAsync(Guid userId)
        {
            var projects = await projectRepo.GetByUserIdAsync(userId);
            var results = new List<ProjectResponse>();
            foreach (var p in projects)
                results.Add(await MapToResponseAsync(p));
            return results;
        }

        public async Task<ProjectResponse> UpdateAsync(Guid id, Guid userId, UpdateProjectRequest request)
        {
            var project = await projectRepo.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("Project not found");

            if (project.UserId != userId) throw new UnauthorizedAccessException();

            if (request.Title != null) project.Title = request.Title;
            if (request.Description != null) project.Description = request.Description;
            if (request.Status.HasValue) project.Status = request.Status.Value;
            if (request.Budget.HasValue) project.Budget = request.Budget.Value;
            if (request.Deadline.HasValue) project.Deadline = request.Deadline.Value;
            if (request.ProgressPct.HasValue) project.ProgressPct = request.ProgressPct.Value;

            await projectRepo.UpdateAsync(project);
            return await MapToResponseAsync(project);
        }

        public async Task DeleteAsync(Guid id, Guid userId)
        {
            var project = await projectRepo.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("Project not found");
            if (project.UserId != userId) throw new UnauthorizedAccessException();
            await projectRepo.DeleteAsync(id);
        }

        //public async Task<AnalyticsDataResponse> GetAnalyticsDataAsync(Guid userId)
        //{
        //    var raw = await projectRepo.GetAnalyticsDataAsync(userId);
        //    var projects = (raw as IEnumerable<dynamic> ?? []).ToList();

        //    return new AnalyticsDataResponse(
        //        Projects: [],
        //        TotalActiveProjects: projects.Count,
        //        TotalOverdueTasks: 0,
        //        InactiveClients: 0,
        //        GeneratedAt: DateTime.UtcNow
        //    );
        //}

        //public async Task<DashboardSummaryResponse> GetDashboardSummaryAsync(Guid userId)
        //{
        //    var activeProjects = await projectRepo.GetActiveProjectsAsync(userId);
        //    var overdueTasks = await taskRepo.GetOverdueTasksAsync(userId);
        //    var deadlineSoon = await projectRepo.GetProjectsWithDeadlinesAsync(userId, 7);

        //    var projectList = activeProjects.ToList();
        //    var overdueList = overdueTasks.ToList();

        //    return new DashboardSummaryResponse(
        //        ActiveProjects: projectList.Count,
        //        PendingTasks: projectList.Sum(p => p.Tasks.Count(t =>
        //            t.Status is Enums.TaskStatus.Todo or Enums.TaskStatus.InProgress)),
        //        OverdueTasks: overdueList.Count,
        //        DeadlinesThisWeek: deadlineSoon.Count(),
        //        RecentProjects: projectList.Take(5).Select(p => new ProjectSummaryResponse(
        //            p.Id, p.Title, p.Status, p.ProgressPct, p.Deadline,
        //            p.Deadline.HasValue ? (int)(p.Deadline.Value - DateTime.UtcNow).TotalDays : 999
        //        )).ToList(),
        //        ActiveInsights: [],
        //        OverdueTasks_List: overdueList.Select(t => new OverdueTaskResponse(
        //            t.Id, t.Title, t.Project.Title, t.ProjectId,
        //            t.Project.Client?.Name, t.DueDate!.Value,
        //            (int)(DateTime.UtcNow - t.DueDate!.Value).TotalDays
        //        )).ToList()
        //    );
        //}

        private Task<ProjectResponse> MapToResponseAsync(Project project)
        {
            var response = new ProjectResponse(
                project.Id,
                project.Title,
                project.Description,
                project.Status,
                project.Budget,
                project.Currency,
                project.StartDate,
                project.Deadline,
                project.ProgressPct,
                project.Client?.Name,
                project.Tasks.Count,
                project.Tasks.Count(t => t.Status == TaskStatusEnum.Done),
                project.Tasks.Count(t => t.DueDate < DateTime.UtcNow && t.Status != TaskStatusEnum.Done),
                project.CreatedAt
            );
            return Task.FromResult(response);
        }

       
    }

}

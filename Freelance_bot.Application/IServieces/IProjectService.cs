using Freelance_bot.Application.Feature.Projects.DTOs;
using Freelance_bot.Application.Feature.Projects.Requests;
using Freelance_bot.Application.Feature.Projects.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance_bot.Application.IServieces
{
    
        public interface IProjectService
        {
            Task<ProjectResponse> CreateAsync(Guid userId, CreateProjectRequest request);
            Task<ProjectResponse?> GetByIdAsync(Guid id, Guid userId);
            Task<IEnumerable<ProjectResponse>> GetAllAsync(Guid userId);
            Task<ProjectResponse> UpdateAsync(Guid id, Guid userId, UpdateProjectRequest request);
            Task DeleteAsync(Guid id, Guid userId);
            Task<List<ProjectBotDto>> GetByTelegramIdAsync(long telegramId);
        //Task<AnalyticsDataResponse> GetAnalyticsDataAsync(Guid userId);
        //Task<DashboardSummaryResponse> GetDashboardSummaryAsync(Guid userId);
    }
       
    
}
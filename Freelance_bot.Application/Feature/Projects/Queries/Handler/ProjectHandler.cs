//using AutoMapper;
//using Freelance_bot.Application.Feature.Projects.DTOs;
//using Freelance_bot.Application.Feature.Projects.Queries.Models;
//using Freelance_Bot.Domain.Entity;
//using Freelance_Bot.Domain.IRepository;
//using MediatR;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Freelance_bot.Application.Feature.Projects.Queries.Handler
//{
//    public class ProjectHandler : IRequestHandler<GetProjectsQueries, IEnumerable<ProjectDto>> /// dto
//    {

//        #region Fields
//        private readonly IProjectRepository _projectRepo;
//        private readonly Mapper _mapper;
//        #endregion

//        #region Ctor
//        public ProjectHandler(IProjectRepository ProjectRepo,Mapper mapper )
//        {
//            _projectRepo = ProjectRepo;
//            _mapper = mapper;
//        }
//        #endregion

//        #region Handle
//        public async Task<IEnumerable<ProjectDto>> Handle(GetProjectsQueries request, CancellationToken cancellationToken)
//        {
//           var projectList= await _projectRepo.GetAllAsync();
//            var projectDtoList =  _mapper.Map<IEnumerable<ProjectDto>>(projectList);
//            return projectDtoList;
//        }
//        #endregion

//    }
//}


using AutoMapper;
using Freelance_bot.Application.Feature.Projects.DTOs;
using Freelance_bot.Application.Feature.Projects.Queries.Models;
using Freelance_bot.Application.Feature.Projects.Responses;
using Freelance_Bot.Domain.IRepository;
using MediatR;

namespace Freelance_bot.Application.Feature.Projects.Queries.Handler
{
    public class ProjectHandler
        : IRequestHandler<GetProjectsQueries, IEnumerable<ProjectResponse>>
    {
        #region Fields

        private readonly IProjectRepository _projectRepo;
        private readonly IMapper _mapper;

        #endregion

        #region Ctor

        public ProjectHandler(
            IProjectRepository ProjectResponse,
            IMapper mapper)
        {
            _projectRepo = ProjectResponse;
            _mapper = mapper;
        }

        #endregion

        #region Handle

        public async Task<IEnumerable<ProjectResponse>> Handle(
            GetProjectsQueries request,
            CancellationToken cancellationToken)
        {
            var projectList = await _projectRepo.GetAllAsync();

            var projectDtoList =
                _mapper.Map<IEnumerable<ProjectResponse>>(projectList);

            return projectDtoList;
        }

        #endregion
    }
}
using AutoMapper;
using Freelance_bot.Application.Feature.Projects.Queries.Models;
using Freelance_bot.Application.Feature.Projects.Responses;
using Freelance_bot.Application.Feature.Tasks.Queries.Models;
using Freelance_bot.Application.Feature.Tasks.Response;
using Freelance_Bot.Domain.IRepository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance_bot.Application.Feature.Tasks.Queries.Handler
{
    public class TaskHandler
          : IRequestHandler<GetTaskQueries, IEnumerable<TaskResponse>>
    {
        #region Fields

        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;

        #endregion

        #region Ctor

        public TaskHandler(
            ITaskRepository taskRepository,
            IMapper mapper)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
        }

        #endregion

        #region Handle

        public async Task<IEnumerable<TaskResponse>> Handle(
            GetTaskQueries request,
            CancellationToken cancellationToken)
        {
            var taskList = await _taskRepository.GetAllAsync();

            var taskDtoList =
                _mapper.Map<IEnumerable<TaskResponse>>(taskList);

            return taskDtoList;
        }
        #endregion
    }
}
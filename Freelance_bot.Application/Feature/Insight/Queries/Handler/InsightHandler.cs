using AutoMapper;
using Freelance_bot.Application.Feature.Event.Queries.Models;
using Freelance_bot.Application.Feature.Event.Responses;
using Freelance_bot.Application.Feature.Insight.Queries.Models;
using Freelance_bot.Application.Feature.Insight.Response;
using Freelance_Bot.Domain.IRepository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance_bot.Application.Feature.Insight.Queries.Handler
{
    public class InsightHandler
         : IRequestHandler<GetInsightQueries, IEnumerable<InsightResponse>>
    {
        #region Fields

        private readonly IInsightRepository _insightRepository;
        private readonly IMapper _mapper;

        #endregion

        #region Ctor

        public InsightHandler(
            IInsightRepository insightRepository,
            IMapper mapper)
        {
            _insightRepository = insightRepository;
            _mapper = mapper;
        }

        #endregion

        #region Handle

        public async Task<IEnumerable<InsightResponse>> Handle(
            GetInsightQueries request,
            CancellationToken cancellationToken)
        {
            var insightList = await _insightRepository.GetAllAsync();

            var insightDtoList =
                _mapper.Map<IEnumerable<InsightResponse>>(insightList);

            return insightDtoList;
        }

        #endregion
    }
}

using AutoMapper;
using Freelance_bot.Application.Feature.Client.Queries.Models;
using Freelance_bot.Application.Feature.Client.Response;
using Freelance_bot.Application.Feature.Event.Responses;
using Freelance_bot.Application.Feature.Projects.Queries.Models;
using Freelance_Bot.Domain.IRepository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance_bot.Application.Feature.Client.Queries.Handler
{
    public class ClientHandler
        : IRequestHandler<GetClientQueries, IEnumerable<ClientResponse>>
    {
        #region Fields

        private readonly IClientRepository _clientRepository;
        private readonly IMapper _mapper;

        #endregion

        #region Ctor

        public ClientHandler(
            IClientRepository clientRepository,
            IMapper mapper)
        {
            _clientRepository = clientRepository;
            _mapper = mapper;
        }

        #endregion

        #region Handle

        public async Task<IEnumerable<ClientResponse>> Handle(
            GetClientQueries request,
            CancellationToken cancellationToken)
        {
            var clientList  = await _clientRepository.GetAllAsync();

            var clientDtoList =
                _mapper.Map<IEnumerable<ClientResponse>>(clientList);

            return clientDtoList;
        }

        #endregion
    }
}
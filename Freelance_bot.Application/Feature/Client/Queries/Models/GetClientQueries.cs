using Freelance_bot.Application.Feature.Client.Response;
using Freelance_bot.Application.Feature.Event.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance_bot.Application.Feature.Client.Queries.Models
{
    public class GetClientQueries : IRequest<IEnumerable<ClientResponse>> /// dto 
    {
    }
}

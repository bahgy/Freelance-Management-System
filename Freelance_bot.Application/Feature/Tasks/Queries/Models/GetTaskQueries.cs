using Freelance_bot.Application.Feature.Projects.Responses;
using Freelance_bot.Application.Feature.Tasks.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance_bot.Application.Feature.Tasks.Queries.Models
{
    public class GetTaskQueries : IRequest<IEnumerable<TaskResponse>> /// dto 
    {
    }
}

using Freelance_bot.Application.Feature.Event.Responses;
using Freelance_bot.Application.Feature.Projects.DTOs;
using Freelance_bot.Application.Feature.Projects.Responses;
using Freelance_Bot.Domain.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance_bot.Application.Feature.Event.Queries.Models
{
    public class GetEventQueries
         : IRequest<IEnumerable<UnprocessedEventsResponse>>
    {
    }
}

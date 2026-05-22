using Freelance_bot.Application.Feature.Projects.DTOs;
using Freelance_bot.Application.Feature.Projects.Responses;
using Freelance_Bot.Domain.Entity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance_bot.Application.Feature.Projects.Queries.Models
{
    public class GetProjectsQueries:IRequest<IEnumerable<ProjectResponse>> /// dto 
    {
    }
}

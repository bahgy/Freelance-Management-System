using Freelance_Bot.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance_bot.Application.Feature.Projects.DTOs
{
    public class ProjectBotDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public ProjectStatus Status { get; set; } = ProjectStatus.Active;


    }
}

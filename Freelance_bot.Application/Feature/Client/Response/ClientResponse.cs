using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance_bot.Application.Feature.Client.Response
{
    public record ClientResponse(
     Guid Id,
     string Name,
     string? Email,
     string? Phone,
     string? Company,
     string Status,
     DateTime? LastContactedAt,
     int ActiveProjects,
     DateTime CreatedAt
 );
}

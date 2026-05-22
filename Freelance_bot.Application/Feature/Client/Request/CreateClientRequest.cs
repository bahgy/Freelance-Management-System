using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance_bot.Application.Feature.Client.Request
{
    public record CreateClientRequest(
    string Name,
    string? Email,
    string? Phone,
    string? Company
);
}

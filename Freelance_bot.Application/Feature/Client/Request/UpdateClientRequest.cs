using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance_bot.Application.Feature.Client.Request
{
    public record UpdateClientRequest(
    string? Name,
    string? Email,
    string? Phone,
    string? Company,
    string? Status
);
}

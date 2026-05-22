using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance_bot.Application.Feature.Reports.Request
{
    public record AutoSendRequest(
    Guid ProjectId,
    string Channel = "both"
);
}

using Freelance_Bot.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance_Bot.Domain.IRepository
{
    public interface IClientRepository : IRepository<Client>
    {
        Task<IEnumerable<Client>> GetByUserIdAsync(Guid userId);
        Task<Client?> GetWithProjectsAsync(Guid id);
        Task<IEnumerable<Client>> GetInactiveClientsAsync(Guid userId, int daysSinceContact);
        Task UpdateLastContactedAsync(Guid clientId);
    }
}

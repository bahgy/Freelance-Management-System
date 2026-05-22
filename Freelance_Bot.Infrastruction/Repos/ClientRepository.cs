using Freelance_Bot.Domain.Entity;
using Freelance_Bot.Domain.IRepository;
using Freelance_Bot.Infrastruction.DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance_Bot.Infrastruction.Repos
{
    public class ClientRepository(FreelancerDbContext db)
     : Repository<Client>(db), IClientRepository
    {
        public async Task<IEnumerable<Client>> GetByUserIdAsync(Guid userId)
            => await _db.clients
                .Include(c => c.Projects)
                .Where(c => c.UserId == userId)
                .OrderByDescending(c => c.UpdatedAt)
                .ToListAsync();

        public async Task<Client?> GetWithProjectsAsync(Guid id)
            => await _db.clients
                .Include(c => c.Projects)
                .FirstOrDefaultAsync(c => c.Id == id);

        public async Task<IEnumerable<Client>> GetInactiveClientsAsync(Guid userId, int daysSinceContact)
        {
            var cutoff = DateTime.UtcNow.AddDays(-daysSinceContact);
            return await _db.clients
                .Where(c => c.UserId == userId
                         && c.Status == "active"
                         && (c.LastContactedAt == null || c.LastContactedAt < cutoff))
                .ToListAsync();
        }

        public async Task UpdateLastContactedAsync(Guid clientId)
        {
            var client = await _db.clients.FindAsync(clientId);
            if (client != null)
            {
                client.LastContactedAt = DateTime.UtcNow;
                await _db.SaveChangesAsync();
            }
        }
    }
}

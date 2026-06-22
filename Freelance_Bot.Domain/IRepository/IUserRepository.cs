using Freelance_Bot.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance_Bot.Domain.IRepository
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByTelegramChatIdAsync(long chatId);
        Task<bool> EmailExistsAsync(string email);
        Task SaveChangesAsync();
        
    }
}

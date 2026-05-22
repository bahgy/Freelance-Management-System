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
    public class UserRepository(FreelancerDbContext db)
    : Repository<User>(db), IUserRepository
    {
        public async Task<User?> GetByEmailAsync(string email)
            => await _db.users.FirstOrDefaultAsync(u => u.Email == email.ToLower());

        public async Task<User?> GetByTelegramChatIdAsync(string chatId)
            => await _db.users.FirstOrDefaultAsync(u => u.TelegramChatId == chatId);

        public async Task<bool> EmailExistsAsync(string email)
            => await _db.users.AnyAsync(u => u.Email == email.ToLower());
    }
}

using Freelance_bot.Application.Feature.Users.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

namespace Freelance_bot.Application.IServieces
{
    public interface IUserService
    {
        Task<UserDto?> GetByIdAsync(Guid id);

        
        Task<UserDto?> GetByEmailAsync(string email);

        
        Task<UserDto> GetOrCreateByTelegramIdAsync(long chatId, string name);
        Task<UserDto> RegisterAsync(RegisterUserDto dto);

    
        Task<bool> EmailExistsAsync(string email);
    }
}

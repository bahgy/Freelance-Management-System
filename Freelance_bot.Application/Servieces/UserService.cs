using Freelance_bot.Application.Feature.Users.DTO;
using Freelance_bot.Application.IServieces;
using Freelance_Bot.Domain.Entity;
using Freelance_Bot.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance_bot.Application.Servieces
{
    public class UserService(IUserRepository userRepository) : IUserService
    {
        public async Task<UserDto?> GetByIdAsync(Guid id)
        {
            var user = await userRepository.GetByIdAsync(id);
            return user is null ? null : MapToDto(user);
        }

        public async Task<UserDto?> GetByEmailAsync(string email)
        {
            var user = await userRepository.GetByEmailAsync(email);
            return user is null ? null : MapToDto(user);
        }

        public async Task<UserDto> GetOrCreateByTelegramIdAsync(long chatId, string name)
        {
            var existing = await userRepository.GetByTelegramChatIdAsync(chatId);
            if (existing is not null)
                return MapToDto(existing);


            var newUser = new User
            {
                Id = Guid.NewGuid(),
                FullName = name,
                TelegramChatId = chatId,
                Email = $"telegram_{chatId}@freelanceos.app", // placeholder
                CreatedAt = DateTime.UtcNow
            };

            await userRepository.AddAsync(newUser);
            await userRepository.SaveChangesAsync();

            return MapToDto(newUser);
        }

     
        public async Task<UserDto> RegisterAsync(RegisterUserDto dto)
        {
       
            if (await userRepository.EmailExistsAsync(dto.Email))
                throw new InvalidOperationException("الإيميل ده مسجل قبل كده.");

            var user = new User
            {
                Id = Guid.NewGuid(),
                FullName = dto.Name,
                Email = dto.Email.ToLower(),
                TelegramChatId = dto.TelegramChatId,
                CreatedAt = DateTime.UtcNow
            };

            await userRepository.AddAsync(user);
            await userRepository.SaveChangesAsync();

            return MapToDto(user);
        }

        
        public async Task<bool> EmailExistsAsync(string email)
            => await userRepository.EmailExistsAsync(email);

     
        private static UserDto MapToDto(User user) => new()
        {
            Id = user.Id,
            Name = user.FullName,
            Email = user.Email,
            TelegramChatId = user.TelegramChatId
        };
    }
}

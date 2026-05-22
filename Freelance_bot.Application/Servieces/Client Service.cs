using Freelance_bot.Application.Feature.Client.Request;
using Freelance_bot.Application.Feature.Client.Response;
using Freelance_bot.Application.IServieces;
using Freelance_Bot.Domain.Entity;
using Freelance_Bot.Domain.Enum;
using Freelance_Bot.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance_bot.Application.Servieces
{

    public class ClientService(IClientRepository clientRepo) : IClientService
    {
        public async Task<ClientResponse> CreateAsync(Guid userId, CreateClientRequest request)
        {
            var client = new Client
            {
                UserId = userId,
                Name = request.Name,
                Email = request.Email,
                Phone = request.Phone,
                Company = request.Company
            };

            await clientRepo.AddAsync(client);
            return MapToResponse(client);
        }

        public async Task<ClientResponse?> GetByIdAsync(Guid id, Guid userId)
        {
            var client = await clientRepo.GetWithProjectsAsync(id);
            if (client == null || client.UserId != userId) return null;
            return MapToResponse(client);
        }

        public async Task<IEnumerable<ClientResponse>> GetAllAsync(Guid userId)
        {
            var clients = await clientRepo.GetByUserIdAsync(userId);
            return clients.Select(MapToResponse);
        }

        public async Task<ClientResponse> UpdateAsync(Guid id, Guid userId, UpdateClientRequest request)
        {
            var client = await clientRepo.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("Client not found");

            if (client.UserId != userId) throw new UnauthorizedAccessException();

            if (request.Name != null) client.Name = request.Name;
            if (request.Email != null) client.Email = request.Email;
            if (request.Phone != null) client.Phone = request.Phone;
            if (request.Company != null) client.Company = request.Company;
            if (request.Status != null) client.Status = request.Status;

            await clientRepo.UpdateAsync(client);
            return MapToResponse(client);
        }

        public async Task DeleteAsync(Guid id, Guid userId)
        {
            var client = await clientRepo.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("Client not found");
            if (client.UserId != userId) throw new UnauthorizedAccessException();
            await clientRepo.DeleteAsync(id);
        }

        private static ClientResponse MapToResponse(Client c)
            => new(
                c.Id, c.Name, c.Email, c.Phone, c.Company, c.Status,
                c.LastContactedAt,
                c.Projects?.Count(p => p.Status == ProjectStatus.Active) ?? 0,
                c.CreatedAt
            );
    }
}

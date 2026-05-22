using Freelance_bot.Application.Feature.Client.Request;
using Freelance_bot.Application.Feature.Client.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance_bot.Application.IServieces
{
    public interface IClientService
    {
        Task<ClientResponse> CreateAsync(Guid userId, CreateClientRequest request);
        Task<ClientResponse?> GetByIdAsync(Guid id, Guid userId);
        Task<IEnumerable<ClientResponse>> GetAllAsync(Guid userId);
        Task<ClientResponse> UpdateAsync(Guid id, Guid userId, UpdateClientRequest request);
        Task DeleteAsync(Guid id, Guid userId);
    }
}

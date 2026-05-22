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
    public class Repository<T>(FreelancerDbContext db) : IRepository<T> where T : BaseEntity
    {
        protected readonly FreelancerDbContext _db = db;
        protected readonly DbSet<T> _set = db.Set<T>();

        public virtual async Task<T?> GetByIdAsync(Guid id)
            => await _set.FindAsync(id);

        public virtual async Task<IEnumerable<T>> GetAllAsync()
            => await _set.ToListAsync();

        public virtual async Task<T> AddAsync(T entity)
        {
            await _set.AddAsync(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            _set.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"{typeof(T).Name} not found");
            _set.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(Guid id)
            => await _set.AnyAsync(e => e.Id == id);
    }

}

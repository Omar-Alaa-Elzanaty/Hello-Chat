using Hello.Domain.Interfaces.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hello.Persistence.Repos
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly HelloDbContext _context;

        public BaseRepository(HelloDbContext context)
        {
            _context = context;
        }

        public IQueryable<T> Entities => _context.Set<T>();

        public async Task AddAsync(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            await _context.AddAsync(entity);
        }

        public async Task AddRange(IEnumerable<T> entities)
        {
            ArgumentNullException.ThrowIfNull(entities);

            await _context.AddRangeAsync(entities);
        }

        public void Delete(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            _context.Remove(entity);
        }

        public void Update(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            _context.Update(entity);
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            _context.UpdateRange(entities);
        }
    }
}

using G01.Context;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Classes
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        // Database connection
        private readonly GymDbContext _dbContext;
        private readonly DbSet<TEntity> _Set;
        public GenericRepository(GymDbContext dbContext)
        {
            _dbContext = dbContext;
            // regester service in program.cs

            _Set = _dbContext.Set<TEntity>();
        }
        public async void AddAsync(TEntity entity)
        {
           _Set.Add(entity); // add local 

        }

        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predict, CancellationToken ct = default)
        {
            return _Set.AsNoTracking().AnyAsync(predict, ct);
        }

        public async void DeleteAsync(TEntity entity)
        {
           _Set.Remove(entity);
            
        }

        public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predict, bool tracking = false, CancellationToken ct = default)
        {
            IQueryable<TEntity> query = tracking ? _Set : _Set.AsNoTracking();
            return query.FirstOrDefaultAsync(predict, ct);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool tracking = false, CancellationToken ct = default)
        {
            IQueryable<TEntity> query = tracking ? _Set : _Set.AsNoTracking();
            return await query.ToListAsync(ct);
        }

        public async Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default)
            => await _Set.FindAsync( id , ct);

        public async void UpdateAsync(TEntity entity)
        {
            _Set.Update(entity);
        }
    }
}

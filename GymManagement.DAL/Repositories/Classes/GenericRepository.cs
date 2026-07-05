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
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
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
        public async void UpdateAsync(TEntity entity)
        {
            _Set.Update(entity);
        }
        public async void DeleteAsync(TEntity entity)
        {
            _Set.Remove(entity);

        }



        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
            => _Set.AsNoTracking().AnyAsync(predicate, ct);

        public Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken ct = default)
            => predicate is null ? _Set.AsNoTracking().CountAsync(ct) : _Set.AsNoTracking().CountAsync(predicate, ct);



        public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool tracking = false,
                                                        CancellationToken ct = default)
        {
            IQueryable<TEntity> query = tracking ? _Set : _Set.AsNoTracking();
            return await query.FirstOrDefaultAsync(predicate, ct);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool tracking = false, CancellationToken ct = default)
        {
            IQueryable<TEntity> query = tracking ? _Set : _Set.AsNoTracking();
            return await query.ToListAsync(ct);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null, bool tracking = false, CancellationToken ct = default)
        {
            IQueryable<TEntity> query = tracking ? _Set : _Set.AsNoTracking();
            if (predicate is not null) query = query.Where(predicate);
            return await query.ToListAsync(ct);
        }

        public async Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default)
            => await _Set.FindAsync( [id] , ct);

        
    }
}

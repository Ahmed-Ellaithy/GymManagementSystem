using G01.Context;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        // database connection
        private readonly GymDbContext _dbContext;
        private readonly Dictionary<string, object> _repositories = [];
        public UnitOfWork(GymDbContext dbContext) 
        {
            _dbContext = dbContext;

        }
        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            // check if repo exist or not >> IDictionary<>
            // IGenericRepository<Member> => Name
            var TypeName = typeof(TEntity).Name;
            // if exist in dictionary => use it 
            if (_repositories.TryGetValue(TypeName, out object? value))
                return (IGenericRepository<TEntity>)value;

            // if not ,, create Repo => add dictionary => return repo 
            else
            {
                var repo = new GenericRepository<TEntity>(_dbContext);
                _repositories[TypeName] = repo;
                return repo;
            }
        }



        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
            => await _dbContext.SaveChangesAsync(ct);
    }
}

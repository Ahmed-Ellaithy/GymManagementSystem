using GymManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        // Get by id
        Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default);
        // Add
        Task<int> AddAsync(TEntity entity);
        // Update
        Task<int> UpdateAsync(TEntity entity);
        // Delete
        Task<int> DeleteAsync(TEntity entity);
        // Get all
        Task<IEnumerable<TEntity>> GetAllAsync(bool tracking = false, CancellationToken ct = default);


        Task<bool> AnyAsync(Expression<Func<TEntity , bool>> predict , CancellationToken ct = default);

        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predict, bool tracking = false , CancellationToken ct = default);

    }
}

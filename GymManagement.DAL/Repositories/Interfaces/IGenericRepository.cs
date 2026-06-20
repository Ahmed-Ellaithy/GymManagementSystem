using GymManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}

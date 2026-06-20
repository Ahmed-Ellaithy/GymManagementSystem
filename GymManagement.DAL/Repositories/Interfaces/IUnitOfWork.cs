using GymManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        // Get Repo 
        IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new();
        // Save Changes
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}

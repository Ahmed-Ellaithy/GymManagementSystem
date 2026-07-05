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
        public IMembershipRepository MembershipRepository { get; }
        public ISessionRepository SessionRepository { get; }
        public IBookingRepository BookingRepository { get; }
        // Get Repo 
        IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity;
        // Save Changes
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}

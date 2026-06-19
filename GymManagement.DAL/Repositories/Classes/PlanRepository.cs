using G01.Context;
using G01.Models;
using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Classes
{
    public class PlanRepository : IPlanRepository
    {
        private readonly GymDbContext dbcontext;
        public PlanRepository(GymDbContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public async Task<int> AddAsync(Plan plan, CancellationToken ct = default)
        {
            dbcontext.Plans.Add(plan);
            return await dbcontext.SaveChangesAsync(ct);
        }

        public Task<int> DeleteAsync(Plan plan, CancellationToken ct = default)
        {
            dbcontext.Plans.Remove(plan);
            return dbcontext.SaveChangesAsync(ct);
        }

        public async Task<IEnumerable<Plan>> GetAllAsync(bool tracking = false, CancellationToken ct = default)
        {
            //if(tracking)
            //    return await dbcontext.Plans.ToListAsync(ct);
            //else
            //    return await dbcontext.Plans.AsNoTracking().ToListAsync(ct);
            IQueryable<Plan> query = tracking ? dbcontext.Plans : dbcontext.Plans.AsNoTracking();
            return await query.ToListAsync(ct);
        }

        public async Task<Plan?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await dbcontext.Plans.FindAsync(id, ct);
        }

        public Task<int> UpdateAsync(Plan plan, CancellationToken ct = default)
        {
            dbcontext.Plans.Update(plan);
            return dbcontext.SaveChangesAsync(ct);
        }
    }
}
